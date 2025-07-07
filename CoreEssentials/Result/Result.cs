using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoreEssentials.Result
{
    public class Result<TResult, TError>
    {
        private readonly TResult _value;
        private readonly TError _error;
        private readonly bool _isSuccess;
        private readonly SerializableException? _ex;

        private Result(TResult value)
        {
            _value = value;
            _error = default!;
            _isSuccess = true;
        }

        private Result(TError error)
        {
            _value = default!;
            _error = error;
            _isSuccess = false;
            if (error is Exception ex)
            {
                _ex = new SerializableException(ex);
            }
        }

        /// <summary>
        /// Creates a successful result containing the specified value.
        /// </summary>
        /// <param name="value">The value to be encapsulated in the successful result.</param>
        /// <returns>A <see cref="Result{TResult, TError}"/> instance representing a successful operation with the provided
        /// value.</returns>
        public static Result<TResult, TError> Ok(TResult value) => new Result<TResult, TError>(value);

        /// <summary>
        /// Creates a result representing an error.
        /// </summary>
        /// <param name="error">The error value to associate with the result. Cannot be null.</param>
        /// <returns>A <see cref="Result{TResult, TError}"/> instance containing the specified error.</returns>
        public static Result<TResult, TError> Err(TError error) => new Result<TResult, TError>(error);

        /// <summary>
        /// Determines whether the current operation was successful.
        /// </summary>
        /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
        public bool IsOk() => _isSuccess;

        /// <summary>
        /// Determines whether the current operation resulted in an error.
        /// </summary>
        /// <returns><see langword="true"/> if the operation was unsuccessful; otherwise, <see langword="false"/>.</returns>
        public bool IsErr() => !_isSuccess;

        /// <summary>
        /// Gets the value of the result if the operation was successful.
        /// </summary>
        public TResult Value
        {
            get
            {
                if (!_isSuccess)
                    throw new InvalidOperationException("Called success Value on an Error Result");
                return _value;
            }
        }

        /// <summary>
        /// Gets the error associated with a failed result.
        /// </summary>
        public TError Error
        {
            get
            {
                if (_isSuccess)
                    throw new InvalidOperationException("Called Error on a Success Result");
                return _error;
            }
        }

        /// <summary>
        /// Returns the contained value if the operation was successful; otherwise, returns the specified default value.
        /// </summary>
        /// <param name="defaultValue">The value to return if the operation was not successful.</param>
        /// <returns>The contained value if the operation was successful; otherwise, the specified default value.</returns>
        public TResult UnwrapOr(TResult defaultValue) => _isSuccess ? _value : defaultValue;

        /// <summary>
        /// Retrieves the result value if the operation was successful, or throws an exception if it failed.
        /// </summary>
        /// <remarks>If the operation was unsuccessful, this method throws the exception associated with
        /// the failure,  or a generic exception containing the error details if no specific exception is
        /// available.</remarks>
        /// <returns>The result value of type <typeparamref name="TResult"/> if the operation was successful.</returns>
        /// <exception cref="Exception">Thrown if the operation was unsuccessful and no specific exception is available. The exception message
        /// contains the error details.</exception>
        public TResult Unwrap()
        {
            if(_isSuccess)
            {
                return _value;
            }
            else
            {
                if(_ex != null)
                {
                    throw _ex.ToException();
                }
                else
                {
                    throw new Exception(ErrorAsString());
                }
            }   
        }

        /// <summary>
        /// Determines whether the current exception is caused by or derived from the specified exception type.
        /// </summary>
        /// <remarks>This method attempts to match the type of the current exception against the specified
        /// exception type <typeparamref name="T"/>. It performs a series of checks to resolve the exception type,
        /// including searching loaded assemblies and handling ambiguous matches. If the exception type cannot be
        /// resolved, the method returns <see langword="false"/>.
        /// Inspired by https://github.com/microsoft/durabletask-dotnet/blob/main/src/Abstractions/TaskFailureDetails.cs</remarks>
        /// <typeparam name="T">The type of exception to check against. Must derive from <see cref="System.Exception"/>.</typeparam>
        /// <returns><see langword="true"/> if the current exception is of type <typeparamref name="T"/> or a derived type;
        /// otherwise, <see langword="false"/>.</returns>
        /// <exception cref="AmbiguousMatchException">Thrown if multiple exception types with the same name are found in the loaded assemblies.</exception>
        public bool IsCausedBy<T>() where T : Exception
        {
            var targetBaseExceptionType = typeof(T);

            if (!IsErr() || _ex is null)
            {
                return false;
            }

            // This check works for .NET exception types defined in System.Core.PrivateLib (aka mscorelib.dll)
            var loadedExceptionType = Type.GetType(_ex.ExceptionType, throwOnError: false);

            // For exception types defined in the same assembly as the target exception type.
            loadedExceptionType ??= targetBaseExceptionType.Assembly.GetType(_ex.ExceptionType, throwOnError: false);

            // For custom exception types defined in the app's assembly
            loadedExceptionType ??= Assembly.GetCallingAssembly().GetType(_ex.ExceptionType);

            if (loadedExceptionType is null)
            {
                // This last check works for exception types defined in any loaded assembly (e.g. NuGet packages, etc.).
                // This is a fallback that should rarely be needed except in obscure cases.
                List<Type> matchingExceptionTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetType(_ex.ExceptionType, throwOnError: false))
                    .Where(t => t != null)
                    .ToList();
                if (matchingExceptionTypes.Count == 1)
                {
                    loadedExceptionType = matchingExceptionTypes[0];
                }
                else if (matchingExceptionTypes.Count > 1)
                {
                    throw new AmbiguousMatchException($"Multiple exception types with the name '{_ex.ExceptionType}' were found.");
                }
            }

            if (loadedExceptionType is null)
            {
                // The actual exception type could not be loaded, so we cannot determine if it matches the target type.
                return false;
            }

            return targetBaseExceptionType.IsAssignableFrom(loadedExceptionType);
        }

        /// <summary>
        /// Implicitly converts a value of type <typeparamref name="TResult"/> to a <see cref="Result{TResult,
        /// TError}"/> instance.
        /// </summary>
        /// <param name="value">The value to be wrapped in a <see cref="Result{TResult, TError}"/> as a successful result.</param>
        public static implicit operator Result<TResult, TError>(TResult value) => Ok(value);

        /// <summary>
        /// Implicitly converts a value of type <typeparamref name="TError"/> to a <see cref="Result{TResult, TError}"/>
        /// representing an error.
        /// </summary>
        /// <remarks>This operator provides a convenient way to create an error result without explicitly
        /// calling the <c>Err</c> method.</remarks>
        /// <param name="error">The error value to be encapsulated in the <see cref="Result{TResult, TError}"/>.</param>
        public static implicit operator Result<TResult, TError>(TError error) => Err(error);

        #region :: Private methods ::

        private string? ErrorAsString()
        {
            if (_isSuccess)
                return null;
            return _error?.ToString();
        }

        #endregion

    }
}

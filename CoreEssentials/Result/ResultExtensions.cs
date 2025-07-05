using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreEssentials.Result
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Determines whether the result represents a successful operation and, if so, executes the specified action.
        /// </summary>
        /// <remarks>This method checks whether the result is successful and iff the
        /// result is successful,  the specified <paramref name="action"/> is invoked with the value contained in the
        /// result.</remarks>
        /// <typeparam name="TResult">The type of the value contained in the result if the operation is successful.</typeparam>
        /// <typeparam name="TError">The type of the error contained in the result if the operation fails.</typeparam>
        /// <param name="result">The result to evaluate. Must not be null.</param>
        /// <param name="action">The action to execute if the result represents a successful operation. The action is passed the value of the
        /// result.</param>
        /// <returns><see langword="true"/> if the result represents a successful operation; otherwise, <see langword="false"/>.</returns>
        public static bool IsOkAnd<TResult, TError>(this Result<TResult, TError> result, Action<TResult> action)
        {
            if (result.IsOk())
            {
                action(result.Value);
            }
            return result.IsOk();
        }

        /// <summary>
        /// Determines whether the result is successful and, if so, executes the specified asynchronous action.
        /// </summary>
        /// <remarks>This method checks whether the result is successful and if the
        /// result is successful,  the provided asynchronous action is executed with the result value and the
        /// cancellation token. The method returns the success status of the result.</remarks>
        /// <typeparam name="TResult">The type of the successful result value.</typeparam>
        /// <typeparam name="TError">The type of the error value.</typeparam>
        /// <param name="result">The result to evaluate. Must not be null.</param>
        /// <param name="action">An asynchronous action to execute if the result is successful. The action receives the successful result
        /// value  and a <see cref="CancellationToken"/> for cancellation support.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/> if not
        /// specified.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the result
        /// is  successful; otherwise, <see langword="false"/>.</returns>
        public static async Task<bool> IsOkAndAsync<TResult, TError>(this Result<TResult, TError> result, 
            Func<TResult, CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            if (result.IsOk())
            {
                await action(result.Value, cancellationToken);
            }
            return result.IsOk();
        }

        /// <summary>
        /// Determines whether the result represents an error and, if so, executes the specified action.
        /// </summary>
        /// <remarks>If the result represents an error, the specified <paramref name="action"/> is invoked
        /// with the error value. This method is useful for handling errors in a functional style while also checking
        /// the error state.</remarks>
        /// <typeparam name="TResult">The type of the successful result value.</typeparam>
        /// <typeparam name="TError">The type of the error value.</typeparam>
        /// <param name="result">The result to evaluate. Must not be <see langword="null"/>.</param>
        /// <param name="action">The action to execute if the result represents an error. Must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the result represents an error; otherwise, <see langword="false"/>.</returns>
        public static bool IsErrAnd<TResult, TError>(this Result<TResult, TError> result, Action<TError> action)
        {
            if (result.IsErr())
            {
                action(result.Error);
            }
            return result.IsErr();
        }

        /// <summary>
        /// Determines whether the result represents an error and, if so, executes the specified asynchronous action.
        /// </summary>
        /// <remarks>This method evaluates whether the result is an error using <c>IsErr()</c>. If the
        /// result is an error, the provided <paramref name="action"/> is invoked asynchronously with the error value
        /// and the cancellation token. The method always returns the result of <c>IsErr()</c>, regardless of whether
        /// the action was executed.</remarks>
        /// <typeparam name="TResult">The type of the successful result value.</typeparam>
        /// <typeparam name="TError">The type of the error value.</typeparam>
        /// <param name="result">The result to evaluate. Must not be null.</param>
        /// <param name="action">An asynchronous action to execute if the result represents an error. The action receives the error value and
        /// a <see cref="CancellationToken"/> for cooperative cancellation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the result
        /// represents an error; otherwise, <see langword="false"/>.</returns>
        public static async Task<bool> IsErrAndAsync<TResult, TError>(this Result<TResult, TError> result,
            Func<TError, CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            if (result.IsErr())
            {
                await action(result.Error, cancellationToken);
            }
            return result.IsErr();
        }

        /// <summary>
        /// Transforms the successful result value of the current <see cref="Result{TResult, TError}"/>  into a new
        /// result using the specified mapping function.
        /// </summary>
        /// <typeparam name="TResult">The type of the value in the original result.</typeparam>
        /// <typeparam name="TError">The type of the error in the original result.</typeparam>
        /// <typeparam name="TUResult">The type of the value in the transformed result.</typeparam>
        /// <param name="result">The original result to be transformed. Must not be null.</param>
        /// <param name="mapFunc">A function that maps the value of the original result to a new value.  This function is only invoked if the
        /// original result represents a successful state.</param>
        /// <returns>A new <see cref="Result{TUResult, TError}"/> containing the transformed value if the original result 
        /// represents success, or the original error if the result represents failure.</returns>
        public static Result<TUResult, TError> Map<TResult, TError, TUResult>(this Result<TResult, TError> result, 
            Func<TResult, TUResult> mapFunc)
        {
            if (result.IsOk())
            {
                return mapFunc(result.Value);
            }
            return result.Error;
        }

        /// <summary>
        /// Asynchronously maps the value of a successful <see cref="Result{TResult, TError}"/> to a new result type
        /// using the specified mapping function.
        /// </summary>
        /// <remarks>This method is useful for transforming the value of a successful result into a
        /// different type while preserving the error state if the original result is not successful.</remarks>
        /// <typeparam name="TResult">The type of the value in the original result.</typeparam>
        /// <typeparam name="TError">The type of the error in the result.</typeparam>
        /// <typeparam name="TUResult">The type of the value in the mapped result.</typeparam>
        /// <param name="result">The original result to map. Must be a successful result to perform the mapping.</param>
        /// <param name="mapFunc">A function that asynchronously maps the value of the original result to a new value. The function takes the
        /// original value and a <see cref="CancellationToken"/> as parameters.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="Result{TUResult, TError}"/> containing the mapped value if the original result was successful,
        /// or the original error if the result was not successful.</returns>
        public static async Task<Result<TUResult, TError>> MapAsync<TResult, TError, TUResult>(
            this Result<TResult, TError> result, Func<TResult, CancellationToken, Task<TUResult>> mapFunc,
            CancellationToken cancellationToken = default)
        {
            if (result.IsOk())
            {
                var mappedValue = await mapFunc(result.Value, cancellationToken);
                return mappedValue;
            }
            return result.Error;
        }

        /// <summary>
        /// Transforms the value of a successful <see cref="Result{TResult, TError}"/> using the specified mapping
        /// function,  or returns a default value if the result represents an error.
        /// </summary>
        /// <remarks>This method is useful for handling results where you want to apply a transformation
        /// to the value of a successful result  while providing a fallback value for error cases.</remarks>
        /// <typeparam name="TResult">The type of the value contained in the successful result.</typeparam>
        /// <typeparam name="TError">The type of the error contained in the failed result.</typeparam>
        /// <typeparam name="TUResult">The type of the value returned by the mapping function or the default value.</typeparam>
        /// <param name="result">The <see cref="Result{TResult, TError}"/> instance to evaluate.</param>
        /// <param name="mapFunc">A function to transform the value of a successful result.</param>
        /// <param name="defaultVal">The value to return if the result represents an error.</param>
        /// <returns>The transformed value if the result is successful; otherwise, the specified default value.</returns>
        public static TUResult MapOr<TResult, TError, TUResult>(this Result<TResult, TError> result,
            Func<TResult, TUResult> mapFunc, TUResult defaultVal)
        {
            if (result.IsOk())
            {
                return mapFunc(result.Value);
            }
            return defaultVal;
        }

        /// <summary>
        /// Transforms the value of a successful <see cref="Result{TResult, TError}"/> instance asynchronously  using
        /// the specified mapping function, or returns a default value if the result represents an error.
        /// </summary>
        /// <typeparam name="TResult">The type of the value contained in the <see cref="Result{TResult, TError}"/>.</typeparam>
        /// <typeparam name="TError">The type of the error contained in the <see cref="Result{TResult, TError}"/>.</typeparam>
        /// <typeparam name="TUResult">The type of the result produced by the mapping function or the default value.</typeparam>
        /// <param name="result">The <see cref="Result{TResult, TError}"/> instance to evaluate.</param>
        /// <param name="mapFunc">A function that asynchronously maps the value of a successful result to a new value. The function takes the
        /// value of type <typeparamref name="TResult"/> and a <see cref="CancellationToken"/> as input.</param>
        /// <param name="defaultVal">The value to return if the <paramref name="result"/> represents an error.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the execution of <paramref name="mapFunc"/>. Defaults to
        /// <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is either the transformed value  produced
        /// by <paramref name="mapFunc"/> if the <paramref name="result"/> is successful, or  <paramref
        /// name="defaultVal"/> if the <paramref name="result"/> represents an error.</returns>
        public static async Task<TUResult> MapOrAsync<TResult, TError, TUResult>(
            this Result<TResult, TError> result, Func<TResult, CancellationToken, Task<TUResult>> mapFunc,
            TUResult defaultVal, CancellationToken cancellationToken = default)
        {
            if (result.IsOk())
            {
                return await mapFunc(result.Value, cancellationToken);
            }
            return defaultVal;
        }


        /// <summary>
        /// Transforms the error value of a <see cref="Result{TResult, TError}"/> into a new error type using the
        /// specified mapping function.
        /// </summary>
        /// <typeparam name="TResult">The type of the success value in the <see cref="Result{TResult, TError}"/>.</typeparam>
        /// <typeparam name="TError">The type of the original error value in the <see cref="Result{TResult, TError}"/>.</typeparam>
        /// <typeparam name="TUError">The type of the transformed error value.</typeparam>
        /// <param name="result">The <see cref="Result{TResult, TError}"/> instance to transform.</param>
        /// <param name="mapFunc">A function that maps the original error value to the new error type. This function is only invoked if the
        /// <paramref name="result"/> represents an error.</param>
        /// <returns>A <see cref="Result{TResult, TUError}"/> containing the original success value if <paramref name="result"/>
        /// represents success,  or the transformed error value if <paramref name="result"/> represents an error.</returns>
        public static Result<TResult, TUError> MapErr<TResult, TError, TUError>(this Result<TResult, TError> result,
            Func<TError, TUError> mapFunc)
        {
            if (result.IsErr())
            {
                return mapFunc(result.Error);
            }
            return result.Value;
        }

        /// <summary>
        /// Asynchronously maps the error value of a <see cref="Result{TResult, TError}"/> to a new error type.
        /// </summary>
        /// <remarks>This method is useful for transforming the error type of a <see cref="Result{TResult,
        /// TError}"/>  in scenarios where the error needs to be processed or converted asynchronously. If the <paramref
        /// name="result"/> represents a success, the original success value is returned unchanged.</remarks>
        /// <typeparam name="TResult">The type of the success value in the <see cref="Result{TResult, TError}"/>.</typeparam>
        /// <typeparam name="TError">The type of the original error value in the <see cref="Result{TResult, TError}"/>.</typeparam>
        /// <typeparam name="TUError">The type of the new error value after mapping.</typeparam>
        /// <param name="result">The <see cref="Result{TResult, TError}"/> instance to map.</param>
        /// <param name="mapFunc">A function that asynchronously maps the original error value to a new error value. The function takes the
        /// original error value and a <see cref="CancellationToken"/> as parameters.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result is a  <see
        /// cref="Result{TResult, TUError}"/> containing either the original success value or the mapped error value.</returns>
        public static async Task<Result<TResult, TUError>> MapErrAsync<TResult, TError, TUError>(
            this Result<TResult, TError> result, Func<TError, CancellationToken, Task<TUError>> mapFunc,
            CancellationToken cancellationToken = default)
        {
            if (result.IsErr())
            {
                var mappedError = await mapFunc(result.Error, cancellationToken);
                return mappedError;
            }
            return result.Value;
        }

        /// <summary>
        /// Transforms the value of a <see cref="Result{TResult, TError}"/> into a new type based on whether the result
        /// is successful or an error.
        /// </summary>
        /// <remarks>This method provides a way to map both the success and error cases of a <see
        /// cref="Result{TResult, TError}"/>  into a unified type, enabling flexible handling of both
        /// outcomes.</remarks>
        /// <typeparam name="TResult">The type of the value contained in the successful result.</typeparam>
        /// <typeparam name="TError">The type of the error contained in the failed result.</typeparam>
        /// <typeparam name="TUResult">The type of the transformed result.</typeparam>
        /// <param name="result">The <see cref="Result{TResult, TError}"/> instance to transform.</param>
        /// <param name="okMapFunc">A function to apply to the value if the result is successful.</param>
        /// <param name="errMapFunc">A function to apply to the error if the result is a failure.</param>
        /// <returns>The transformed result of type <typeparamref name="TUResult"/>. If the result is successful,  <paramref
        /// name="okMapFunc"/> is applied to the value; otherwise, <paramref name="errMapFunc"/> is applied to the
        /// error.</returns>
        public static TUResult MapOrElse<TResult, TError, TUResult>(this Result<TResult, TError> result,
            Func<TResult, TUResult> okMapFunc, Func<TError, TUResult> errMapFunc)
        {
            if (result.IsOk())
            {
                return okMapFunc(result.Value);
            }
            return errMapFunc(result.Error);
        }

        /// <summary>
        /// Transforms the value of a <see cref="Result{TResult, TError}"/> into a new result asynchronously,  using one
        /// of two mapping functions depending on whether the result represents success or failure.
        /// </summary>
        /// <remarks>This method applies the appropriate mapping function based on whether the <paramref
        /// name="result"/> is in a success or failure state: - If the result is successful, <paramref
        /// name="okMapFunc"/> is invoked with the success value. - If the result represents an error, <paramref
        /// name="errMapFunc"/> is invoked with the error value.</remarks>
        /// <typeparam name="TResult">The type of the success value in the original result.</typeparam>
        /// <typeparam name="TError">The type of the error value in the original result.</typeparam>
        /// <typeparam name="TUResult">The type of the result produced by the mapping functions.</typeparam>
        /// <param name="result">The <see cref="Result{TResult, TError}"/> instance to transform.</param>
        /// <param name="okMapFunc">A function to asynchronously map the success value of the result to a new value. The function takes the
        /// success value and a <see cref="CancellationToken"/> as parameters.</param>
        /// <param name="errMapFunc">A function to asynchronously map the error value of the result to a new value. The function takes the error
        /// value and a <see cref="CancellationToken"/> as parameters.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the transformed value of type
        /// <typeparamref name="TUResult"/>.</returns>
        public static async Task<TUResult> MapOrElseAsync<TResult, TError, TUResult>(
            this Result<TResult, TError> result, Func<TResult, CancellationToken, Task<TUResult>> okMapFunc,
            Func<TError, CancellationToken, Task<TUResult>> errMapFunc, CancellationToken cancellationToken = default)
        {
            if (result.IsOk())
            {
                var okValue = await okMapFunc(result.Value, cancellationToken);
                return okValue;
            }
            var errValue = await errMapFunc(result.Error, cancellationToken);
            return errValue;
        }

    }
}

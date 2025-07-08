using System;
using System.Reflection;
using System.Text.Json.Serialization;
using CoreEssentials.Utils;

namespace CoreEssentials.Result
{
    /// <summary>
    /// Represents a serializable version of an <see cref="Exception"/> that can be used to capture and reconstruct
    /// exception details across application boundaries.
    /// </summary>
    /// <remarks>This class is designed to serialize the key properties of an <see cref="Exception"/> object,
    /// including its type name, message, source, stack trace, and inner exception. It can be used to transfer exception
    /// details between processes or systems where the original exception type may not be available. Use the <see
    /// cref="ToException"/> method to reconstruct the original exception or a close approximation.
    /// Note that this class does not capture custom properties or fields of the original exception.
    /// </remarks>
    internal sealed class SerializableException
    {
        public string? ExceptionType { get; set; }
        public string? Message { get; set; }
        public string? Source { get; set; }
        public string? StackTrace { get; set; }
        public SerializableException? InnerException { get; set; }

        [JsonConstructor]
        public SerializableException() { }

        /// <summary>
        /// Constructor that now captures custom properties.
        /// </summary>
        public SerializableException(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            ExceptionType = ex.GetType().FullName;
            Message = ex.Message;
            Source = ex.Source;
            StackTrace = ex.StackTrace;
            if (ex.InnerException != null)
            {
                InnerException = new SerializableException(ex.InnerException);
            }
        }

        /// <summary>
        /// Reconstruction method that now restores custom properties.
        /// </summary>

        public Exception ToException()
        {
            Exception? inner = InnerException?.ToException();
            Exception? reconstructedException = null;

            // Create the instance (same logic as before)
            Type? type = Type.GetType(ExceptionType ?? string.Empty);
            if (type != null)
            {
                try 
                {
                    reconstructedException = ReflectionUtils.CreateInstance<Exception>(type, Message!, inner!);
                }
                //add or AmbiguousMatchException
                catch (SystemException)
                {
                    try 
                    { 
                        reconstructedException = ReflectionUtils.CreateInstance<Exception>(type, Message!);
                    }
                    catch (SystemException)
                    {
                        try 
                        { 
                            reconstructedException = ReflectionUtils.CreateInstance<Exception>(type); 
                        } 
                        catch 
                        {
                            //log the error or handle it as needed
                        }
                    }
                }
            }
            reconstructedException ??= new Exception(Message, inner);

            // Restore stack trace (same logic as before)
            if (!string.IsNullOrEmpty(StackTrace))
            {
                ReflectionUtils.SetInstanceField(reconstructedException, typeof(Exception), "_remoteStackTraceString", StackTrace);
            }

            reconstructedException.Source = this.Source;
            return reconstructedException;
        }
    }
}

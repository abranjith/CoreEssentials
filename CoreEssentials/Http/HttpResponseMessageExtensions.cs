using CoreEssentials.Result;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CoreEssentials.Http
{
    public static class HttpResponseMessageExtensions
    {

        /// <summary>
        /// Converts an <see cref="HttpResponseMessage"/> into a <see cref="Result{TSuccess, TError}"/> object by
        /// deserializing the response content into either a success or error type.
        /// </summary>
        /// <remarks>This method uses <see cref="HttpContent.ReadFromJsonAsync{T}(JsonSerializerOptions?,
        /// CancellationToken)"/> to deserialize the response content. Ensure that the response content matches the
        /// expected JSON structure for <typeparamref name="TSuccess"/> or <typeparamref name="TError"/>.</remarks>
        /// <typeparam name="TSuccess">The type to which the response content is deserialized if the HTTP response indicates success.</typeparam>
        /// <typeparam name="TError">The type to which the response content is deserialized if the HTTP response indicates failure.</typeparam>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to process.</param>
        /// <param name="options">Optional <see cref="JsonSerializerOptions"/> to customize the JSON deserialization process. Defaults to <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the asynchronous operation to complete.
        /// Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="Result{TSuccess, TError}"/> object containing the deserialized content. If the HTTP response
        /// indicates success, the result contains the deserialized <typeparamref name="TSuccess"/> value. If the HTTP
        /// response indicates failure, the result contains the deserialized <typeparamref name="TError"/> value.</returns>
        public async static Task<Result<TSuccess, TError>> AsResult<TSuccess, TError>(this HttpResponseMessage response,
            JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = response.HasContent()
                        ? await response.Content.ReadFromJsonAsync<TSuccess>(options, cancellationToken)
                        : default;
                return content ?? default!;
            }
            else
            {
                var content = response.HasContent()
                        ? await response.Content.ReadFromJsonAsync<TError>(options, cancellationToken)
                        : default;
                return content ?? default!;
            }
        }

        /// <summary>
        /// Determines whether the HTTP response message contains content.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to check for content.</param>
        /// <returns><see langword="true"/> if the response contains content; otherwise, <see langword="false"/>.</returns>
        private static bool HasContent(this HttpResponseMessage response)
        {
            if (response?.Content == null)
                return false;

            // If ContentLength is explicitly set and is greater than 0, we have content
            if (response.Content.Headers.ContentLength.HasValue)
                return response.Content.Headers.ContentLength.Value > 0;

            // If ContentLength is not set, we assume there might be content
            // This handles cases like chunked transfer encoding
            return true;
        }
    }
}

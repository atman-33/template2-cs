using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Template2.Infrastructure.RestApi
{
    internal static class RestApiHelper
    {
        /// <summary>
        /// GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="createEntity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static async Task<IReadOnlyList<T>> Get<T>(
            string requestUri,
            Func<JsonElement, T> createEntity)
        {
            var result = new List<T>();

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    //// Handle HTTP error
                    throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (!string.IsNullOrEmpty(json))
                {
                    var jsonElementList = JsonSerializer.Deserialize<List<JsonElement>>(json, options);

                    if (jsonElementList == null)
                    {
                        return result;
                    }

                    foreach (var jsonElement in jsonElementList)
                    {
                        if (createEntity == null)
                        {
                            throw new ArgumentNullException(nameof(createEntity));
                        }

                        result.Add(createEntity(jsonElement));
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                //// Handle HTTP request exception
                //// Log or rethrow as needed
                Console.WriteLine($"HTTP request failed: {ex.Message}");
            }
            catch (JsonException ex)
            {
                //// Handle JSON deserialization exception
                //// Log or rethrow as needed
                Console.WriteLine($"JSON deserialization failed: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="Exception"></exception>
        internal async static Task<HttpResponseMessage> PostAsync(string requestUri, StringContent content)
        {
            using var client = new HttpClient();
            try
            {
                Debug.WriteLine($"PostAsync reauest uri: ${ requestUri}");
                Debug.WriteLine($"PostAsync content: ${content}");

                var response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"An error occurred while sending the request. Request URI: {requestUri}. {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred. Request URI: {requestUri}. {ex.Message}", ex);
            }
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="Exception"></exception>
        internal async static Task<HttpResponseMessage> DeleteAsync(string requestUri, string query)
        {
            var uri = $"{requestUri}?{query}";
            Debug.WriteLine($"DeleteAsync uri: ${uri}");

            using var client = new HttpClient();
            try
            {
                var response = await client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"An error occurred while sending the request. Request URI: {uri}. {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred. Request URI: {uri}. {ex.Message}", ex);
            }
        }


    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class WebApiClient : IDisposable
    {
        private HttpClient _client;

        private JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            DateFormatString = "yy MMM+dd",
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public WebApiClient(string baseAddress)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Brotli
            };

            _client = new HttpClient(handler, true);
            _client.BaseAddress = new Uri(baseAddress);

            _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("br"));
        }

        public HttpRequestHeaders GetHttpRequestHeaders()
        {
            return _client.DefaultRequestHeaders;
        }


        public async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            T result = ReadFromJson<T>(await response.Content.ReadAsStringAsync());
            return result;
        }
        public async Task<string> GetStringAsync(string requestUri)
        {
            var response = await _client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result.Trim('"');
        }
        public async Task<string> PostRequestAsync<T>(string requestUri, T content)
        {
            using var stringContent = WriteToJson(content);
            var response = await _client.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<T> PostAsync<T>(string requestUri, T content)
        {
            var body = await PostRequestAsync(requestUri, content);
            return ReadFromJson<T>(body);
        }

        public async Task PutAsync<T>(string requestUri, T content)
        {
            using StringContent stringContent = WriteToJson(content);
            var response = await _client.PutAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
        }

        private StringContent WriteToJson<T>(T content)
        {
            return new StringContent(JsonConvert.SerializeObject(content, JsonSerializerSettings), Encoding.UTF8, "application/json");
        }

        public async Task DeleteAsync(string requestUri)
        {
            var response = await _client.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
        }

        private T ReadFromJson<T>(string json)
        {
            var result = JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
            return result;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}

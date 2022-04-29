using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            var openApiClient = new swaggerClient("http://localhost:5000/", httpClient);
            var openApiUsers = openApiClient.UsersAllAsync().Result;


            using var client = new WebApiClient("http://localhost:5000/api/");
            var users = client.GetAsync<IEnumerable<User>>("Users").Result;
            var token = client.PostRequestAsync("Users/Login", users.First(x => x.Roles.HasFlag(Roles.Read))).Result;
            client.GetHttpRequestHeaders().Authorization = new AuthenticationHeaderValue("Bearer", token.Trim('"'));
            var orders = client.GetAsync<IEnumerable<Order>>("Orders").Result;

        }

        private static void ManualWay()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Brotli
            };
            using var httpClient = new HttpClient(handler, true);
            httpClient.BaseAddress = new Uri("http://localhost:5000/api/");

            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("br"));

            var response = httpClient.GetAsync("Users").Result;

            //sprawdzenie czy status code jest taki jak oczekujemy
            //if(response.StatusCode == System.Net.HttpStatusCode.OK)
            //return;

            //funkcja, która rzuca wyjątkiem jeśli status code jest spoza 2xx, albo nie robi nic
            response.EnsureSuccessStatusCode();

            //Sprawdzenie czy status code jest z puli 2xx
            if (!response.IsSuccessStatusCode)
                return;


            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateFormatString = "yy MMM+dd",
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };


            var body = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(body, settings);
            //var users = response.Content.ReadFromJsonAsync<IEnumerable<User>>(new System.Text.Json.JsonSerializerOptions() {  }).Result;

            response = httpClient.PostAsJsonAsync("Users/Login", users.First()).Result;

            using (var content = new StringContent(JsonConvert.SerializeObject(users.First(), settings), Encoding.UTF8, "application/json"))
            {
                response = httpClient.PostAsync("Users/Login", content).Result;
            }

            response.EnsureSuccessStatusCode();

            var token = response.Content.ReadAsStringAsync().Result.Trim('"');

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            response = httpClient.GetAsync("Orders").Result;
            response.EnsureSuccessStatusCode();

            body = response.Content.ReadAsStringAsync().Result;
            var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(body, settings);
        }
    }
}

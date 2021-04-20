// =============================
// BlazorSpread.net Sample
// =============================
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleClient
{
    class Program
    {
        //! Run Multiple startup projects. 

        // asp.net core url
        static readonly string _apiRoot = "https://localhost:44335/";

        const string MEDIA_TYPE = "application/x-msgpack";

        static void Main()
        {
            Console.WriteLine("Hello MessagePack!");
            Console.WriteLine($"Press any key when server {_apiRoot} is ready.");
            Console.ReadKey();

            GetData();
            GetData(1);
            PostData();
        }

        private static void GetData()
        {
            Console.WriteLine("\nGET");

            using var httpClient = new HttpClient { BaseAddress = new Uri(_apiRoot) };
            var result = httpClient.GetAsync("api/WeatherForecast").Result;
            var bytes = result.Content.ReadAsByteArrayAsync().Result;
            var data = MessagePackSerializer.Deserialize<List<WeatherForecast>>(bytes, ContractlessStandardResolver.Options);

            data.ForEach(item => Console.WriteLine(item));
        }

        private static void GetData(int id)
        {
            Console.WriteLine($"\nGET/{id}");

            using var httpClient = new HttpClient { BaseAddress = new Uri(_apiRoot) };
            var result = httpClient.GetAsync($"api/WeatherForecast/{id}").Result;
            var bytes = result.Content.ReadAsByteArrayAsync().Result;
            var item = MessagePackSerializer.Deserialize<WeatherForecast>(bytes, ContractlessStandardResolver.Options);

            Console.WriteLine(item);
        }

        private static void PostData()
        {
            Console.WriteLine("\nPOST");
            // post object
            var item = new WeatherForecast(DateTime.Now, 17, "Cool in Bogotá");

            using var httpClient = new HttpClient { BaseAddress = new Uri(_apiRoot) };
            var buffer = MessagePackSerializer.Serialize(item, ContractlessStandardResolver.Options);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue(MEDIA_TYPE);
            var result = httpClient.PostAsync("api/WeatherForecast", byteContent).Result;

            Console.WriteLine("\nResult: {0}", result.StatusCode);
        }
    }

    public record WeatherForecast(DateTime Date, int TemperatureC, string Summary);
}

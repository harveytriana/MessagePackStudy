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
        static readonly string _apiRoot = "https://localhost:44335/";

        static void Main()
        {
            Console.WriteLine("Hello MessagePack!");
            Console.WriteLine("Press any key when server is ready.");
            Console.ReadKey();

            GetData();

            PostData();

            //GetOne();
        }

        private static void GetData()
        {
            Console.WriteLine("\nGET");
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_apiRoot);
            var result = httpClient.GetAsync("WeatherForecast").Result;
            var bytes = result.Content.ReadAsByteArrayAsync().Result;
            var data = MessagePackSerializer.Deserialize<List<WeatherForecast>>(bytes, ContractlessStandardResolver.Options);

            data.ForEach(item => Console.WriteLine(item));
        }

        private static void GetOne()
        {
            Console.WriteLine("\nGET/1");
            using var httpClient = new HttpClient(); httpClient.BaseAddress = new Uri(_apiRoot);

            var result = httpClient.GetAsync("WeatherForecast/1").Result;
            var bytes = result.Content.ReadAsByteArrayAsync().Result;
            var item = MessagePackSerializer.Deserialize<WeatherForecast>(bytes, ContractlessStandardResolver.Options);

            Console.WriteLine(item);
        }

        private static void PostData()
        {
            Console.WriteLine("\nPOST");
            var item = new WeatherForecast(DateTime.Now, 17 + 1, "Cool in London");

            using var httpClient = new HttpClient(); httpClient.BaseAddress = new Uri(_apiRoot);

            var buffer = MessagePackSerializer.Serialize(item, ContractlessStandardResolver.Options);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-msgpack");
            var result = httpClient.PostAsync("WeatherForecast", byteContent).Result;

            Console.WriteLine("\nResult: {0}", result.StatusCode);
        }
    }

    public record WeatherForecast(DateTime Date, int TemperatureC, string Summary);
}

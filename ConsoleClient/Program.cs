﻿using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Net.Http;

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
        }

        private static void GetData()
        {
            Console.WriteLine("\nGET");
            using var httpClient = new HttpClient(); httpClient.BaseAddress = new Uri(_apiRoot);
            var result = httpClient.GetAsync("WeatherForecast").Result;
            var bytes = result.Content.ReadAsByteArrayAsync().Result;

            var data = MessagePackSerializer.Deserialize<List<WeatherForecast>>(bytes, ContractlessStandardResolver.Options);

            data.ForEach(x => {
                Console.WriteLine($"{x.Date:dd-MM-yy HH:m:ss} {x.TemperatureC:N2} {x.Summary}");
            });
        }

        private static void PostData()
        {
            Console.WriteLine("\nPOST");
            var item = new WeatherForecast {
                Date = DateTime.Now,
                TemperatureC = 13,
                Summary = "Freezing in Bogotá!"
            };

            using (var httpClient = new HttpClient()) {
                httpClient.BaseAddress = new Uri(_apiRoot);
                var buffer = MessagePackSerializer.Serialize(item, ContractlessStandardResolver.Options);
                var byteContent = new ByteArrayContent(buffer);
                var result = httpClient.PostAsync("WeatherForecast", byteContent).Result;

                Console.WriteLine("Result: {0}", result);
            }
        }

    }

    // public record WeatherForecast(DateTime Date, int TemperatureC, string Summary);
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
using System;

namespace MessagePackApi
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        public override string ToString()
        {
            return $"{Date:dd-MM-yy HH:m:ss} {TemperatureC:N2} {Summary}";
        }
    }
}

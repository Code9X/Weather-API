using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApp.Models
{
    public class WeatherData
    {
        public string City { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
    }
}

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApp.Models
{
    public class WeatherData
    {
        public string City { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public string Icon { get; set; }
        public double FeelsLike { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int Visibility { get; set; }
        public double WindSpeed { get; set; }
    }
}

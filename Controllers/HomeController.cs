using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            List<string> cityNames = GetCityNames();
            ViewBag.CityList = cityNames;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if (!string.IsNullOrEmpty(city))
            {
                WeatherData weatherData = await GetWeatherData(city);
                List<string> cityNames = GetCityNames();
                ViewBag.CityList = cityNames;
                ViewBag.SelectedCity = city;
                return View(weatherData);
            }
            return View();
        }
        private List<string> GetCityNames()
        {
            return new List<string>
            {
                "New York",
                "Los Angeles",
                "Chicago",
                "Houston",
                "Tokyo",
                "New Delhi",
				"Kochi"
			};
        }
        private async Task<WeatherData> GetWeatherData(string city)
        {
            using (var client = new HttpClient())
            {
                string apiKey = _configuration["WeatherSettings:ApiKey"];
                string weatherApiUrl = _configuration["WeatherSettings:weatherApiUrl"];

                string url = string.Format(weatherApiUrl, city, apiKey);

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    // Extract the data from the JSON response
                    string cityName = data["name"];
                    string description = data["weather"][0]["description"];
                    double temperature = data["main"]["temp"];
                    double feelsLike = data["main"]["feels_like"];
                    double minTemperature = data["main"]["temp_min"];
                    double maxTemperature = data["main"]["temp_max"];
                    int pressure = data["main"]["pressure"];
                    int humidity = data["main"]["humidity"];
                    int visibility = data["visibility"];
                    double windSpeed = data["wind"]["speed"];
                    string icon = data["weather"][0]["icon"];

                    // Create a new WeatherData object with the extracted data
                    WeatherData weatherData = new WeatherData
                    {
                        City = cityName,
                        Description = description,
                        Temperature = temperature,
                        FeelsLike = feelsLike,
                        MinTemperature = minTemperature,
                        MaxTemperature = maxTemperature,
                        Pressure = pressure,
                        Humidity = humidity,
                        Visibility = visibility,
                        WindSpeed = windSpeed,
                        Icon = icon // Assign the icon to the property
                    };

                    return weatherData;
                }
                return null;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

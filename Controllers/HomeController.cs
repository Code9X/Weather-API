using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private List<string> cityNames;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<ActionResult> Index()
        {
            List<string> cityNames = GetCityNames();
            ViewBag.CityList = cityNames;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string city)
        {
            if (!string.IsNullOrEmpty(city))
            {
                WeatherData weatherData = await GetWeatherData(city);
                List<string> cityNames = GetCityNames(); // Get city names again
                ViewBag.CityList = cityNames; // pass the city list
                ViewBag.SelectedCity = city; // pass the selected city
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
                "New Delhi"
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

                    // Create a new WeatherData object with the extracted data
                    WeatherData weatherData = new WeatherData
                    {
                        City = cityName,
                        Description = description,
                        Temperature = temperature
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


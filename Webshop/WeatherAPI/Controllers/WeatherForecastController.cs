using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using WeatherAPI.Model;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeatherData(string city)
        {
            string APIKey = "";
            string APILocation = "";
            var responseFromAPI = await _httpClient.GetAsync(APILocation);
            if (!responseFromAPI.IsSuccessStatusCode) 
            {
                return StatusCode((int)responseFromAPI.StatusCode,"Hiba az idõjárás lekérése során!");
            }
            var json= await responseFromAPI.Content.ReadAsStringAsync();
            var weather=JsonConvert.DeserializeObject<Weather>(json);
            return Ok(weather);
        }
    }
}

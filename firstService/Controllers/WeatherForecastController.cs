using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace firstService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptionsMonitor<RemoteServiceSettings> _settings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsMonitor<RemoteServiceSettings> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            if (_settings.CurrentValue.UseRestSharp)
            {
                RestRequest request = new RestRequest("weatherforecast");
                var client = new RestClient(_settings.CurrentValue.ConnectionString);

                List<WeatherForecast> forecast = await client.GetAsync<List<WeatherForecast>>(request);

                return forecast;
            }
            else
            {
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient client = new HttpClient(handler);
                HttpResponseMessage response = await client.GetAsync(_settings.CurrentValue.ConnectionString + "/weatherforecast");

                response.EnsureSuccessStatusCode();
                List<WeatherForecast> forecast = JsonConvert.DeserializeObject<List<WeatherForecast>>(await response.Content.ReadAsStringAsync());

                return forecast;
            }
        }
    }
}

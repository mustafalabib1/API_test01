using Microsoft.AspNetCore.Mvc;
using test01.Services;

namespace test01.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherForecastService service;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
    {
        _logger = logger;
        this.service = service;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    { 
        return service.GetForecasts();
        
    }
}

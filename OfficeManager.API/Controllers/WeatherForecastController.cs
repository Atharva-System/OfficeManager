using Microsoft.AspNetCore.Mvc;
using OfficeManager.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace OfficeManager.API.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        [HttpGet]
        [Route("All")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await Mediator.Send(new GetWeatherForecastsQuery());
        }
    }
}
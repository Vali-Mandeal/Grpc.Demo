using Grpc.Demo.Client.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Grpc.Demo.Client.HttpEndpoints
{
    [Route("api")]
    [ApiController]
    public class WeatherEndpoint : ControllerBase
    {
        private readonly ILogger<WeatherEndpoint> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherEndpoint(ILogger<WeatherEndpoint> logger, IWeatherService wheatherService)
        {
            _logger = logger;
            _weatherService = wheatherService;
        }

        [SwaggerOperation(Summary = "Gets random generated weather values, for a random european capital city, using gRPC Unary Request.")]
        [HttpGet("Weather")]
        public async Task<IActionResult> GetRandomWeather()
        {
            var weatherResponseMessage = await _weatherService.GetRandomWeather();

            return Ok(weatherResponseMessage);
        }

        [SwaggerOperation(Summary = "Gets random generated weather values, for all european capital cities, using gRPC Server-Side Stream Request.")]
        [HttpGet("WeatherStream")]
        public async Task<IActionResult> GetRandomWeatherStream(CancellationToken cancellationToken)
        {
            IEnumerable<WeatherResponseMessage> weatherResponseMessages;
            const string userCancellationMessage = "User cancelled request. Gracefully stopping...";

            try
            {
                weatherResponseMessages = await _weatherService.GetRandomWeatherStream(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation($"Class: {nameof(WeatherService)}, Method: {nameof(GetRandomWeatherStream)} - ${userCancellationMessage}");
                return BadRequest(userCancellationMessage);
            }

            return Ok(weatherResponseMessages);
        }
    }
}

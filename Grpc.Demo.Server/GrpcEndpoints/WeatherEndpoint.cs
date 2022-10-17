using Grpc.Core;
using Grpc.Demo.Server.Services;

namespace Grpc.Demo.Server.GrpcEndpoints
{
    public class WeatherEndpoint : WheatherService.WheatherServiceBase
    {
        private readonly ILogger<WeatherEndpoint> _logger;
        private readonly IWeatherService _weatherService;
        public WeatherEndpoint(ILogger<WeatherEndpoint> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        public override async Task<WeatherResponseMessage> GetCurrentWeather(WeatherRequestMessage request, ServerCallContext context)
        {
            string loggingMessage = $"Class: {nameof(WeatherEndpoint)}, Method: {nameof(GetCurrentWeather)} - Received unary weather request for city: {request.City}.";
            _logger.LogInformation(loggingMessage);

            var randomWeather = await _weatherService.GetRandomWeather(request.City);

            return await Task.FromResult(randomWeather);
        }

        public override async Task GetCurrentWeatherStream(WeatherRequestMesagesWrapper weatherRequestMesagesWrapper, IServerStreamWriter<WeatherResponseMessage> responseStream, ServerCallContext context)
        {
            string loggingMessage = $"Class: {nameof(WeatherEndpoint)}, Method: {nameof(GetCurrentWeatherStream)} - Received server-side streaming request for all cities.";
            _logger.LogInformation(loggingMessage);

            foreach (var requestMessage in weatherRequestMesagesWrapper.Messages)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Request was cancelled.");
                    break;
                }

                var weather = await _weatherService.GetRandomWeather(requestMessage.City);

                await responseStream.WriteAsync(weather);

                await Task.Delay(1000);
            }
        }
    }
}

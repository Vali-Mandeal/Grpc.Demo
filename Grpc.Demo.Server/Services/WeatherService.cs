using Google.Protobuf.WellKnownTypes;

namespace Grpc.Demo.Server.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public async Task<WeatherResponseMessage> GetRandomWeather(string city)
        {
            var loggingMessage = $"Class: {nameof(WheatherService)}, Method: {nameof(GetRandomWeather)} - Processing weather for {city} city.";
            _logger.LogInformation(loggingMessage);

            var temperature = GetRandomNumber(0, 30);
            var feelsLike = GetRandomNumber(temperature - 1, temperature + 1);
            var timestamp = Timestamp.FromDateTime(DateTime.UtcNow);

            return await Task.FromResult(new WeatherResponseMessage
            {
                City = city,
                Temperature = temperature,
                FeelsLike = feelsLike,
                Timestamp = timestamp
            });
        }

        private static double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}

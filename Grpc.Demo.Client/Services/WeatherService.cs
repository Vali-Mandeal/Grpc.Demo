using Grpc.Core;

namespace Grpc.Demo.Client.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly WheatherService.WheatherServiceClient _wheatherServiceClient;
        public WeatherService(ILogger<WeatherService> logger, WheatherService.WheatherServiceClient wheatherServiceClient)
        {
            _logger = logger;
            _wheatherServiceClient = wheatherServiceClient;
        }

        public async Task<WeatherResponseMessage> GetRandomWeather()
        {
            WeatherRequestMessage weatherRequestMessage = GetWeatherRequestMessage();

            var loggingMessage = $"Class: {nameof(WeatherService)}, Method: {nameof(GetRandomWeather)} - Sending unary weather request to gRPC server for city: {weatherRequestMessage.City}.";
            _logger.LogInformation(loggingMessage);

            return await _wheatherServiceClient.GetCurrentWeatherAsync(weatherRequestMessage);
        }

        public async Task<IEnumerable<WeatherResponseMessage>> GetRandomWeatherStream(CancellationToken cancellationToken)
        {
            var loggingMessage = $"Class: {nameof(WeatherService)}, Method: {nameof(GetRandomWeatherStream)} - Sending server-side streaming request to gRPC server for all european capitals.";
            _logger.LogInformation(loggingMessage);

            var weatherRequestMesagesWrapper = GetWeatherRequestMessagesWrapper();

            var reply = _wheatherServiceClient.GetCurrentWeatherStream(weatherRequestMesagesWrapper, cancellationToken: cancellationToken);

            List<WeatherResponseMessage> weatherResponseMessages = await GetParsedWeatherResponseMessages(reply, cancellationToken);

            return weatherResponseMessages;
        }

        private async Task<List<WeatherResponseMessage>> GetParsedWeatherResponseMessages(AsyncServerStreamingCall<WeatherResponseMessage> reply, CancellationToken cancellationToken)
        {
            List<WeatherResponseMessage> weatherResponseMessages = new();

            while (await reply.ResponseStream.MoveNext(cancellationToken))
            {
                var currentResult = reply.ResponseStream.Current;

                string resultLoggingMessage = $"Class: {nameof(WeatherService)}, Method: {nameof(GetRandomWeatherStream)} - Weather for city {currentResult.City}: temperature {currentResult.Temperature}, feels like {currentResult.FeelsLike}, sent on {currentResult.Timestamp}";
                _logger.LogInformation(resultLoggingMessage);

                weatherResponseMessages.Add(currentResult);

                await Task.Delay(1000, cancellationToken);
            }

            return weatherResponseMessages;
        }

        private static WeatherRequestMesagesWrapper GetWeatherRequestMessagesWrapper()
        {
            WeatherRequestMesagesWrapper weatherRequestMesagesWrapper = new();

            foreach (var europeanCapitalCity in Enum.GetNames(typeof(EuropeanCapitalCities)))
            {
                WeatherRequestMessage weatherRequestMessage = new()
                {
                    City = europeanCapitalCity
                };

                weatherRequestMesagesWrapper.Messages.Add(weatherRequestMessage);
            }

            return weatherRequestMesagesWrapper;
        }

        private static WeatherRequestMessage GetWeatherRequestMessage()
        {
            var europeanCapitalCity = GetRandomEuropeanCapitalCity();

            return new() { City = europeanCapitalCity };
        }

        private static string GetRandomEuropeanCapitalCity()
        {
            var startIndex = Enum.GetValues(typeof(EuropeanCapitalCities)).Cast<int>().Min();
            var endIndex = Enum.GetValues(typeof(EuropeanCapitalCities)).Cast<int>().Max();

            var randomNumber = GetRandomNumber(startIndex, endIndex);

            var europeanCapitalCity = (EuropeanCapitalCities)randomNumber;

            return europeanCapitalCity.ToString();
        }

        private static int GetRandomNumber(int minimum, int maximum)
        {
            Random random = new();
            return random.Next(minimum, maximum);
        }
    }
}

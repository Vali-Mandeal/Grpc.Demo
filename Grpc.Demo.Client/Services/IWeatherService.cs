namespace Grpc.Demo.Client.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponseMessage> GetRandomWeather();
        Task<IEnumerable<WeatherResponseMessage>> GetRandomWeatherStream(CancellationToken cancellationToken);
    }
}

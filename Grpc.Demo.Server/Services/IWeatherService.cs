namespace Grpc.Demo.Server.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponseMessage> GetRandomWeather(string city);
    }
}

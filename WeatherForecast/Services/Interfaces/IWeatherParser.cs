using WeatherForecast.Models;

namespace WeatherForecast.Services.Interfaces;

public interface IWeatherParser
{
    ReturnStatus<List<WeatherData>, FileLoadingStatus> ParseWeather(Stream stream);
}
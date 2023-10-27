using WeatherForecast.Database.Models;

namespace WeatherForecast.ViewModels;

public class FilteredData
{
    public int Count { get; set; }
    public List<Data> Data { get; set; }
}
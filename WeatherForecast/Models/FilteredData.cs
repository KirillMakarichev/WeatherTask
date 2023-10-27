using WeatherForecast.Database.Models;

namespace WeatherForecast.Models;

public class FilteredData
{
    public int Count { get; set; }
    public List<Data> Data { get; set; }
}
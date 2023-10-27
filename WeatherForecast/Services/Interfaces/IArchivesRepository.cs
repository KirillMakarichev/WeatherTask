using WeatherForecast.Database.Models;
using WeatherForecast.Models;
using WeatherForecast.ViewModels;

namespace WeatherForecast.Services.Interfaces;

public interface IArchivesRepository
{
    Task<List<int>> GetAllYearsAsync();
    Task<FilteredData> GetAsync(int? year = null, int? month = null, int take = 10, int skip = 0);
    Task<bool> SaveAsync(List<Data> data);
}
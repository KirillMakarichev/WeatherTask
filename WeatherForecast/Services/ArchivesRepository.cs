using Microsoft.EntityFrameworkCore;
using WeatherForecast.Database;
using WeatherForecast.Database.Models;
using WeatherForecast.Models;
using WeatherForecast.Services.Interfaces;
using WeatherForecast.ViewModels;

namespace WeatherForecast.Services;

public class ArchivesRepository : IArchivesRepository
{
    private readonly ArchivesContext _context;

    public ArchivesRepository(ArchivesContext context)
    {
        _context = context;
    }

    public async Task<List<int>> GetAllYearsAsync()
    {
        var query = _context.WeatherData
            .GroupBy(x => x.Date.Year)
            .Select(x => x.Key)
            .OrderBy(x => x);

        Console.WriteLine(query.ToQueryString());

        var f = await query
            .ToListAsync();

        return f;
    }

    public async Task<FilteredData> GetAsync(int? year = null, int? month = null, int take = 10, int skip = 0)
    {
        var query = _context.WeatherData.AsQueryable();

        if (year != null)
            query = query.Where(x => x.Date.Date.Year == year);
        if (month != null)
            query = query.Where(x => x.Date.Date.Month == month);

        var count = await query.CountAsync();

        if (skip > 0)
            query = query.Skip(skip);
        if (take != 0)
            query = query.Take(take);

        var data = await query.ToListAsync();

        return new FilteredData()
        {
            Count = count,
            Data = data
        };
    }

    public async Task<bool> SaveAsync(List<Data> data)
    {
        try
        {
            var uniqueData = data.DistinctBy(x => x.Date).ToDictionary(x => x.Date, x => x);

            await _context.WeatherData.AsNoTracking().Where(x => uniqueData.Keys.Contains(x.Date)).ExecuteDeleteAsync();

            _context.WeatherData.AddRange(uniqueData.Values);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}
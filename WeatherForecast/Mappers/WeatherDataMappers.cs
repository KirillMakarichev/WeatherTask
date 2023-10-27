using WeatherForecast.Database.Models;
using WeatherForecast.Models;

namespace WeatherForecast.Mappers;

public static class WeatherDataMappers
{
    public static WeatherData ModelFromDbData(Data data) => new WeatherData()
    {
        Cloudiness = data.Cloudiness,
        Date = data.Date,
        Temperature = data.Temperature,
        Time = data.Date.TimeOfDay,
        Visibility = data.Visibility,
        AtmosphericPressure = data.AtmosphericPressure,
        CloudinessPercentage = data.CloudinessPercentage,
        DewPoint = data.DewPoint,
        RelativeHumidity = data.RelativeHumidity,
        WeatherPhenomenon = data.WeatherPhenomenon,
        WindDirection = data.WindDirection,
        WindSpeed = data.WindSpeed
    };
    
    public static Data? DbDataFromModel(WeatherData weatherData)
    {
        var date = GetUtcDateTime(weatherData.Date, weatherData.Time);

        if (date == null) return null;
        
        return new Data()
        {
            Cloudiness = weatherData.Cloudiness,
            Date = date.Value.DateTime,
            Temperature = weatherData.Temperature,
            Visibility = weatherData.Visibility,
            AtmosphericPressure = weatherData.AtmosphericPressure,
            CloudinessPercentage = weatherData.CloudinessPercentage,
            DewPoint = weatherData.DewPoint,
            RelativeHumidity = weatherData.RelativeHumidity,
            WeatherPhenomenon = weatherData.WeatherPhenomenon,
            WindDirection = weatherData.WindDirection,
            WindSpeed = weatherData.WindSpeed
        };
    }

    private static DateTimeOffset? GetUtcDateTime(DateTime? date, TimeSpan? time)
    {
        if (!date.HasValue || !time.HasValue) return null;
        var dateTimeOffset = new DateTimeOffset(date.Value.Year, 
            date.Value.Month, date.Value.Day, 
            time.Value.Hours, time.Value.Minutes, 
            time.Value.Seconds, time.Value.Milliseconds, 
            TimeSpan.FromHours(0));
        
        var utcDateTime = dateTimeOffset.ToUniversalTime();
        
        return utcDateTime;
    }
}
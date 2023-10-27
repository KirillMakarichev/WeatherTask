using System.Diagnostics;

namespace WeatherForecast.Models;

[DebuggerDisplay("Count = {Date} {Time}")]
public class WeatherData
{
    [XLSXColumnIndex(0)]
    public DateTime? Date { get; set; }
    
    [XLSXColumnIndex(1)]
    public TimeSpan? Time { get; set; }
    
    [XLSXColumnIndex(2)]
    public double? Temperature { get; set; } // Температура воздуха, градусы Цельсия
    
    [XLSXColumnIndex(3)]
    public double? RelativeHumidity { get; set; } // Относительная влажность воздуха, %

    [XLSXColumnIndex(4)]
    public double? DewPoint { get; set; } // Точка росы, градусы Цельсия
    
    [XLSXColumnIndex(5)]
    public double? AtmosphericPressure { get; set; } // Атмосферное давление, мм рт. ст.
    
    [XLSXColumnIndex(6)]
    public string? WindDirection { get; set; }
    
    [XLSXColumnIndex(7)]
    public double? WindSpeed { get; set; } // Скорость ветра, м/с
    
    [XLSXColumnIndex(8)]
    public double? CloudinessPercentage { get; set; } // Облачность, %
    
    [XLSXColumnIndex(9)]
    public double? Cloudiness { get; set; } // Облачность, м
    
    [XLSXColumnIndex(10)]
    public double? Visibility { get; set; } // Горизонтальная видимость, км
    
    [XLSXColumnIndex(11)]
    public string? WeatherPhenomenon { get; set; } // Погодные явления
}
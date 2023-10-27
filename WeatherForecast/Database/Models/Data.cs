using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Database.Models;

public class Data
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public double? Temperature { get; set; } // Температура воздуха, градусы Цельсия
    public double? RelativeHumidity { get; set; } // Относительная влажность воздуха, %
    public double? DewPoint { get; set; } // Точка росы, градусы Цельсия
    public double? AtmosphericPressure { get; set; } // Атмосферное давление, мм рт. ст.
    public string? WindDirection { get; set; }
    public double? WindSpeed { get; set; } // Скорость ветра, м/с
    public double? CloudinessPercentage { get; set; } // Облачность, %
    public double? Cloudiness { get; set; } // Облачность, м
    public double? Visibility { get; set; } // Горизонтальная видимость, км
    public string? WeatherPhenomenon { get; set; } // Погодные явления
}
namespace WeatherForecast.Models;

public class FilesLoadingResponse
{
    public List<(string name, FileLoadingStatus status)> Statuses { get; set; }
    public bool Saved { get; set; }
}
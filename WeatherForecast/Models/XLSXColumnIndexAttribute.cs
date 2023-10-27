namespace WeatherForecast.Models;

public class XLSXColumnIndexAttribute : Attribute
{
    public int ColumnIndex { get; set; }

    public XLSXColumnIndexAttribute(int columnIndex)
    {
        ColumnIndex = columnIndex;
    }
}
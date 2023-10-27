using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.Util;
using WeatherForecast.Helpers;
using WeatherForecast.Models;
using WeatherForecast.Services.Interfaces;

namespace WeatherForecast.Services;

public class XlsxParser : IWeatherParser
{
    public ReturnStatus<List<WeatherData>, FileLoadingStatus> ParseWeather(Stream stream)
    {
        try
        {
            using var fileInputStream = new FileInputStream(stream);
            using var book = WorkbookFactory.Create(fileInputStream);

            if (book == null) return ReturnStatus<List<WeatherData>, FileLoadingStatus>.Create(new List<WeatherData>(), 
                FileLoadingStatus.WrongExcelFormat);

            if (book.NumberOfSheets == 0) return ReturnStatus<List<WeatherData>, FileLoadingStatus>.Create(new List<WeatherData>(), 
                FileLoadingStatus.WrongExcelFormat);

            var weatherDataType = typeof(WeatherData);
            var properties = ReflectionDataHelper.GetPropertiesDefinition(weatherDataType);

            var data = new List<WeatherData>();
            var firstRowIndex = 0;
            const string tryParseName = nameof(IParsable<int>.TryParse);

            for (var i = 0; i < book.NumberOfSheets; i++)
            {
                var sheet = book.GetSheetAt(i);

                for (var rowIndex = firstRowIndex; rowIndex < sheet.LastRowNum; rowIndex++)
                {
                    var row = sheet.GetRow(rowIndex);

                    if (row == null) continue;

                    var rowWeatherData = ParseRow(row, properties, tryParseName);
                    
                    if (rowWeatherData.Date == default) continue;

                    data.Add(rowWeatherData);
                }
            }

            if(!data.Any()) return ReturnStatus<List<WeatherData>, FileLoadingStatus>.Create(new List<WeatherData>(), 
                FileLoadingStatus.WrongExcelFormat);
            
            return ReturnStatus<List<WeatherData>, FileLoadingStatus>.Create(data, 
                FileLoadingStatus.Ok);
        }
        catch (Exception)
        {
            return ReturnStatus<List<WeatherData>, FileLoadingStatus>.Create(new List<WeatherData>(), 
                FileLoadingStatus.NonExcel);
        }
    }

    private WeatherData ParseRow(IRow row, List<ReflectionData> properties, string invokedMethodName)
    {
        var weatherData = new WeatherData();

        foreach (var property in properties)
        {
            var cell = row.GetCell(property.Index);
            if (cell == null) continue;

            var propertyInfo = property.PropertyInfo;
            var propertyType = property.PropertyType;
            if (propertyType.Name == nameof(String))
            {
                propertyInfo.SetValue(weatherData, cell.ToString());
                continue;
            }

            if (!property.IsParsable) continue;

            var methods = propertyType.GetMethods(BindingFlags.Static | BindingFlags.Public);
            var parseMethod = methods.First(x => x.Name == invokedMethodName);
            var parameters = new object[] { cell.ToString(), null };
            var result = (bool)parseMethod.Invoke(null, parameters);
            if (!result) continue;

            var parsedValue = parameters[1]; // Извлекаем значение из out параметра
            propertyInfo.SetValue(weatherData, parsedValue);
        }

        return weatherData;
    }
}
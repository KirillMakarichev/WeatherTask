using System.Reflection;
using WeatherForecast.Models;

namespace WeatherForecast.Helpers;

public static class ReflectionDataHelper
{
    public static List<ReflectionData> GetPropertiesDefinition(Type type)
    {
        var properties = new List<ReflectionData>();
        var propertiesInfo = type.GetProperties();
        var iParsableType = typeof(IParsable<>);
        foreach (var propertyInfo in propertiesInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            
            var indexAttribute = propertyInfo.GetCustomAttribute<XLSXColumnIndexAttribute>();

            if (indexAttribute == null) continue;
            
            var isNullable = false;
            var mainType = propertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                mainType = propertyType.GenericTypeArguments[0];
                isNullable = true;
            }

            var isParsable = mainType.GetInterface(iParsableType.Name) != null;

            properties.Add(new ReflectionData()
            {
                Index = indexAttribute.ColumnIndex,
                PropertyInfo = propertyInfo,
                IsParsable = isParsable,
                PropName = propertyInfo.Name,
                IsNullable = isNullable,
                PropertyType = isNullable ? propertyType.GenericTypeArguments[0] : mainType
            });
        }

        return properties;
    }
}
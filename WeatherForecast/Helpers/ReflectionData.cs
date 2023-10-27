using System.Reflection;

namespace WeatherForecast.Helpers;

public struct ReflectionData
{
    public int Index { get; init; }
    public string PropName { get; init; }
    public PropertyInfo PropertyInfo { get; init; }
    public bool IsParsable { get; init; }
    public bool IsNullable { get; init; }
    public Type PropertyType { get; init; }
}
using System.Globalization;

namespace Learning.App.Converters;

/// <summary>
/// Converter to convert an object to a boolean based on whether the object is null.
/// </summary>
public class NullOrEmptyToBoolConverter : IValueConverter
{
    /// <summary>
    /// Convert an object to a boolean based on whether the object is null.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter">Expects a boolean value. If the value is true then the result is inverted.</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var invertResultParam = System.Convert.ToBoolean(parameter);
        var result = value is null ? false : true;
        if (value is string val)
        {
            result = string.IsNullOrEmpty(val) ? false : true;
        }

        return invertResultParam ? !result : result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
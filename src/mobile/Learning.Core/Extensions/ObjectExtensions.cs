namespace Learning.Core.Extensions;

/// <summary>
/// Extension method for objects.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Checks if the <paramref name="obj"/> is an instance of type <paramref name="genericType"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="genericType"></param>
    /// <returns></returns>
    public static bool IsInstanceOfGenericType(this object obj, Type genericType)
    {
        Type objType = obj.GetType();
        while (objType != null && objType != typeof(object))
        {
            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
            objType = objType.BaseType;
        }
        return false;
    }

    public static bool IsInstanceOfGenericType(this IEnumerable<object> collection, Type genericType)
    {
        return collection.OfType<object>().Any(item => item.GetType() == genericType);
    }
}

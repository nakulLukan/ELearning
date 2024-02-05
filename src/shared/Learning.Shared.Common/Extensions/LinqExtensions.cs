using System.Linq.Expressions;

namespace Learning.Shared.Common.Extensions;

public static class LinqExpressions
{
    public static IQueryable<T> SortyBy<T>(this IQueryable<T> query, string propertyName, bool isDescending)
    {
        Type entityType = typeof(T);
        ParameterExpression parameterExpression = Expression.Parameter(entityType, "x");
        MemberExpression propertyExpression = Expression.Property(parameterExpression, propertyName);
        LambdaExpression lambdaExpression = Expression.Lambda(propertyExpression, parameterExpression);
        MethodCallExpression methodCallExpression = isDescending
            ? Expression.Call(typeof(Queryable), "OrderByDescending", new[] { entityType, propertyExpression.Type }, query.Expression, lambdaExpression)
            : Expression.Call(typeof(Queryable), "OrderBy", new[] { entityType, propertyExpression.Type }, query.Expression, lambdaExpression);
        return query.Provider.CreateQuery<T>(methodCallExpression);
    }

    public static IQueryable<TSource> SortyBy<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, bool isDescending)
    {
        return isDescending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }
}

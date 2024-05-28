using QueryR.Extensions;
using QueryR.QueryModels;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace QueryR.QueryActions
{
    internal class SortQueryAction : IQueryAction
    {
        public QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult)
        {
            var type = typeof(T);
            bool isFirst = true;

            foreach (var sort in query.Sorts ?? Enumerable.Empty<Sort>())
            {
                var parameter = Expression.Parameter(type, "t");

                Expression memberExpression = parameter;
                foreach (var propertyName in sort.PropertyName.Split(new[] { "." }, StringSplitOptions.None))
                {
                    memberExpression = Expression.Property(memberExpression, propertyName);
                }

                var lambda = Expression.Lambda(memberExpression, parameter);
                var methodName = GetOrderMethodName(isFirst, sort.IsAscending);
                var method = typeof(Queryable).GetGenericMethod(methodName, 2, 2, type, memberExpression.Type);
                queryResult.PagedQuery = (IQueryable<T>)method.Invoke(null, new object[] { queryResult.PagedQuery, lambda });

                isFirst = false;
            }

            return queryResult;
        }
        private static string GetOrderMethodName(bool isFirst, bool isAscending) => isFirst ?
                isAscending ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending) :
                isAscending ? nameof(Queryable.ThenBy) : nameof(Queryable.ThenByDescending);
    }
}

using QueryR.Extensions;
using QueryR.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryR.QueryActions
{
    internal class FilterQueryAction : IQueryAction
    {
        public QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult)
        {
            if (queryResult == null || queryResult.PagedQuery == null)
            {
                throw new ArgumentNullException(nameof(queryResult));
            }

            foreach (var filter in query.Filters ?? Enumerable.Empty<Filter>())
            {
                var parameter = Expression.Parameter(typeof(T), "t");
                Expression memberExpression = parameter;

                var propertyNames = new Queue<string>(filter.PropertyName.Split(new[] { "." }, StringSplitOptions.None));
                var result = GetMemberExpression(typeof(T), memberExpression, propertyNames, filter, parameter);
                var lambda = Expression.Lambda<Func<T, bool>>(result, parameter);
                queryResult.PagedQuery = queryResult.PagedQuery.Where(lambda);
                queryResult.CountQuery = queryResult.CountQuery.Where(lambda);
            }

            return queryResult;
        }

        /// <summary>
        /// Recursive method to build Property chain with .Any() on collections.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="previousExpression"></param>
        /// <param name="propertyNames"></param>
        /// <param name="filter"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private Expression GetMemberExpression(
            Type type,
            Expression previousExpression,
            Queue<string> propertyNames,
            Filter filter,
            ParameterExpression parameter)
        {
            var propertyName = propertyNames.Dequeue();

            var enumerableType = TypeExtensions.FindElementType(previousExpression.Type);

            // If at end, return Property Expression.
            if (!propertyNames.Any())
            {
                Expression endExpression = null;
                //if collection, use any
                if (enumerableType != null)
                {
                    var childItem = Expression.Parameter(enumerableType, "item");
                    var childItemProperty = Expression.Property(childItem, propertyName);
                    var target = Expression.Constant(filter.Value.Convert(childItemProperty.Type), childItemProperty.Type);
                    var operatorExpression = filter.Operator.ExpressionMethod(childItemProperty, target);

                    var anyLambda = Expression.Lambda(operatorExpression, childItem);

                    var anyCall = Expression.Call(
                            typeof(Enumerable).GetGenericMethod(nameof(Enumerable.Any), 1, 2, enumerableType),
                            previousExpression,
                            anyLambda);

                    endExpression = anyCall;
                }
                else
                {
                    var memberExpression = Expression.Property(previousExpression, propertyName);
                    var target = Expression.Constant(filter.Value.Convert(memberExpression.Type), memberExpression.Type);
                    endExpression = filter.Operator.ExpressionMethod(memberExpression, target);
                }

                return endExpression;
            }

            // If collection, recurse on children.
            if (enumerableType != null)
            {
                var childItem = Expression.Parameter(enumerableType, "item");
                var childItemProperty = Expression.Property(childItem, propertyName);

                var childItemPropertySubExpression = GetMemberExpression(
                    enumerableType,
                    childItemProperty,
                    propertyNames,
                    filter,
                    childItem);

                var anyLambda = Expression.Lambda(childItemPropertySubExpression, childItem);

                var anyCall = Expression.Call(
                        typeof(Enumerable).GetGenericMethod(nameof(Enumerable.Any), 1, 2, enumerableType),
                        previousExpression,
                        anyLambda);

                return anyCall;
            }
            // If not, recurse on property.
            else
            {
                var memberExpression = Expression.Property(previousExpression, propertyName);
                var nullCheck = Expression.Equal(memberExpression, Expression.Constant(null));
                return Expression.Condition(nullCheck, Expression.Constant(false), GetMemberExpression(type, memberExpression, propertyNames, filter, parameter));
            }
        }
    }
}

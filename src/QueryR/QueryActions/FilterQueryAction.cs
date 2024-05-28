using QueryR.Extensions;
using QueryR.QueryModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryR.QueryActions
{
    internal class FilterQueryAction : IQueryAction
    {
        public QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult)
        {
            ValidateQueryResult(queryResult);

            foreach (var filter in query?.Filters ?? Enumerable.Empty<Filter>())
            {
                var parameter = Expression.Parameter(typeof(T), "t");
                var propertyNames = new Queue<string>(filter.PropertyName.Split('.'));
                var filterExpression = FilterExpression(typeof(T), parameter, propertyNames, filter);

                var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                queryResult.PagedQuery = queryResult.PagedQuery.Where(lambda);
                queryResult.CountQuery = queryResult.CountQuery.Where(lambda);
            }

            return queryResult;
        }

        private void ValidateQueryResult<T>(QueryResult<T> queryResult)
        {
            if (queryResult == null)
            {
                throw new ArgumentNullException(nameof(queryResult), "QueryResult cannot be null.");
            }

            if (queryResult.PagedQuery == null)
            {
                throw new ArgumentNullException($"{nameof(queryResult)}.{nameof(QueryResult<T>.PagedQuery)}", "PagedQuery within QueryResult cannot be null.");
            }

            if (queryResult.CountQuery == null)
            {
                throw new ArgumentNullException($"{nameof(queryResult)}.{nameof(QueryResult<T>.CountQuery)}", "CountQuery within QueryResult cannot be null.");
            }
        }

        /// <summary>
        /// Recursive method to build Property chain with .Any() on collections.
        /// </summary>
        private Expression FilterExpression(
            Type type,
            Expression parentExpression,
            Queue<string> propertyNames,
            Filter filter)
        {
            var propertyName = propertyNames.Dequeue();
            var enumerableType = TypeExtensions.FindElementType(parentExpression.Type);

            var isStillNavigating = propertyNames.Any();
            var isObject = enumerableType == null;

            return (isStillNavigating, isObject).TruthTable(
                //end of navigation, is collection
                () => CollectionFilter(enumerableType, parentExpression, propertyName, filter),
                //end of navigation, is object
                () => ObjectFilter(enumerableType, parentExpression, propertyName, filter),
                //still navigating, is collection
                () => ChainedCollectionExpression(enumerableType, parentExpression, propertyName, propertyNames, filter),
                //still navigating, is object
                () => ChainedObjectExpression(parentExpression, propertyName, propertyNames, filter)
            )();
        }

        /// <summary>
        /// eg. t.Items.Any(item => [ObjectFilter])
        /// </summary>
        private Expression CollectionFilter(Type elementType, Expression parentExpression, string propertyName, Filter filter)
        {
            var itemParameter = Expression.Parameter(elementType, "item");
            var filterExpression = ObjectFilter(elementType, itemParameter, propertyName, filter);
            return Any(elementType, parentExpression, itemParameter, filterExpression);
        }

        /// <summary>
        /// If collection, t.ListOfNames.Contains("MyName")
        /// If object, t.Name == "MyItem"
        /// </summary>
        private Expression ObjectFilter(Type elementType, Expression parentExpression, string propertyName, Filter filter)
        {
            var memberExpression = Expression.Property(parentExpression, propertyName);
            //eg. t.ListOfNames.Contains("MyName")
            if (typeof(IEnumerable).IsAssignableFrom(memberExpression.Type)
                && memberExpression.Type != typeof(string))
            {
                var type = TypeExtensions.FindElementType(memberExpression.Type);
                var target = Expression.Constant(filter.Value.Convert(type), type);

                try
                {
                    return filter.Operator.ExpressionMethod(memberExpression, target);
                }
                //If we get an expression we can't handle, silently do not filter.
                //TODO: In future perhaps we should configure silent fail and/or a "CanExecute" on FilterOperator class.
                catch
                {
                    return Expression.Constant(true);
                }


            }
            //eg. t.Name == "MyItem"
            else
            {
                var target = Expression.Constant(filter.Value.Convert(memberExpression.Type), memberExpression.Type);
                try
                {
                    return filter.Operator.ExpressionMethod(memberExpression, target);
                }

                //If we get an expression we can't handle, silently do not filter.
                //TODO: In future perhaps we should configure silent fail and/or a "CanExecute" on FilterOperator class.
                catch
                {
                    return Expression.Constant(true);
                }
            }
        }

        /// <summary>
        /// eg. t.Items.Any(item => ...
        /// </summary>
        private Expression ChainedCollectionExpression(
            Type elementType,
            Expression parentExpression,
            string propertyName,
            Queue<string> propertyNames,
            Filter filter)
        {
            var itemParameter = Expression.Parameter(elementType, "item");
            var itemProperty = Expression.Property(itemParameter, propertyName);
            var subExpression = FilterExpression(elementType, itemProperty, propertyNames, filter);

            return Any(elementType, parentExpression, itemParameter, subExpression);
        }

        /// <summary>
        /// eg. IIF(t.Items == null, False, ...
        /// </summary>
        private Expression ChainedObjectExpression(
            Expression parentExpression,
            string propertyName,
            Queue<string> propertyNames,
            Filter filter)
        {
            var memberExpression = Expression.Property(parentExpression, propertyName);
            var nullCheck = Expression.Equal(memberExpression, Expression.Constant(null));
            var nestedExpression = FilterExpression(memberExpression.Type, memberExpression, propertyNames, filter);

            return Expression.Condition(nullCheck, Expression.Constant(false), nestedExpression);
        }

        private Expression Any(
            Type elementType,
            Expression parentExpression,
            ParameterExpression itemParameter,
            Expression subExpression)
        {
            return Expression.Call(
                typeof(Enumerable).GetGenericMethod(nameof(Enumerable.Any), 1, 2, elementType),
                parentExpression,
                Expression.Lambda(subExpression, itemParameter));
        }
    }
}

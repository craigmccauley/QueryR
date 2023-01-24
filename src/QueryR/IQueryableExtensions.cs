using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Services;
using System.Collections.Generic;
using System.Linq;

namespace QueryR
{
    public static class IQueryableExtensions
    {
        private static List<IQueryAction> QueryActions { get; set; }

        private static void InitializeQueryActions()
        {
            if (QueryActions == null)
            {
                QueryActions = new List<IQueryAction>()
                {
                    new SparseFieldsQueryAction(new NullMaxDepthService()),
                    new FilterQueryAction(),
                    new SortQueryAction(),
                    new PagingQueryAction(),
                };
            }
        }

        /// <summary>
        /// Performs the <see cref="QueryModels.Query"/> on the source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryResult<T> Query<T>(this IQueryable<T> source, Query query)
        {
            var result = new QueryResult<T>
            {
                CountQuery = source,
                PagedQuery = source
            };

            InitializeQueryActions();

            foreach (var action in QueryActions)
            {
                action.Execute(query, result);
            }

            return result;
        }

        /// <summary>
        /// Convienience method, adds all the query parts to a <see cref="QueryModels.Query"/> and executes the <see cref="Query{T}(IQueryable{T}, Query)" /> extension method. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="firstQueryPart"></param>
        /// <param name="queryParts"></param>
        /// <returns></returns>
        public static QueryResult<T> Query<T>(this IQueryable<T> source, IQueryPart firstQueryPart, params IQueryPart[] queryParts)
        {
            return source.Query(new[] { firstQueryPart }.Concat(queryParts));
        }

        /// <summary>
        /// Convienience method, adds all the query parts to a <see cref="QueryModels.Query"/> and executes the <see cref="Query{T}(IQueryable{T}, Query)" /> extension method. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="firstQueryPart"></param>
        /// <param name="queryParts"></param>
        /// <returns></returns>
        public static QueryResult<T> Query<T>(this IQueryable<T> source, IEnumerable<IQueryPart> filters)
        {
            var query = new Query
            {
                Filters = new List<Filter>(),
                Sorts = new List<Sort>(),
                SparseFields = new List<SparseField>()
            };

            foreach (var item in filters)
            {
                if (item is Filter filter)
                {
                    query.Filters.Add(filter);
                }
                else if (item is PagingOptions pagingOptions)
                {
                    query.PagingOptions = pagingOptions;
                }
                else if (item is Sort sort)
                {
                    query.Sorts.Add(sort);
                }
                else if (item is SparseField sparseField)
                {
                    query.SparseFields.Add(sparseField);
                }
            }

            return source.Query(query);
        }
    }
}

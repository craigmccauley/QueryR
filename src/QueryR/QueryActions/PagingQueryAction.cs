using QueryR.QueryModels;
using System.Linq;

namespace QueryR.QueryActions
{
    internal class PagingQueryAction : IQueryAction
    {
        public QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult)
        {
            if (query.PagingOptions != null)
            {
                //one-based indexing for pages
                //TODO: Configuration for 0-based indexing?
                var skipCount = (query.PagingOptions.PageNumber - 1) * query.PagingOptions.PageSize;
                queryResult.PagedQuery = queryResult.PagedQuery
                    .Skip(skipCount)
                    .Take(query.PagingOptions.PageSize);
            }

            return queryResult;
        }
    }
}

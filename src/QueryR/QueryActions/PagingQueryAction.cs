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
                queryResult.PagedQuery = queryResult.PagedQuery
                    .Skip((query.PagingOptions.PageNumber - 1) * query.PagingOptions.PageSize)
                    .Take(query.PagingOptions.PageSize);
            }

            return queryResult;
        }
    }
}

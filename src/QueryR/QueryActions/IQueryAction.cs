using QueryR.QueryModels;

namespace QueryR.QueryActions
{
    internal interface IQueryAction
    {
        QueryResult<T> Execute<T>(Query query, QueryResult<T> queryResult);
    }
}

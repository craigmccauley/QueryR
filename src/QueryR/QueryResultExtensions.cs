using QueryR;
using QueryR.QueryModels;
using System.Collections.Generic;
using System.Linq;

namespace QueryR
{
    public static class QueryResultExtensions
    {
        public static int Count<T>(this QueryResult<T> queries) => queries.CountQuery.Count();
        public static List<T> ToList<T>(this QueryResult<T> queries) => queries.PagedQuery.ToList();
        public static (int Count, List<T> Items) GetCountAndList<T>(this QueryResult<T> queries) => (queries.Count(), queries.ToList());
    }
}

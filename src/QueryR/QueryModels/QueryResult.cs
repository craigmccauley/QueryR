using System.Linq;

namespace QueryR.QueryModels
{
    /// <summary>
    /// Call <see cref="QueryResultExtensions.GetCountAndList{T}(QueryResult{T})"/> to get the count and requested records.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T>
    {
        /// <summary>
        /// Call <see cref="QueryResultExtensions.Count{T}(QueryResult{T})"/> to get the number of total records matching the filters.
        /// </summary>
        public IQueryable<T> CountQuery { get; set; }
        /// <summary>
        /// Call <see cref="QueryResultExtensions.ToList{T}(QueryResult{T})"/> to get the requested records.
        /// </summary>
        public IQueryable<T> PagedQuery { get; set; }
    }
}

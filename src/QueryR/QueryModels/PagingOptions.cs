namespace QueryR.QueryModels
{
    public class PagingOptions : IQueryPart
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

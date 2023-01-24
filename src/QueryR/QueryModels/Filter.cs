namespace QueryR.QueryModels
{
    public class Filter: IQueryPart
    {
        public string PropertyName { get; set; }
        public FilterOperator Operator { get; set; }
        public string Value { get; set; }
    }
}

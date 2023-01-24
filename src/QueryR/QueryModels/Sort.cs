namespace QueryR.QueryModels
{
    public class Sort : IQueryPart
    {
        public bool IsAscending { get; set; }
        public string PropertyName { get; set; }
    }
}

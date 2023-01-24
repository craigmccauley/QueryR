using System.Collections.Generic;

namespace QueryR.QueryModels
{
    public class SparseField : IQueryPart
    {
        public string EntityName { get; set; }
        public List<string> PropertyNames { get; set; } = new List<string>();
    }
}

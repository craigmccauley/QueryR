using System.Collections.Generic;

namespace QueryR.QueryModels
{
    public class Query
    {
        public List<Filter> Filters { get; set; } = new List<Filter>();
        public PagingOptions PagingOptions { get; set; }
        public List<Sort> Sorts { get; set; } = new List<Sort>();
        public List<SparseField> SparseFields { get; set; } = new List<SparseField>(); 
    }
}

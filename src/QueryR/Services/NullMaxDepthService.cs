using QueryR.QueryModels;

namespace QueryR.Services
{
    internal class NullMaxDepthService : IMaxDepthService
    {
        public int? GetMaxDepth(Query query) => null;
    }
}

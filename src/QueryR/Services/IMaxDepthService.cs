using QueryR.QueryModels;

namespace QueryR.Services
{
    internal interface IMaxDepthService
    {
        int? GetMaxDepth(Query query);
    }
}

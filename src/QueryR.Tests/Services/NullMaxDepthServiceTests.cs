using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Services;
using QueryR.Tests.TestHelpers;
using Xunit;

namespace QueryR.Tests.Services;
public class NullMaxDepthServiceTests
{
    [Theory, AutoSubData]
    internal void GetMaxDepth_ReturnsNull(
        Query query,
        NullMaxDepthService sut)
    {
        // Act
        var result = sut.GetMaxDepth(query);

        // Assert
        result.Should().BeNull();
    }
}

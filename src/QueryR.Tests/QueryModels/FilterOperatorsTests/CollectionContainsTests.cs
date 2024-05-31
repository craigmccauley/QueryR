using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class CollectionContainsTests
{
    [Theory, AutoSubData]
    public void CollectionContains_ShouldWorkOnList(
        List<List<int>> values)
    {
        //arrange
        var valueToFind = values.PickRandom().PickRandom();

        var parameter = Expression.Parameter(typeof(List<int>), "value");
        var constant = Expression.Constant(valueToFind);
        var whereExpression = Expression.Lambda<Func<List<int>, bool>>(FilterOperators.CollectionContains.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.Should().Contain(list => list.Contains(valueToFind));
    }
}

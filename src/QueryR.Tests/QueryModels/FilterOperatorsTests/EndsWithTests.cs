using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class EndsWithTests
{
    [Theory, AutoSubData]
    public void EndsWith_ShouldWorkOnString(
        List<string> values)
    {
        //arrange
        var valueToEndWith = values.PickRandom()[^5..];

        var parameter = Expression.Parameter(typeof(string), "value");
        var constant = Expression.Constant(valueToEndWith);
        var whereExpression = Expression.Lambda<Func<string, bool>>(FilterOperators.EndsWith.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.Should().OnlyContain(value => value.EndsWith(valueToEndWith));
    }
}

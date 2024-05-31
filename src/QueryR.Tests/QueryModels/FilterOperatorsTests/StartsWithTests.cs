using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class StartsWithTests
{
    [Theory, AutoSubData]
    public void StartsWith_ShouldWorkOnString(
        List<string> values)
    {
        //arrange
        var valueToStartWith = values.PickRandom().Substring(0, 1);

        var parameter = Expression.Parameter(typeof(string), "value");
        var constant = Expression.Constant(valueToStartWith);
        var whereExpression = Expression.Lambda<Func<string, bool>>(FilterOperators.StartsWith.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.Should().OnlyContain(value => value.StartsWith(valueToStartWith));
    }
}


using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class GreaterThanTests
{
    [Theory, AutoSubData]
    public void GreaterThan_ShouldWorkOnValueTypes(
        List<int> values)
    {
        //arrange
        var valueToCompare = values.OrderBy(v => v).Skip(1).First();

        var parameter = Expression.Parameter(typeof(int), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<int, bool>>(FilterOperators.GreaterThan.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.Should().OnlyContain(value => value > valueToCompare);
    }

    [Theory, AutoSubData]
    public void GreaterThan_ShouldWorkOnDateTime(
        List<DateTime> values)
    {
        //arrange
        var valueToCompare = values.OrderBy(v => v).Skip(1).First();

        var parameter = Expression.Parameter(typeof(DateTime), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<DateTime, bool>>(FilterOperators.GreaterThan.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.Should().OnlyContain(value => value > valueToCompare);
    }
}


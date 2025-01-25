using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class GreaterThanOrEqualTests
{
    [Theory, AutoSubData]
    public void GreaterThanOrEqual_ShouldWorkOnValueTypes(
        List<int> values)
    {
        //arrange
        var valueToCompare = values.PickRandom();

        var parameter = Expression.Parameter(typeof(int), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<int, bool>>(FilterOperators.GreaterThanOrEqual.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value >= valueToCompare);
    }

    [Theory, AutoSubData]
    public void GreaterThanOrEqual_ShouldWorkOnDateTime(
        List<DateTime> values)
    {
        //arrange
        var valueToCompare = values.PickRandom();

        var parameter = Expression.Parameter(typeof(DateTime), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<DateTime, bool>>(FilterOperators.GreaterThanOrEqual.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value >= valueToCompare);
    }
}

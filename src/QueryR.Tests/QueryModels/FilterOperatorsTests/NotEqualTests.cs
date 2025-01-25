using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class NotEqualTests
{
    [Theory, AutoSubData]
    public void NotEqual_ShouldWorkOnValueTypes(
        List<int> values)
    {
        //arrange
        var valueToCompare = values.PickRandom();

        var parameter = Expression.Parameter(typeof(int), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<int, bool>>(FilterOperators.NotEqual.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value != valueToCompare);
    }

    [Theory, AutoSubData]
    public void NotEqual_ShouldWorkOnString(
        List<string> values)
    {
        //arrange
        var valueToCompare = values.PickRandom();

        var parameter = Expression.Parameter(typeof(string), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<string, bool>>(FilterOperators.NotEqual.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value != valueToCompare);
    }

    [Theory, AutoSubData]
    public void NotEqual_ShouldWorkOnGuid(
        List<Guid> values)
    {
        //arrange
        var valueToCompare = values.PickRandom();

        var parameter = Expression.Parameter(typeof(Guid), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<Guid, bool>>(FilterOperators.NotEqual.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value != valueToCompare);
    }

    [Theory, AutoSubData]
    public void NotEqual_ShouldWorkOnDateTime(
        List<DateTime> values)
    {
        //arrange
        var valueToCompare = values.PickRandom();

        var parameter = Expression.Parameter(typeof(DateTime), "value");
        var constant = Expression.Constant(valueToCompare);
        var whereExpression = Expression.Lambda<Func<DateTime, bool>>(FilterOperators.NotEqual.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value != valueToCompare);
    }
}

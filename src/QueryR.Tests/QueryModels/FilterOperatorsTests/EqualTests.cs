using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class EqualTests
{
    [Theory, AutoSubData]
    public void Equal_ShouldWorkOnValueTypes(
        List<int> values)
    {
        //arrange
        var valueToEqual = values.PickRandom();

        var parameter = Expression.Parameter(typeof(int), "value");
        var constant = Expression.Constant(valueToEqual);
        var whereExpression = Expression.Lambda<Func<int, bool>>(FilterOperators.Equal.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldHaveSingleItem();
        result.First().ShouldBe(valueToEqual);
    }

    [Theory, AutoSubData]
    public void Equal_ShouldWorkOnString(
        List<string> values)
    {
        //arrange
        var valueToEqual = values.PickRandom();

        var parameter = Expression.Parameter(typeof(string), "value");
        var constant = Expression.Constant(valueToEqual);
        var whereExpression = Expression.Lambda<Func<string, bool>>(FilterOperators.Equal.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldHaveSingleItem();
        result.First().ShouldBe(valueToEqual);
    }
    [Theory, AutoSubData]
    public void Equal_ShouldWorkOnGuid(
        List<Guid> values)
    {
        //arrange
        var valueToEqual = values.PickRandom();

        var parameter = Expression.Parameter(typeof(Guid), "value");
        var constant = Expression.Constant(valueToEqual);
        var whereExpression = Expression.Lambda<Func<Guid, bool>>(FilterOperators.Equal.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldHaveSingleItem();
        result.First().ShouldBe(valueToEqual);
    }
    [Theory, AutoSubData]
    public void Equal_ShouldWorkOnDateTime(
        List<DateTime> values)
    {
        //arrange
        var valueToEqual = values.PickRandom();

        var parameter = Expression.Parameter(typeof(DateTime), "value");
        var constant = Expression.Constant(valueToEqual);
        var whereExpression = Expression.Lambda<Func<DateTime, bool>>(FilterOperators.Equal.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldHaveSingleItem();
        result.First().ShouldBe(valueToEqual);
    }
    public class TestDummy { }
    [Theory, AutoSubData]
    public void Equal_ShouldWorkOnReferenceType(
    List<TestDummy> values)
    {
        //arrange
        var valueToEqual = values.PickRandom();

        var parameter = Expression.Parameter(typeof(TestDummy), "value");
        var constant = Expression.Constant(valueToEqual);
        var whereExpression = Expression.Lambda<Func<TestDummy, bool>>(FilterOperators.Equal.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldHaveSingleItem();
        result.First().ShouldBe(valueToEqual);
    }
}

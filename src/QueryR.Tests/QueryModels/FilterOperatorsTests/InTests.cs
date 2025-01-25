using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class InTests
{
    [Theory, AutoSubData]
    public void In_ShouldWorkOnValueTypes(
        List<int> values)
    {
        //arrange
        var valueToFind = values.PickRandom();
        var listToSearchIn = new List<int> { valueToFind };

        var parameter = Expression.Parameter(typeof(int), "value");
        var constant = Expression.Constant(listToSearchIn);
        var whereExpression = Expression.Lambda<Func<int, bool>>(FilterOperators.In.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value == valueToFind);
    }

    [Theory, AutoSubData]
    public void In_ShouldWorkOnString(
        List<string> values)
    {
        //arrange
        var valueToFind = values.PickRandom();
        var listToSearchIn = new List<string> { valueToFind };

        var parameter = Expression.Parameter(typeof(string), "value");
        var constant = Expression.Constant(listToSearchIn);
        var whereExpression = Expression.Lambda<Func<string, bool>>(FilterOperators.In.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.ShouldAllBe(value => value == valueToFind);
    }
}

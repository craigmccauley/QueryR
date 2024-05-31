using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QueryR.Tests.QueryModels.FilterOperatorsTests;
public class ContainsTests
{
    [Theory, AutoSubData]
    public void Contains_ShouldWorkOnString(
        List<string> values)
    {
        //arrange
        var valueToContain = values.PickRandom();

        var parameter = Expression.Parameter(typeof(string), "value");
        var constant = Expression.Constant(valueToContain);
        var whereExpression = Expression.Lambda<Func<string, bool>>(FilterOperators.Contains.ExpressionMethod(parameter, constant), parameter);

        //act
        var result = values.AsQueryable().Where(whereExpression).ToList();

        //assert
        result.Should().OnlyContain(value => value.Contains(valueToContain));
    }
}

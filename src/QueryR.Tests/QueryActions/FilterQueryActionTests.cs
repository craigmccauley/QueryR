using FluentAssertions;
using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static QueryR.Tests.TestHelpers.TestData;

namespace QueryR.Tests.QueryActions
{
    public class FilterQueryActionTests
    {
        [Theory, AutoSubData]
        internal void Filter_ShouldWorkAsExpected(
            FilterQueryAction sut)
        {
            //arrange
            var testData = new TestData();
            var queryResult = new QueryResult<Person>
            {
                CountQuery = testData.Persons.AsQueryable(),
                PagedQuery = testData.Persons.AsQueryable(),
            };

            var query = new Query
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        PropertyName = nameof(Person.Name),
                        Operator = FilterOperators.Equal,
                        Value = "Craig",
                    }
                }
            };


            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.Should().Be(1);
            list.First().Should().Be(testData.Craig);
        }

        [Theory, AutoSubData]
        internal void Filter_WhenItemIsOnNavigationPropertyPath_ShouldWorkAsExpected(
            FilterQueryAction sut)
        {
            //arrange
            var testData = new TestData();
            var queryResult = new QueryResult<Person>
            {
                CountQuery = testData.Persons.AsQueryable(),
                PagedQuery = testData.Persons.AsQueryable(),
            };

            var query = new Query
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        PropertyName = $"{nameof(Person.Pets)}.{nameof(Pet.PetType)}.{nameof(PetType.Name)}",
                        Operator = FilterOperators.Equal,
                        Value = "Bird",
                    }
                }
            };


            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.Should().Be(1);
            list.First().Should().Be(testData.Craig);
        }
    }
}

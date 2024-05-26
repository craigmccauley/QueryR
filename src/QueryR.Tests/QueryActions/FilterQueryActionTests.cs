using FluentAssertions;
using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System;
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

        [Theory, AutoSubData]
        internal void Filter_WhenNavigationPropertyPathIsCollectionChild_ShouldWorkAsExpected(
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
                        PropertyName = $"{nameof(Person.Pets)}.{nameof(Pet.Name)}",
                        Operator = FilterOperators.Equal,
                        Value = "Titan"
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
        internal void Filter_WhenNavigationPropertyPathIsCollection_ShouldWorkAsExpected(
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
                        PropertyName = $"{nameof(Person.AltNames)}",
                        Operator = FilterOperators.CollectionContains,
                        Value = "Robbie"
                    }
                }
            };

            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.Should().Be(1);
            list.First().Should().Be(testData.Bob);
        }

        [Theory, AutoSubData]
        internal void Filter_WhenQueryResultIsNull_ShouldThrowArgumentNullException(
            FilterQueryAction sut)
        {
            //arrange
            var query = new Query();

            //act
            var result = Record.Exception(() => sut.Execute<Person>(query, null));

            //assert
            var ex = result.Should().BeOfType<ArgumentNullException>().Which;
            ex.ParamName.Should().Be("queryResult");
            ex.Message.Should().StartWith("QueryResult cannot be null.");
        }

        [Theory, AutoSubData]
        internal void Filter_WhenPagedQueryIsNull_ShouldThrowArgumentNullException(
            FilterQueryAction sut)
        {
            // Arrange
            var query = new Query();
            var queryResult = new QueryResult<Person>
            {
                CountQuery = new List<Person>().AsQueryable(),
                PagedQuery = null
            };

            // Act
            var result = Record.Exception(() => sut.Execute(query, queryResult));

            // Assert
            var ex = result.Should().BeOfType<ArgumentNullException>().Which;
            ex.ParamName.Should().Be("queryResult.PagedQuery");
            ex.Message.Should().StartWith("PagedQuery within QueryResult cannot be null.");
        }

        [Theory, AutoSubData]
        internal void Filter_WhenCountQueryIsNull_ShouldThrowArgumentNullException(
            FilterQueryAction sut)
        {
            // Arrange
            var query = new Query();
            var queryResult = new QueryResult<Person>
            {
                CountQuery = null,
                PagedQuery = new List<Person>().AsQueryable()
            };

            // Act
            var result = Record.Exception(() => sut.Execute(query, queryResult));

            // Assert
            var ex = result.Should().BeOfType<ArgumentNullException>().Which;
            ex.ParamName.Should().Be("queryResult.CountQuery");
            ex.Message.Should().StartWith("CountQuery within QueryResult cannot be null.");
        }
    }
}

using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
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
            count.ShouldBe(1);
            list.First().ShouldBe(testData.Craig);
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
            count.ShouldBe(1);
            list.First().ShouldBe(testData.Craig);
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
            count.ShouldBe(1);
            list.First().ShouldBe(testData.Craig);
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
            count.ShouldBe(1);
            list.First().ShouldBe(testData.Bob);
        }
        [Theory, AutoSubData]
        internal void Filter_WhenParentNavigationPropertyPathIsCollection_ShouldWorkAsExpected(
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
                        PropertyName = $"{nameof(Person.Pets)}.{nameof(Pet.AltNames)}",
                        Operator = FilterOperators.CollectionContains,
                        Value = "Stinky"
                    }
                }
            };

            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.ShouldBe(2);
            list.ShouldContain(testData.Craig);
            list.ShouldContain(testData.Bob);
        }

        [Theory, AutoSubData]
        internal void Filter_WhenCollectionWithTypeMismatch_ShouldNotFilter(
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
                        PropertyName = $"{nameof(Person.Pets)}.{nameof(Pet.AltNames)}",
                        Operator = FilterOperators.Equal,
                        Value = "Stinky"
                    }
                }
            };

            //act
            QueryResult<Person> result = null;
            var exception = Record.Exception(() => result = sut.Execute(query, queryResult));

            //assert
            exception.ShouldBeNull();
            var (count, items) = result.GetCountAndList();
            items.ShouldBeEquivalentTo(testData.Persons);
        }

        [Theory, AutoSubData]
        internal void Filter_WhenObjectWithTypeMismatch_ShouldNotFilter(
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
                        PropertyName = $"{nameof(Person.Age)}",
                        Operator = FilterOperators.CollectionContains,
                        Value = "4"
                    }
                }
            };

            //act
            QueryResult<Person> result = null;
            var exception = Record.Exception(() => result = sut.Execute(query, queryResult));

            //assert
            exception.ShouldBeNull();
            var (count, items) = result.GetCountAndList();
            items.ShouldBeEquivalentTo(testData.Persons);
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
            var ex = result.ShouldBeOfType<ArgumentNullException>();
            ex.ParamName.ShouldBe("queryResult");
            ex.Message.ShouldStartWith("QueryResult cannot be null.");
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
            var ex = result.ShouldBeOfType<ArgumentNullException>();
            ex.ParamName.ShouldBe("queryResult.PagedQuery");
            ex.Message.ShouldStartWith("PagedQuery within QueryResult cannot be null.");
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
            var ex = result.ShouldBeOfType<ArgumentNullException>();
            ex.ParamName.ShouldBe("queryResult.CountQuery");
            ex.Message.ShouldStartWith("CountQuery within QueryResult cannot be null.");
        }
    }
}

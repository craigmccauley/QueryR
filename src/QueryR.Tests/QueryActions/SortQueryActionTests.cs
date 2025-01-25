using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static QueryR.Tests.TestHelpers.TestData;

namespace QueryR.Tests.QueryActions
{
    public class SortQueryActionTests
    {
        [Theory, AutoSubData]
        internal void Sort_ShouldWorkAsExpected(
            SortQueryAction sut)
        {
            //arrange
            var testData = new TestData();
            var baseQuery = testData.Persons.AsQueryable();
            var queryResult = new QueryResult<Person>
            {
                CountQuery = baseQuery,
                PagedQuery = baseQuery,
            };

            var query = new Query
            {
                Sorts = new List<Sort>
                {
                    new Sort
                    {
                        IsAscending = false,
                        PropertyName = nameof(Person.Name)
                    }
                }
            };


            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.ShouldBe(2);
            list.First().ShouldBe(testData.Craig);
            list.Skip(1).First().ShouldBe(testData.Bob);
        }

        [Theory, AutoSubData]
        internal void Sort_WhenItemIsOnNavigationPropertyPath_ShouldWorkAsExpected(
            SortQueryAction sut)
        {
            //arrange
            var testData = new TestData();
            var baseQuery = testData.Pets.AsQueryable();
            var queryResult = new QueryResult<Pet>
            {
                CountQuery = baseQuery,
                PagedQuery = baseQuery,
            };

            var query = new Query
            {
                Sorts = new List<Sort>
                {
                    new Sort
                    {
                        IsAscending = false,
                        PropertyName = $"{nameof(Pet.PetType)}.{nameof(PetType.Name)}"
                    },
                    new Sort
                    {
                        IsAscending = true,
                        PropertyName = nameof(Pet.Name)
                    }
                }
            };

            //act
            var result = sut.Execute(query, queryResult).ToList();

            //assert
            result[0].ShouldBeSameAs(testData.Rufus);
            result[1].ShouldBeSameAs(testData.Titan);
            result[2].ShouldBeSameAs(testData.Kitty);
            result[3].ShouldBeSameAs(testData.Meowswers);
            result[4].ShouldBeSameAs(testData.Tweeter);
        }
    }
}

using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System.Linq;
using Xunit;
using static QueryR.Tests.TestHelpers.TestData;

namespace QueryR.Tests.QueryActions
{
    public class PagingQueryActionTests
    {

        [Theory, AutoSubData]
        internal void Paging_ShouldWorkAsExpected(
            PagingQueryAction sut)
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
                PagingOptions = new PagingOptions
                {
                    PageNumber = 1,
                    PageSize = 1
                }
            };

            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.ShouldBe(2);
            list.First().ShouldBe(testData.Craig);
        }
    }
}

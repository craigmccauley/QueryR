using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Services;
using QueryR.Tests.TestHelpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static QueryR.Tests.TestHelpers.TestData;

namespace QueryR.Tests.QueryActions
{
    public class SparseFieldsQueryActionTests
    {
        [Theory, AutoMoqData]
        internal void SparseField_ShouldWorkAsExpected(
            [Frozen] Mock<IMaxDepthService> maxDepthServiceMock,
            SparseFieldsQueryAction sut)
        {
            //arrange
            maxDepthServiceMock.Setup(svc => svc.GetMaxDepth(It.IsAny<Query>())).Returns(new int?());
            var testData = new TestData();
            var queryResult = new QueryResult<Person>
            {
                CountQuery = testData.Persons.AsQueryable(),
                PagedQuery = testData.Persons.AsQueryable(),
            };

            var query = new Query
            {
                SparseFields = new List<SparseField>
                {
                    new SparseField
                    {
                        EntityName = nameof(Person),
                        PropertyNames = new List<string>
                        {
                            nameof(Person.Name)
                        }
                    }
                }
            };


            //act
            var result = sut.Execute(query, queryResult);

            //assert
            var (count, list) = result.GetCountAndList();
            count.Should().Be(2);
            list.First().Id.Should().Be(default);
            list.First().Name.Should().Be(testData.Craig.Name);
            list.First().Pets.Should().BeNull();
        }
    }
}

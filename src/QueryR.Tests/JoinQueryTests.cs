using FluentAssertions;
using QueryR.QueryModels;
using QueryR.Tests.TestHelpers;
using System.Linq;
using Xunit;

namespace QueryR.Tests
{
    public class JoinQueryTests
    {
        [Fact]
        internal void Query_WhenQuerableIsJoinedObject_ShouldWorkAsExpected()
        {
            //arrange
            var testData = new TestData();

            var joinedData =
                from person in testData.Persons
                join pet in testData.Pets
                on person equals pet.Owner
                select new
                {
                    OwnerName = person.Name,
                    PetName = pet.Name
                };

            var filter = new Filter
            {
                PropertyName = "OwnerName",
                Operator = FilterOperators.Equal,
                Value = "Craig"
            };

            //act
            var result = joinedData.AsQueryable().Query(filter);

            //assert
            var (Count, Items) = result.GetCountAndList();

            Count.Should().Be(3);
            foreach(var item in Items)
            {
                item.OwnerName.Should().Be("Craig");
            }
        }
    }
}

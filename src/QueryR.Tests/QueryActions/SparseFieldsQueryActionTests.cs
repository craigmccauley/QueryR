﻿using AutoFixture.Xunit2;
using NSubstitute;
using QueryR.QueryActions;
using QueryR.QueryModels;
using QueryR.Services;
using QueryR.Tests.TestHelpers;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static QueryR.Tests.TestHelpers.TestData;

namespace QueryR.Tests.QueryActions
{
    public class SparseFieldsQueryActionTests
    {
        [Theory, AutoSubData]
        internal void SparseField_ShouldWorkAsExpected(
            [Frozen] IMaxDepthService maxDepthServiceMock,
            SparseFieldsQueryAction sut)
        {
            //arrange
            maxDepthServiceMock
                .GetMaxDepth(Arg.Any<Query>())
                .Returns(new int?());
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
            count.ShouldBe(2);
            list.First().Id.ShouldBe(default);
            list.First().Name.ShouldBe(testData.Craig.Name);
            list.First().Pets.ShouldBeNull();
        }
    }
}

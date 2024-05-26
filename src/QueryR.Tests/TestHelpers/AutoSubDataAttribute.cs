using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace QueryR.Tests.TestHelpers
{
    internal class AutoSubDataAttribute : AutoDataAttribute
    {
        public AutoSubDataAttribute()
            : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
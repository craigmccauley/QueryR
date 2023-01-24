using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xunit;
using Xunit.Sdk;

namespace QueryR.Tests.TestHelpers
{
    //Work around for 
    //MemberAutoData only uses first entry from the supplied enumerable #1142
    //https://github.com/AutoFixture/AutoFixture/issues/1142
    //Code taken from here
    //https://github.com/AutoFixture/AutoFixture/issues/1142#issuecomment-545579385
    internal class MemberAutoDataAttribute : DataAttribute
    {
        private readonly Lazy<IFixture> fixture;
        private readonly MemberDataAttribute memberDataAttribute;

        public MemberAutoDataAttribute(string memberName, params object[] parameters)
          : this(memberName, parameters, () => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }

        protected MemberAutoDataAttribute(string memberName, object[] parameters, Func<IFixture> fixtureFactory)
        {
            if (fixtureFactory == null)
            {
                throw new ArgumentNullException(nameof(fixtureFactory));
            }

            memberDataAttribute = new MemberDataAttribute(memberName, parameters);
            fixture = new Lazy<IFixture>(fixtureFactory, LazyThreadSafetyMode.PublicationOnly);
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            var memberData = memberDataAttribute.GetData(testMethod);

            using var enumerator = memberData.GetEnumerator();
            if (enumerator.MoveNext())
            {
                var specimens = GetSpecimens(testMethod.GetParameters(), enumerator.Current.Length).ToArray();

                do
                {
                    yield return enumerator.Current.Concat(specimens).ToArray();
                } while (enumerator.MoveNext());
            }
        }

        private IEnumerable<object> GetSpecimens(IEnumerable<ParameterInfo> parameters, int skip)
        {
            foreach (var parameter in parameters.Skip(skip))
            {
                CustomizeFixture(parameter);

                yield return Resolve(parameter);
            }
        }

        private void CustomizeFixture(ParameterInfo p)
        {
            var customizeAttributes = p.GetCustomAttributes()
                .OfType<IParameterCustomizationSource>()
                .OrderBy(x => x, new CustomizeAttributeComparer());

            foreach (var ca in customizeAttributes)
            {
                var c = ca.GetCustomization(p);
                fixture.Value.Customize(c);
            }
        }

        private object Resolve(ParameterInfo p)
        {
            var context = new SpecimenContext(fixture.Value);

            return context.Resolve(p);
        }

        private class CustomizeAttributeComparer : Comparer<IParameterCustomizationSource>
        {
            public override int Compare(IParameterCustomizationSource x, IParameterCustomizationSource y)
            {
                var xfrozen = x is FrozenAttribute;
                var yfrozen = y is FrozenAttribute;

                if (xfrozen && !yfrozen)
                {
                    return 1;
                }

                if (yfrozen && !xfrozen)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}
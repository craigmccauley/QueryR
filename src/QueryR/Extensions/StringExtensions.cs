using System;
using System.ComponentModel;

namespace QueryR.Extensions
{
    internal static class StringExtensions
    {
        public static object Convert(this string input, Type type)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter != null)
                {
                    return converter.ConvertFromString(input);
                }
                return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}

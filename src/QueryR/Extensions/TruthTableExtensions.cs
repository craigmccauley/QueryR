namespace QueryR.Extensions
{
    internal static class TruthTableExtensions
    {
        public static T TruthTable<T>(this (bool A, bool B) inputs, T notANotB, T notAB, T ANotB, T AB)
        {
            return inputs switch
            {
                (true, true) => AB,
                (true, false) => ANotB,
                (false, true) => notAB,
                _ => notANotB
            };
        }
    }
}

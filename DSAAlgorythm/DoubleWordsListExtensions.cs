using System.Collections.Generic;

namespace DSAAlgorythm
{
    public static class DoubleWordsListExtensions
    {
        public static bool IsEqualOrGreaterThan(this List<uint> a, List<uint> b)
        {
            bool areEqual = a.Count == b.Count;
            if (areEqual)
            {
                for (int i = a.Count - 1; i >= 0; i--)
                {
                    if (a[i] > b[i]) return true;
                    if (b[i] > a[i]) return false;
                }

                return true;
            }
            else
            {
                return a.Count >= b.Count;
            }
        }
    }
}

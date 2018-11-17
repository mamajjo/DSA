using System.Numerics;

namespace DSAAlgorythm
{
    public class Signature
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }
        public Signature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}

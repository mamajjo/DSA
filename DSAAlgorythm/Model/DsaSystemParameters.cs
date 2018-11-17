using System.Numerics;
using System.Security.Cryptography;

namespace DSAAlgorythm.Model
{
    // for parameters integrity, objects of this class are immutable
    public class DsaSystemParameters
    {
        public BigInteger P { get; }
        public BigInteger Q { get; }
        public BigInteger G { get; }
        public Hasher HashFunction { get; set; }

        public DsaSystemParameters(BigInteger p, BigInteger q, BigInteger g)
        {
            P = p;
            Q = q;
            G = g;
        }
    }
}

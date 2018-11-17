using System.Numerics;

namespace DSAAlgorythm.Model
{
    public class KeyPair
    {
        public BigInteger PrivateKey { get; }
        public BigInteger PublicKey { get; }

        public KeyPair(BigInteger privateKey, BigInteger publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }
}

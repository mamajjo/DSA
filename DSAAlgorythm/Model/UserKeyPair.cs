using System.Numerics;

namespace DSAAlgorythm.Model
{
    public class UserKeyPair
    {
        public BigInteger PrivateKey { get; }
        public BigInteger PublicKey { get; }

        public UserKeyPair(BigInteger privateKey, BigInteger publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }
}

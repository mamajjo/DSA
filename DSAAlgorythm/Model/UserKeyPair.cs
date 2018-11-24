using System.Numerics;

namespace DSAAlgorythm.Model
{
    public class UserKeyPair
    {
        public BigInteger PrivateKey { get; set; }
        public BigInteger PublicKey { get; set; }

        public UserKeyPair(BigInteger privateKey, BigInteger publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        public UserKeyPair()
        {
        }
    }
}

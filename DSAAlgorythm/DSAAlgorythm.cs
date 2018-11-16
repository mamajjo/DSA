using DSAAlgorythm.Data;
using System;
using System.Security.Cryptography;
using static DSAAlgorythm.Hasher;

namespace DSAAlgorythm
{

    public class DSAAlgorythm
    {
        public BigInteger P { get; set; }
        public BigInteger G { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger R{ get; set; }
        public BigInteger S { get; set; }
        public BigInteger K { get; set; }
        public BigInteger PrivateKey { get; set; }
        public BigInteger PublicKey { get; set; }
        public BigInteger UserPublicKey { get; set; }
        public BigInteger Hash { get; set; }
        private IDataProvider Data { get; }
        public byte[] BytesMessage { get; set; }
        private Hasher HasherImplementation { get; }


        public DSAAlgorythm(IDataProvider dataProvider, HashImplementation hashImplementation)
        {
            BytesMessage = dataProvider.GetData();
            HasherImplementation = new Hasher(hashImplementation);
            Hash = new BigInteger(HasherImplementation.GetHashedMessage(BytesMessage));
        }

        public Signature Signing()
        {
            R = (G.ModPow(K, P)) % Q;
            // TODO power operation
            var reverseHelperBI = new BigInteger(1);
            S = [reverseHelperBI/K * (Hash + PrivateKey * R)] % Q;
            return new Signature(R,S);
        }

        public bool Verifying(Signature signature)
        {
            var reverseHelperBI = new BigInteger(1);
            BigInteger sPrime = reverseHelperBI / signature.S; 
            BigInteger w = sPrime % Q;
            BigInteger a = Hash.ModPow(w, Q);
            BigInteger rPrime = R * w;
            BigInteger b = rPrime % Q;
            //(A * B) mod C = (A mod C * B mod C) mod C
            //g^a * y^b) % p )%g
            BigInteger v = ((G.ModPow(a, P) * UserPublicKey.ModPow(b, P)) % P) % Q;
            if ( v == R)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
   
}

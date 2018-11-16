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
            //R = (Math.Pow(G, K) % P) % Q;
            // TODO power operation
            S = [(Math.Pow(K, -1.0) * (Hash + PrivateKey * r))] % Q;
            // TODO revert BigInt Operation
            return new null;
        }

        public Verifying(Signature signature, )


    }
   
}

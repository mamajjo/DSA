using System;

namespace DSAAlgorythm
{

    public class DSAAlgorythm
    {
        public BigInt P { get; set; }
        public BigInt G { get; set; }
        public BigInt Q { get; set; }
        public BigInt PrivateKey { get; set; }
        public BigInt PublicKey { get; set; }
        public BigInt K { get; set; }

        public BigInt Hash { get; set; }


        public Signature Signing()
        {
            BigInt r, s;
            r = (Math.Pow(G, K) % P) % Q;
            // TODO power operation
            s = [(Math.Pow(K, -1.0) * (Hash + PrivateKey * r))] % Q;
            // TODO revert BigInt Operation
            return new Signature(r, s);
        }

        public Verifying(Signature signature, )


    }
   
}

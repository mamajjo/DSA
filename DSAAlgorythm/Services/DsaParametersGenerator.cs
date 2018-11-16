using System;
using System.Numerics;
using System.Security.Cryptography;
using DSAAlgorythm.ExtensionMethods;
using DSAAlgorythm.Model;
using DSAAlgorythm.Services.Interface;

namespace DSAAlgorythm.Services
{
    public class DsaParametersGenerator
    {
        private static RNGCryptoServiceProvider _rndProvider = new RNGCryptoServiceProvider();
        private IPrimalityTester _primalityTester = new MillerRabinPrimalityTester(20);

        public DsaSystemParameters GenerateParameters(int N, int L, int seedlen)
        {
            //N and L have to be acceptable

            if (seedlen < N)
                throw new ArgumentException("Seedlen cannot be less then N");

            //Bit length of the output block of choosen hash function, shall be equal to or greater than N
            int outlen = 160;
            BigInteger outlenPow = BigInteger.Pow(2, seedlen);


            int n = (int)Math.Ceiling((double)L / outlen) - 1;

            int b = L - 1 - (n * outlen);

            int remainingBits = seedlen % 8;
            byte[] domain_parameter_seed;
            if (remainingBits == 0)
            {
                domain_parameter_seed = new byte[seedlen / 8];
            }
            else
            {
                domain_parameter_seed = new byte[(seedlen / 8) + 1];
            }

            BigInteger q;

            bool backToStep1 = true;
            bool backToStep7 = true;

            do
            {
                do
                {
                    q = SetQ(outlenPow, remainingBits, domain_parameter_seed);

                } while (_primalityTester.CheckPrimality(q));

                int counter = 0, offset = 2;
                do
                {
                    BigInteger p = SetP(L, outlen, outlenPow, n, b, domain_parameter_seed, q, ref counter, ref offset);

                    if (p < BigInteger.Pow(2, L - 1))
                    {
                        counter++;
                        offset = offset + n + 1;
                        if (counter == 4096)
                        {
                            backToStep1 = true;
                            backToStep7 = false;
                        }
                        else
                        {
                            backToStep1 = false;
                            backToStep7 = true;
                        }
                    }
                    else
                    {
                        if (_primalityTester.CheckPrimality(p))
                        {
                            return new KeyPair() { p = p, q = q };
                        }
                        else
                        {
                            counter++;
                            offset = offset + n + 1;
                            if (counter == 4096)
                            {
                                backToStep1 = true;
                                backToStep7 = false;
                            }
                            else
                            {
                                backToStep1 = false;
                                backToStep7 = true;
                            }
                        }
                    }
                } while (backToStep7);

            } while (backToStep1);

            return null;

            //BigInteger domain_parameter_seed = new BigInteger();
        }

        private BigInteger SetQ(BigInteger outlenPow, int remainingBits, byte[] domain_parameter_seed)
        {
            BigInteger q;
            _rndProvider.GetBytes(domain_parameter_seed);

            if (remainingBits > 0)
            {
                byte mask = 0;
                for (int i = 0; i < remainingBits; i++)
                {
                    mask |= (byte)(1 << i);
                }

                domain_parameter_seed[domain_parameter_seed.Length - 1] &= mask;
            }

            domain_parameter_seed[domain_parameter_seed.Length - 1] |= (byte)(1 << (remainingBits > 0 ? remainingBits : 7));

            //set last bit of domain parameter seed to 1

            byte[] hashedDomain;
            byte[] hashedMod;

            byte[] Utmp;
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                hashedDomain = sha1.ComputeHash(domain_parameter_seed);
                byte[] domain_parameter_seed_with_zero = new byte[domain_parameter_seed.Length + 1];
                domain_parameter_seed.CopyTo(domain_parameter_seed_with_zero, 0);
                BigInteger temporary = new BigInteger(domain_parameter_seed_with_zero) + BigInteger.One;
                hashedMod = sha1.ComputeHash((temporary % outlenPow).ToByteArray());

                Utmp =hashedDomain.Xor(hashedMod);
            }

            Utmp[0] |= 1;
            Utmp[Utmp.Length - 1] |= (byte)(1 << 7);
            byte[] U = new byte[Utmp.Length + 1];
            Utmp.CopyTo(U, 0);

            q = new BigInteger(U);
            return q;
        }

        private static BigInteger SetP(int L, int outlen, BigInteger outlenPow, int n, int b, byte[] domain_parameter_seed, BigInteger q, ref int counter, ref int offset)
        {
            BigInteger[] V = new BigInteger[n + 1];


            for (int j = 0; j <= n; j++)
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    BigInteger temp = new BigInteger(domain_parameter_seed) + new BigInteger(offset) + new BigInteger(j);
                    V[j] = new BigInteger(sha1.ComputeHash((temp % outlenPow).ToByteArray()));
                }
            }

            BigInteger W = new BigInteger();

            for (int j = 0; j < n; j++)
            {
                BigInteger exp = BigInteger.Pow(2, j * outlen);
                W += V[j] * exp;
            }

            W += (V[n] % BigInteger.Pow(2, b)) * BigInteger.Pow(2, n * outlen);

            BigInteger X = W + BigInteger.Pow(2, L - 1);

            BigInteger c = X % (new BigInteger(2) * q);

            return (X - (c - 1));
        }
    }
}

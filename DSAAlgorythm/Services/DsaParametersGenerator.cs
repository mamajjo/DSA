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
        private readonly IPrimalityTester _primalityTester = new MillerRabinPrimalityTester(40);

        public DsaSystemParameters GenerateParameters(int L, int N, int seedLen)
        {
            #region CONDITIONS

            //SHA-1 used
            //Bit length of the output block of choosen hash function, shall be equal to or greater than N
            int outlen = 160;

            // check combination support
            if(!CheckLengthCombinations(L, N))
                throw new ArgumentException($"Combination of lengths: L={L} and N={N} is not supported");
            
            if(outlen < N) 
                throw new ArgumentException("Outlen cannot be less than N");

            // insisted by FIPS standard
            if (seedLen < N)
                throw new ArgumentException("Seedlen cannot be less then N");

            #endregion

            // calculate equation parameters L - 1 = n * 160(outlen) + b
            int n = (int)Math.Ceiling((double)L / outlen) - 1;
            int b = L - 1 - (n * outlen);

            // create container for seed
            int remainingBits = seedLen % 8;
            byte[] seed = new byte[(seedLen/8) + (remainingBits == 0 ? 0 : 1)];

            BigInteger outlenPow = BigInteger.Pow(2, seedLen);
            BigInteger p, q;

            bool outerLoop = true;
            do
            {
                q = GenerateQ(outlenPow, remainingBits, seed);

                bool innerLoop = true;
                int counter = 0, offset = 2;
                do
                {
                    p = GenerateP(L, outlen, outlenPow, n, b, seed, q, ref offset);

                    if (p < BigInteger.Pow(2, L - 1))
                    {
                        UpdateControlVariables(n, ref outerLoop, ref innerLoop, ref counter, ref offset);
                    }
                    else
                    {
                        if (_primalityTester.CheckPrimality(p))
                        {
                            outerLoop = false;
                            innerLoop = false;
                        }
                        else
                        {
                            UpdateControlVariables(n, ref outerLoop, ref innerLoop, ref counter, ref offset);
                        }
                    }
                } while (innerLoop);

            } while (outerLoop || (p-1) % q != 0);

            BigInteger g = GenerateG(p, q);

            return new DsaSystemParameters(p, q, g);
        }

        private BigInteger GenerateQ(BigInteger outlenPow, int remainingBits, byte[] seed)
        {
            BigInteger q;
            do
            {
                CryptoRandomNumberProvider.RndProvider.GetBytes(seed);

                // turn off additional bits and set the most significant bit to 1
                if (remainingBits > 0)
                {
                    byte mask = (byte)(0xFF >> (8 - remainingBits));
                    seed[seed.Length - 1] &= mask;
                    seed[seed.Length - 1] |= (byte)(1 << (remainingBits-1));
                }
                else
                {
                    seed[seed.Length - 1] |= (byte) (1 << 7);
                }

                byte[] u;
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hashedSeed = sha1.ComputeHash(seed);
                    BigInteger tmp = seed.CreatePositiveBigInteger() + BigInteger.One;
                    byte[] hashedMod = sha1.ComputeHash((tmp % outlenPow).ToByteArray());

                    u = hashedSeed.Xor(hashedMod);
                }

                // set the least and most significant bit to 1
                u[0] |= 1;
                u[u.Length - 1] |= (byte)(1 << 7);

                q = u.CreatePositiveBigInteger();

            } while (_primalityTester.CheckPrimality(q));

            return q;
        }

        private BigInteger GenerateP(int L, int outlen, BigInteger outlenPow, int n, int b, byte[] seed, BigInteger q, ref int offset)
        {
            BigInteger[] V = new BigInteger[n + 1];

            for (int j = 0; j <= n; j++)
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    BigInteger tmp = seed.CreatePositiveBigInteger() + offset + j;
                    V[j] = new BigInteger(sha1.ComputeHash((tmp % outlenPow).ToByteArray()));
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

            BigInteger c = X % (q * 2);

            return (X - (c - 1));
        }

        private void UpdateControlVariables(int n, ref bool outerLoop, ref bool innerLoop, ref int counter, ref int offset)
        {
            counter++;
            offset += (n + 1);
            if (counter == 4096)
            {
                outerLoop = true;
                innerLoop = false;
            }
            else
            {
                outerLoop = false;
                innerLoop = true;
            }
        }

        private BigInteger GenerateG(BigInteger p, BigInteger q)
        {
            BigInteger a = new BigInteger();
            for (BigInteger h = 2; h < q; h++)
            {
                BigInteger exponent = (p - 1) / q;
                a = BigInteger.ModPow(h, exponent, p);
                if (a != 1)
                {
                    break;
                }
            }
            return a;
        }

        private bool CheckLengthCombinations(int l, int n)
        {
            return l == 1024 && n == 160;
        }
    }
}

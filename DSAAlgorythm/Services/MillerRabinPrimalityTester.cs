using System.Numerics;
using System.Security.Cryptography;
using DSAAlgorythm.Services.Interface;

namespace DSAAlgorythm.Services
{
    public class MillerRabinPrimalityTester : IPrimalityTester
    {
        private readonly int _confidence;

        public MillerRabinPrimalityTester(int confidence)
        {
            _confidence = confidence;
        }

        public bool CheckPrimality(BigInteger p)
        {
            //handle cases < 4 and even numbers
            if (p == 2 || p == 3)
                return true;
            if (p < 2 || p.IsEven)
                return false;

            BigInteger pSub1 = p - 1;
            int s = 0;

            while (pSub1 % 2 == 0)
            {
                pSub1 /= 2;
                s += 1;
            }

            int bytesLength = p.ToByteArray().Length;

            for (int i = 0; i < _confidence; i++)
            {
                BigInteger a = GenerateRandomNumber(2, p - 1, bytesLength);

                BigInteger x = BigInteger.ModPow(a, pSub1, p);
                if (x == 1 || x == p - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, p);
                    if (x == 1)
                        return false;
                    if (x == p - 1)
                        break;
                }

                if (x != p - 1)
                    return false;
            }

            return true;
        }

        private BigInteger GenerateRandomNumber(BigInteger lowerBound, BigInteger upperBound, int bytesLength)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            BigInteger a;
            byte[] bytes = new byte[bytesLength];

            do
            {
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a < lowerBound || a > upperBound);

            return a;
        }
    }
}

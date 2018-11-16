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

        public bool CheckPrimality(BigInteger n)
        {
            //handle cases < 4 and even numbers
            if (n == 2 || n == 3)
                return true;
            if (n < 2 || n.IsEven)
                return false;

            BigInteger nm1 = n - 1;
            int s = 0;

            while (nm1 % 2 == 0)
            {
                nm1 /= 2;
                s += 1;
            }

            int bytesLength = n.ToByteArray().Length;

            for (int i = 0; i < _confidence; i++)
            {

                BigInteger a = GenerateRandomNumber(2, n - 1, bytesLength);

                BigInteger x = BigInteger.ModPow(a, nm1, n);
                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
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

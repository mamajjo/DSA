using System;
using System.Numerics;
using System.Security.Cryptography;

namespace DSAAlgorythm.Services
{
    public static class CryptoRandomNumberProvider
    {
        public static RNGCryptoServiceProvider RndProvider = new RNGCryptoServiceProvider();
        private static Random Random = new Random();

        public static BigInteger GenerateRandomBigInteger(BigInteger lowerBound, BigInteger upperBound)
        {
            int bytesCount = GetRandomNumberOfBytes(lowerBound, upperBound);
            byte[] bytes = new byte[bytesCount];

            BigInteger a;
            do
            {
                RndProvider.GetBytes(bytes);
                a = new BigInteger(bytes);
            } while (a < lowerBound || a > upperBound);

            return a;
        }

        private static int GetRandomNumberOfBytes(BigInteger lowerBound, BigInteger upperBound)
        {
            byte[] lowerBytes = lowerBound.ToByteArray();
            byte[] upperBytes = upperBound.ToByteArray();
            int lowerCount = lowerBytes[lowerBytes.Length - 1] == 0 ? lowerBytes.Length - 1 : lowerBytes.Length;
            int upperCount = upperBytes[upperBytes.Length - 1] == 0 ? upperBytes.Length - 1 : upperBytes.Length;

            int random;
            do
            {
                random = Random.Next(lowerCount, upperCount);
            } while (random == 0);

            return random;
        }
    }
}
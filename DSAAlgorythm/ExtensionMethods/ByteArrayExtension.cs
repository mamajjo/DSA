using System;
using System.Numerics;

namespace DSAAlgorythm.ExtensionMethods
{
    public static class ByteArrayExtension
    {
        public static byte[] Xor(this byte[] a, byte[] b)
        {
            if(a.Length != b.Length)
                throw new ArgumentException("Arrays should be the same length");

            byte[] xorResult = new byte[a.Length];

            for (int i = 0; i < xorResult.Length; i++)
            {
                xorResult[i] = (byte)(a[i] ^ b[i]);
            }

            return xorResult;
        }

        // big integers requires for constructor an array of bytes in little endian 
        // when most significant bit is 1, number is negative
        // to prevent default behaviour when most significant bit is 1, add additional zero byte 0x00 to the end
        public static BigInteger CreatePositiveBigInteger(this byte[] data)
        {
            int byteCount = data.Length;

            if ((data[byteCount - 1] & 0x80) != 0x80) return new BigInteger(data);

            // the most sgnificant bit is 1, number is negative
            byte[] positive = new byte[byteCount + 1];
            data.CopyTo(positive, 0);
            return new BigInteger(positive);
        }
    }
}

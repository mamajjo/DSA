using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSAAlgorythm.ExtensionMethods
{
    public static class StringExtension
    {
        public static byte[] ToByteArray(this string binaryString)
        {
            int count = binaryString.Length / 8;
            var b = new byte[count];
            for (int i = 0; i < count; i++)
                b[i] = Convert.ToByte(binaryString.Substring(i * 8, 8), 2);

            return b;
        }
    }
}

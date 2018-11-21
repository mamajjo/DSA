using System;
using System.Numerics;
using System.Text;
using DSAAlgorythm.Data;
using DSAAlgorythm.ExtensionMethods;

namespace DSAAlgorythm
{
    public class Signature
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }
        public Signature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
        //https://stackoverflow.com/questions/16072709/converting-string-to-byte-array-in-c-sharp
        public void ToBinaryStringFile()
        {
            var s = new StringBuilder();
            s.Append(Encoding.ASCII.GetBytes(R.ToString()));
            s.Append("\n");
            s.Append(Encoding.ASCII.GetBytes(R.ToString()));
            string s1 = s.ToString();
            FileSaver.SaveTextToFile(s.ToString());
           
            //saving Signature
            // TODO save it to txt and save to fileSystem
        }

        public static Signature FromBinaryString(string s)
        {
            var enterPosition = s.IndexOf("\n");
            var rPartOfString = s.Substring(0, enterPosition);
            var sPartOfString = s.Substring(enterPosition + 1, s.Length - enterPosition);
            BigInteger rFromFile = new BigInteger(rPartOfString.ToByteArray());
            BigInteger sFromFile = new BigInteger(sPartOfString.ToByteArray());

            return new Signature(rFromFile, sFromFile);
        }
    }
}

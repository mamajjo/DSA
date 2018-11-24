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
            StringBuilder sb = new StringBuilder();
            byte[] Rarray = R.ToByteArray();
            foreach (byte item in Rarray)
            {
                sb.Append(item + " ");
            }
            byte[] Sarray = S.ToByteArray();
            sb.Append("-");
            foreach (byte item in Sarray)
            {
                sb.Append(item + " ");
            }
            FileSaver.SaveTextToFile(sb.ToString());
           
            //saving Signature
            // TODO save it to txt and save to fileSystem
        }

        public static Signature FromBinaryString(string path)
        {
            string s = FileSaver.ReadTextToStrign(path);
            var enterPosition = s.IndexOf("-");
            var rPartOfString = s.Substring(0, enterPosition);
            var sPartOfString = s.Substring(enterPosition + 1);
            BigInteger rFromFile = new BigInteger(rPartOfString.ToByteArray());
            BigInteger sFromFile = new BigInteger(sPartOfString.ToByteArray());

            return new Signature(rFromFile, sFromFile);
        }
    }
}

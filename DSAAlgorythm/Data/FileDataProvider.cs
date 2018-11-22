using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSAAlgorythm.Data
{
    public class FileDataProvider : IDataProvider
    {
        public string FilePath { get; }
        public FileDataProvider(string filePath)
        {
            if(filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Provided file path is not reachable");
            }
            FilePath = filePath;
        }
        public byte[] GetData()
        {
            if (!File.Exists(FilePath))
            {
                throw new ArgumentException("Provided file path is not reachable");
            }

            return File.ReadAllBytes(FilePath);
        }
        public string GetStringData()
        {

            if (!File.Exists(FilePath))
            {
                throw new ArgumentException("Provided file path is not reachable");
            }
            StringBuilder sb = new StringBuilder();
            // TODO bytes from file to separate bytes
            List<string> sL= File.ReadLines(FilePath, System.Text.Encoding.Default).ToList();
           // List<string> sL = File.Read(FilePath).ToList();
            sb.Append(sL.ElementAt(0));
            sb.Append("-");
            sb.Append(sL.ElementAt(1));

            return sb.ToString();
        }
    }
}

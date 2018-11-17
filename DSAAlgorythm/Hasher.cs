using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel;

namespace DSAAlgorythm
{
    public class Hasher
    {
        public enum HashImplementation
        {
            [Description("MD5")]
            md5,

            [Description("SHA256")]
            sha256,

            [Description("SHA515")]
            sha512
        }
      //  public byte[] Message { get; set; }
        private HashAlgorithm HashAlgorithm { get; set; }
        private HashImplementation hashImplementation;
        public Hasher(HashImplementation _hashImplementation)
        {
            hashImplementation = _hashImplementation;
        }

        public byte[] GetHashedMessage(byte[] message)
        {
            switch (hashImplementation)
            {
                case HashImplementation.md5:
                    {
                        using (MD5 md5Hash = MD5.Create())
                        {
                            byte[] hash = md5Hash.ComputeHash(message);
                            return hash;

                            
                        }
                    }
                case HashImplementation.sha256:
                    {
                        using (SHA256 md5Hash = SHA256.Create())
                        {
                            byte[] hash = md5Hash.ComputeHash(message);
                            return hash;


                        }
                    }
                case HashImplementation.sha512:
                    {
                        using (SHA512 md5Hash = SHA512.Create())
                        {
                            byte[] hash = md5Hash.ComputeHash(message);
                            return hash;


                        }
                    }
                default: return null;
            }
        }
    }
}

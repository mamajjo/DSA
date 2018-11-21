using System.Security.Cryptography;
using System.ComponentModel;

namespace DSAAlgorythm
{
    public class Hasher
    {
        public enum HashImplementation
        {
            [Description("MD5")] Md5,

            [Description("SHA-1")] Sha1,

            [Description("SHA256")] Sha256,

            [Description("SHA515")] Sha512
        }

        private HashImplementation _hashImplementation;

        public Hasher(HashImplementation hashImplementation)
        {
            _hashImplementation = hashImplementation;
        }
        /// <summary>
        /// Allows to define what type of hash method is going to be used in algorithm. Default: MD5
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        public byte[] GetHashedMessage(byte[] message)
        {
            switch (_hashImplementation)
            {
                case HashImplementation.Sha1:
                {
                    using (SHA1Managed sha1 = new SHA1Managed())
                    {
                        return sha1.ComputeHash(message);
                    }
                }
                case HashImplementation.Md5:
                {
                    using (MD5 md5Hash = MD5.Create())
                    {
                        byte[] hash = md5Hash.ComputeHash(message);
                        return hash;
                    }
                }
                case HashImplementation.Sha256:
                {
                    using (SHA256 md5Hash = SHA256.Create())
                    {
                        byte[] hash = md5Hash.ComputeHash(message);
                        return hash;
                    }
                }
                case HashImplementation.Sha512:
                {
                    using (SHA512 md5Hash = SHA512.Create())
                    {
                        byte[] hash = md5Hash.ComputeHash(message);
                        return hash;
                    }
                }
                default:
                    {
                        using (MD5 md5Hash = MD5.Create())
                        {
                            byte[] hash = md5Hash.ComputeHash(message);
                            return hash;
                        }
                    }
            }
        }
    }
}
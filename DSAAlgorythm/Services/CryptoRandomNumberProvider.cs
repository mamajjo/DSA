using System.Security.Cryptography;

namespace DSAAlgorythm.Services
{
    public static class CryptoRandomNumberProvider
    {
        public static RNGCryptoServiceProvider RndProvide = new RNGCryptoServiceProvider();
    }
}

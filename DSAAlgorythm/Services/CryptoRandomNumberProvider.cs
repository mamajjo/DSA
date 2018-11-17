using System.Security.Cryptography;

namespace DSAAlgorythm.Services
{
    public static class CryptoRandomNumberProvider
    {
        public static RNGCryptoServiceProvider RndProvider = new RNGCryptoServiceProvider();
    }
}

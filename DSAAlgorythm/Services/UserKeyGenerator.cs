using System;
using System.Numerics;
using DSAAlgorythm.Model;

namespace DSAAlgorythm.Services
{
    public class UserKeyGenerator
    {
        private DsaSystemParameters _systemParameters;
        public DsaSystemParameters SystemParameters
        {
            get => _systemParameters;
            set => _systemParameters = value ?? throw new ArgumentNullException(nameof(SystemParameters));
        }

        public UserKeyGenerator(DsaSystemParameters systemParameters)
        {
            SystemParameters = systemParameters ?? throw new ArgumentNullException(nameof(systemParameters));
        }

        public UserKeyPair GenerateKeyPair()
        {
            BigInteger q = _systemParameters.Q;
            int byteLength = q.ToByteArray().Length;
            byte[] random = new byte[byteLength];
            BigInteger x;

            CryptoRandomNumberProvider.RndProvider.GetBytes(random);
            x = (BigInteger.Abs((new BigInteger(random))  % (q - 1)) + 1);

            BigInteger y = BigInteger.ModPow(_systemParameters.G, x, _systemParameters.P);

            return new UserKeyPair(x, y);
        }
    }
}

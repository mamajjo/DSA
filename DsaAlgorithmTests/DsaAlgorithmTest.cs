using DSAAlgorythm;
using DSAAlgorythm.Model;
using DSAAlgorythm.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsaAlgorithmTests
{
    [TestClass]
    public class DsaAlgorithmTest
    {
        private readonly byte[] _sampleMessage = new byte[] {0xFF, 0xF2, 0xF3, 0x24, 0x21, 0x64, 0x78, 0x21};
        private DsaAlgorithm _dsaAlgorithm;
        private DsaSystemParameters _dsaParameters;
        private UserKeyPair _keyPair;

        [TestInitialize]
        public void SetUp()
        {
            DsaParametersGenerator paramGen = new DsaParametersGenerator();
            _dsaParameters = paramGen.GenerateParameters(1024, 160, 160);
            _dsaParameters.HashFunction = new Hasher(Hasher.HashImplementation.Sha1);

            UserKeyGenerator keyGen = new UserKeyGenerator(_dsaParameters);
            _keyPair = keyGen.GenerateKeyPair();

            _dsaAlgorithm = new DsaAlgorithm {Parameters = _dsaParameters};
        }


        [TestMethod]
        public void When_SignCalledWithProperParameters_VerifyShould_ReturnTrue()
        {
            Signature signature = _dsaAlgorithm.Sign(_sampleMessage, _keyPair.PrivateKey);

            _dsaAlgorithm.Verify(_sampleMessage, signature, _keyPair.PublicKey).Should().BeTrue();
        }
    }
}

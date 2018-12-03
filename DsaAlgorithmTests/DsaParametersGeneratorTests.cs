using System.Numerics;
using DSAAlgorythm.Model;
using DSAAlgorythm.Services;
using DSAAlgorythm.Services.Interface;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsaAlgorithmTests
{
    [TestClass]
    public class DsaParametersGeneratorTests
    {
        private DsaParametersGenerator _generator;
        private IPrimalityTester _primalityTester;

        [TestInitialize]
        public void SetUp()
        {
            _generator = new DsaParametersGenerator();
            _primalityTester = new MillerRabinPrimalityTester(40);
        }

        [TestMethod]
        public void When_GenerateParameters_Should_ReturnCorrectValues()
        {
            for (int i = 0; i < 3; i++)
            {
                DsaSystemParameters parameters = _generator.GenerateParameters(1024, 160, 160);

                _primalityTester.CheckPrimality(parameters.Q).Should().BeTrue();
                _primalityTester.CheckPrimality(parameters.P).Should().BeTrue();

                //p - 1 is a multiple of q
                ((parameters.P - 1) % parameters.Q).Should().BeEquivalentTo(BigInteger.Zero);

                //g^q is congruent to mod 1
                BigInteger.ModPow(parameters.G, parameters.Q, parameters.P).Should().BeEquivalentTo(BigInteger.One);
            }
        }
    }
}

using System;
using System.Numerics;
using DSAAlgorythm;
using DSAAlgorythm.Model;
using DSAAlgorythm.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsaAlgorithmTests
{
    [TestClass]
    public class DsaParametersGeneratorTests
    {
        private DsaParametersGenerator _generator;

        [TestInitialize]
        public void SetUp()
        {
            _generator = new DsaParametersGenerator();
        }

        [TestMethod]
        public void When_GenerateParameters_Should_ReturnCorrectValues()
        {
            DsaSystemParameters parameters =_generator.GenerateParameters(1024, 160, 160);


            //p - 1 is a multiple of q
            ((parameters.P - 1) % parameters.Q).Should().BeEquivalentTo(BigInteger.Zero);

            //g^q is congruent to mod 1
            BigInteger.ModPow(parameters.G, parameters.Q, parameters.P).Should().BeEquivalentTo(BigInteger.One);
        }
    }
}

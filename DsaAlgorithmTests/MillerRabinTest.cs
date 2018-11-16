using System;
using System.Numerics;
using DSAAlgorythm.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsaAlgorithmTests
{
    [TestClass]
    public class MillerRabinTest
    {
        private MillerRabinPrimalityTester _primalityTester;
        private readonly int _confident = 40;

        [TestInitialize]
        public void SetUp()
        {
            _primalityTester = new MillerRabinPrimalityTester(_confident);
        }

        [TestMethod]
        public void When_PrimalityTestCalled_ShouldReturnCorrectAnswer()
        {
            _primalityTester.CheckPrimality(BigInteger.Parse("1298074214633706835075030044377087")).Should().BeTrue();
        }
    }
}

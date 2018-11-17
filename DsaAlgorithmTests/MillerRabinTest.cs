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
        private readonly int _confidence = 40;

        [TestInitialize]
        public void SetUp()
        {
            _primalityTester = new MillerRabinPrimalityTester(_confidence);
        }

        [TestMethod]
        public void When_PrimalityTestCalled_ShouldReturnCorrectAnswer()
        {
            _primalityTester.CheckPrimality(BigInteger.Parse("1298074214633706835075030044377087")).Should().BeTrue();
            _primalityTester.CheckPrimality(BigInteger.Parse("1298074214633706835075030044377089")).Should().BeFalse();
        }


    }
}

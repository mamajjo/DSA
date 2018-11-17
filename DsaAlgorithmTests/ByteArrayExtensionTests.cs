using System.Numerics;
using DSAAlgorythm.ExtensionMethods;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsaAlgorithmTests
{
    [TestClass]
    public class ByteArrayExtensionTests
    {
        [TestMethod]
        public void When_XorCalled_ShouldReturnCorrectValue()
        {
            byte[] a = new byte[] { 0b01011100, 0b10110011 };
            byte[] b = new byte[] { 0b10010011, 0b01010100 };
            a.Xor(b).Should().BeEquivalentTo(new byte[] {0b11001111, 0b11100111});
        }

        [TestMethod]
        public void When_CreatePositiveBigIntegerCalled_ShouldReturnCorrectValue()
        {
            byte[] a = new byte[] { 0b11011101 };
            a.CreatePositiveBigInteger().Should().BeEquivalentTo(new BigInteger(221));
        }
    }
}

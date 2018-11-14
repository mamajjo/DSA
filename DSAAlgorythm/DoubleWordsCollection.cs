using System;
using System.Collections.Generic;
using System.Linq;

namespace DSAAlgorythm
{

    //Contains a collection of double words (4-bytes) in LSB (least significant bit) order
    public class DoubleWordsCollection
    {
        private uint[] _doubleWords;

        //return a copy of digits
        public List<uint> GetDigits => _doubleWords.ToList();

        public const int BitsInDWord = 32;

        public DoubleWordsCollection(uint[] doubleWords)
        {
            int count = doubleWords.Length;
            while (count != 0 && doubleWords[count - 1] == 0) count--;
            _doubleWords = new uint[count];
            Array.Copy(doubleWords, _doubleWords, count);
        }

        public DoubleWordsCollection(DoubleWordsCollection copy)
        {
            _doubleWords = new uint[copy.DigitsCount];
            Array.Copy(copy._doubleWords, _doubleWords, copy.DigitsCount);
        }

        public uint this[int index] => _doubleWords[index];
        public int DigitsCount => _doubleWords.Length;

        public bool GetBit(int bitNumber)
        {
            if (_doubleWords == null) throw new ArgumentNullException();
            int dWordIndex = GetBitPosition(bitNumber, out int bitPosition);
            return (_doubleWords[dWordIndex] & (1 << bitPosition)) != 0;
        }

        //        public void SetBit(int bitNumber, bool bitValue)
        //        {
        //            if (_doubleWords == null) throw new ArgumentNullException();
        //            int dWordIndex = GetBitPosition(bitNumber, out int bitPosition);
        //            uint value = (uint)(bitValue ? 1 : 0);
        //            _doubleWords[dWordIndex] = _doubleWords[dWordIndex] | (value << bitPosition);
        //        }

        private int GetBitPosition(int bitNumber, out int bit)
        {
            int bitsCount = _doubleWords.Length * BitsInDWord;
            if (bitsCount <= bitNumber) throw new ArgumentException();
            bit = bitNumber % BitsInDWord;
            return bitNumber / bitsCount;
        }

        public int GetLeftmostActiveBitIndex()
        {
            uint lastDWord = _doubleWords[_doubleWords.Length - 1];
            int bits = (_doubleWords.Length - 1) * 32;

            for (int i = 0; i < BitsInDWord; i++)
            {
                uint custom = (uint)1 << (BitsInDWord - 1 - i);
                if ((lastDWord & custom) != 0) return bits + BitsInDWord - i - 1;
            }

            return bits;
        }
        //TODO Implementing function/property that returns the index of the most significant active (=1) bit
    }
}

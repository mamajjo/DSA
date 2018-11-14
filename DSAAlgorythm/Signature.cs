using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSAAlgorythm
{
    public class Signature
    {
        public BigInt R { get; set; }
        public BigInt S { get; set; }
        public Signature(BigInt r, BigInt s)
        {
            R = r;
            S = s;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML2_J.CSP.Comparers
{
    public class VariableCompareDomainSize : IComparer<Variable>
    {
        public int Compare(Variable x, Variable y)
        {
            return x.DomainSize.CompareTo(y.DomainSize);
        }
    }

    public class VariableCompareWordLen : IComparer<Variable>
    {
        public int Compare(Variable x, Variable y)
        {
            return x.Domain.Values[0].Length.CompareTo(y.Domain.Values[0].Length);
        }
    }

    public class WordCompareLen : IComparer<int[]>
    {
        public int Compare(int[] x, int[] y)
        {
            return x.Length.CompareTo(y.Length);
        }
    }
}

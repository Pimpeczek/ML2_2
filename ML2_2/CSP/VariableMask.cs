using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML2_J.CSP
{
    public class VariableMask
    {
        public bool[][] Masks;
        public int[] DomainSizes;

        public VariableMask(Variable[] variables)
        {
            DomainSizes = new int[variables.Length];
            Masks = new bool[variables.Length][];
            for (int v = 0; v < variables.Length; v++)
            {
                DomainSizes[v] = variables[v].DomainSize;
                Masks[v] = new bool[variables[v].DomainSize];
                for (int i = 0; i < variables[v].DomainSize; i++)
                {
                    Masks[v][i] = true;
                }
            }
        }

        public VariableMask(bool[][] masks, int[] domainSizes)
        {
            DomainSizes = new int[domainSizes.Length];
            Masks = new bool[masks.Length][];
            for (int v = 0; v < domainSizes.Length; v++)
            {
                DomainSizes[v] = domainSizes[v];
                Masks[v] = new bool[masks[v].Length];
                for(int i = 0; i < masks[v].Length; i++)
                {
                    Masks[v][i] = masks[v][i];
                }
            }
        }

        public bool MaskWord(int[] word, Variable[] variables, bool ignoreDirection)
        {
            Variable tVar;
            for (int v = 0; v < variables.Length; v++)
            {
                tVar = variables[v];
                bool flag;
                int pos;
                for (int i = 0; i < tVar.Domain.Values.Count; i++)
                {
                    pos = ignoreDirection ? 1 : 0;
                    flag = tVar.Domain.Values[i].Length == word.Length;
                    while (flag && pos < word.Length)
                    {
                        flag = tVar.Domain.Values[i][pos] == word[pos];
                        pos++;
                    }
                    if (flag)
                    {
                        Masks[v][i] = false;
                        DomainSizes[v]--;
                        if (!variables[v].Set && DomainSizes[v] == 0)
                            return false;
                    }
                }
            }
            return true;
        }

        public VariableMask Copy()
        {
            return new VariableMask(Masks, DomainSizes);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML2_J.CSP
{
    public class Domain
    {
        public List<int[]> Values { get; protected set; }

        public bool[] Mask { get; protected set; }
        public int MaskedSize { get; protected set; }
        protected bool maskClear;


        public Domain(List<int[]> values)
        {
            Values = values;
            Mask = new bool[Values.Count];
            for (int i = 0; i < Values.Count; i++)
            {
                Mask[i] = true;
            }
            maskClear = true;
            MaskedSize = Values.Count;
        }

        public void HideValueByPosition(int pos)
        {
            maskClear = false;
            Mask[pos] = false;
            MaskedSize--;
        }

        public void HideValueByValue(int[] val, bool ignoreDirection)
        {
            bool flag;
            int pos;
            for (int i = 0; i < Values.Count; i++)
            {
                pos = ignoreDirection ? 1 : 0;
                flag = Values[i].Length == val.Length;
                while (flag && pos < val.Length)
                {
                    flag = Values[i][pos] == val[pos];
                    pos++;
                }
                if(flag)
                {
                    Mask[i] = false;
                }
            }
        }

        public void ResetMask()
        {
            if (maskClear)
                return;
            for (int i = Mask.Length - 1; i >= 0; i--)
            {
                Mask[i] = true;
            }
            maskClear = true;
            MaskedSize = Values.Count;
        }

        public void RemoveValueByPosition(int pos)
        {
            Values.RemoveAt(pos);
            ResetMask();
        }

        public override string ToString()
        {

            if (Values.Count > 0)
            {
                string str = $"[{ValToStr(Values[0])}";
                for (int i = 1; i < Values.Count; i++)
                {
                    str += $" {ValToStr(Values[i])}";
                }
                return str + "]";
            }
            return "[]";
        }

        protected string ValToStr(int[] val)
        {
            string str = $"[{(val[0] == 1 ? 'v' : '>')}|";
            for (int i = 1; i < val.Length; i++)
            {
                str += $"{(char)val[i]}";
            }
            return str + ']';
        }

    }
}

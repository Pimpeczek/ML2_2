using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML2_J.CSP
{
    public interface ICSPProblem
    {
        bool CheckAll(int[][] state);

        bool IsFilled(int[][] state);

        Variable[] PrepareVariables();

        bool ShrinkDomains(int[] val, Variable[] variables);

        bool ShrinkDomain(int[] val, Variable variable);

        bool TryInsertValueToState(int[][] state, int[] value, int R, int C);

        Constraint[] PrepareConstraints();

        State PrepareState();
    }
}

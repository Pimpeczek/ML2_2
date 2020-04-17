using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ML2_J.CSP
{
    public class Solver
    {
        protected Variable[] variables;
        protected Constraint[] constraints;
        protected Stack<State> stateStack;
        protected ICSPProblem cspProblem;
        public State FinalState;
        protected int curVar;
        Stopwatch sw;
        public double PossibleCombinations { get; protected set; }
        public Solver(ICSPProblem problem)
        {
            cspProblem = problem;
            this.variables = problem.PrepareVariables();
            PossibleCombinations = variables[0].DomainSize;
            for (int i = 1; i < variables.Length; i++)
            {
                PossibleCombinations *= variables[i].DomainSize;
            }
            this.constraints = problem.PrepareConstraints();
            stateStack = new Stack<State>();
            stateStack.Push(problem.PrepareState());
            stateStack.Peek().VariableMask = new VariableMask(variables);
            curVar = -1;

        }

        public void Calculate()
        {
            sw = new Stopwatch();
            sw.Start();
            Try();
            stateStack.Clear();

        }

        protected bool Try()
        {
            curVar++;
            if (curVar >= variables.Length)
                return false;
            State newState;
            if (sw.ElapsedMilliseconds >= 131072)
                return false;
            variables[curVar].Set = true;
            for (int d = variables[curVar].DomainSize - 1; d >= 0; d--)
            {
                newState = stateStack.Peek().MakeClone();
                newState.VariableMask = stateStack.Peek().VariableMask.Copy();
                if (newState.VariableMask.Masks[curVar][d] && cspProblem.TryInsertValueToState(newState.Values, variables[curVar].Domain.Values[d], variables[curVar].R, variables[curVar].C))
                {
                    if (newState.VariableMask.MaskWord(variables[curVar].Domain.Values[d], variables, true) || curVar >= variables.Length-2)
                    {
                        stateStack.Push(newState);
                        if (CheckConstraints(newState))
                        {
                            if (curVar == variables.Length - 1)
                            {
                                FinalState = stateStack.Pop();
                                stateStack.Clear();
                                return true;
                            }
                            if (Try())
                            {
                                return true;
                            }
                            else
                            {
                                stateStack.Pop();
                            }
                            curVar--;
                        }
                        if (curVar < variables.Length - 1)
                        {
                            variables[curVar + 1].Domain.ResetMask();
                        }
                    }
                }

            }
            variables[curVar].Set = false;
            return false;
        }

        protected bool CheckConstraints(State state)
        {
            for (int c = 0; c < constraints.Length; c++)
            {
                if (!constraints[c].Check(state.Values))
                    return false;
            }
            return true;
        }
        public void ResetDomains()
        {
            for (int i = 0; i < variables.Length; i++)
            {
                variables[i].Domain.ResetMask();
            }
        }
        public void WriteState()
        {
            if (stateStack.Count > 0)
                Console.WriteLine($"{stateStack.Peek()}\n");
            else
                Console.WriteLine($"{FinalState}\n");
        }

    }
}

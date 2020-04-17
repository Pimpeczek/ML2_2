using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML2_J.CSP;

namespace ML2_J
{
    class Jolka : CSP.ICSPProblem
    {
        int[][] words;
        int[][] board;
        int variableCount;
        public Jolka(string boardLine, string wordsLine)
        {
            string[] split = wordsLine.ToUpper().Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            words = new int[split.Length][];
            for (int i = 0; i < split.Length; i++)
            {
                words[i] = new int[split[i].Length];
                for (int j = 0; j < words[i].Length; j++)
                {
                    words[i][j] = split[i][j];
                    
                }
                //Console.WriteLine(split[i]);
            }
            split = boardLine.ToUpper().Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            board = new int[split.Length][];
            for (int i = 0; i < split.Length; i++)
            {
                board[i] = new int[split[i].Length];
                for (int j = 0; j < board[i].Length; j++)
                {
                    board[i][j] = split[i][j] == '#' ? -1 : 0;
                }
            }
            int temp;
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == 0)
                    {
                        temp = GetDir(i, j);
                        variableCount += (temp + 1)/2;
                        board[i][j] = temp;

                    }
                }
            }
        }

        protected int GetDir(int r, int c)
        {
            int dir = 0;
            if (r == 0 && board[r + 1][c] != -1)
                dir+=1;

            if (c == 0)
                dir += 2;

            if (dir == 3)
                return dir;

            if (c < board[r].Length - 1 && c > 0 && board[r][c - 1] == -1 && board[r][c + 1] != -1)
                dir += 2;

            if (r < board.Length - 1 && r > 0 && board[r-1][c] == -1 && board[r + 1][c] != -1)
                dir += 1;

            return dir;
        }

        public bool CheckAll(int[][] state)
        {
            throw new NotImplementedException();
        }


        public bool IsFilled(int[][] state)
        {
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (state[i][j] == 0)
                        return false;
                }
            }
            return true;
        }

        public Constraint[] PrepareConstraints()
        {
            return new Constraint[0];
        }

        public State PrepareState()
        {
            int[][] newBoard = new int[board.Length][];
            for (int i = 0; i < board.Length; i++)
            {
                newBoard[i] = new int[board[i].Length];
                for (int j = 0; j < board[i].Length; j++)
                {
                    newBoard[i][j] = board[i][j] == -1 ? -1 : 0; 
                }
            }
            return new State(newBoard);
        }

        public Variable[] PrepareVariables()
        {
            Variable[] variables = new Variable[variableCount];
            int varPoint = 0;
            int dir;
            int curVarLen;
            int scanPos;
            List<int[]> domainValues;
            for (int r = 0; r < board.Length; r++)
            {
                for (int c = 0; c < board[r].Length; c++)
                {
                    if((dir = board[r][c]) > 0)
                    {
                        scanPos = 0;
                        if (dir % 2 == 1)
                        {
                            domainValues = new List<int[]>();
                            scanPos = 0;
                            while (scanPos + r< board.Length && board[scanPos + r][c] >= 0)
                                scanPos++;
                            for(int w = 0; w < words.Length; w++)
                            {
                                if(words[w].Length == scanPos)
                                {
                                    domainValues.Add(CreateDomainValue(words[w], 1));
                                }
                            }
                            variables[varPoint] = new Variable(r, c, new Domain(domainValues));
                            varPoint++;
                        }
                        if (dir >= 2)
                        {
                            domainValues = new List<int[]>();
                            scanPos = 0;
                            while (scanPos + c < board[r].Length && board[r][scanPos + c] >= 0)
                                scanPos++;
                            for (int w = 0; w < words.Length; w++)
                            {
                                if (words[w].Length == scanPos)
                                {
                                    domainValues.Add(CreateDomainValue(words[w], 2));
                                }
                            }
                            variables[varPoint] = new Variable(r, c, new Domain(domainValues));
                            varPoint++;
                        }
                    }
                }
            }
            return variables;
        }

        protected int[] CreateDomainValue(int[] word, int dir)
        {
            int[] value = new int[word.Length + 1];
            value[0] = dir;
            word.CopyTo(value, 1);
            return value;
        }

        public bool ShrinkDomains(int[] val, Variable[] variables)
        {
            for (int i = 0; i < variables.Length; i++)
                if (!ShrinkDomain(val, variables[i]))
                    return false;
            return true;
        }

        public bool TryInsertValueToState(int[][] state, int[] value, int R, int C)
        {
            int statePoint;
            
            if(value[0] == 1)
            {
                if (value.Length - 2  + R >= state.Length)
                {
                    return false;
                }
                for (int r = value.Length - 2; r >= 0; r--)
                {
                    statePoint = state[R + r][C];

                    if (statePoint != 0)
                    {
                        if (statePoint != value[r + 1])
                            return false;
                    }
                }
                for (int r = value.Length - 2; r >= 0; r--)
                {
                    state[R + r][C] = value[r + 1];
                }
            }
            else
            {
                if (value.Length - 2 + C >= state[R].Length)
                {
                    return false;
                }
                for (int c = value.Length - 2; c >= 0; c--)
                {
                    statePoint = state[R][c + C];

                    if (statePoint != 0)
                    {
                        if (statePoint != value[c + 1])
                            return false;
                    }
                }
                for (int c = value.Length - 2; c >= 0; c--)
                {
                    state[R][C + c] = value[c + 1];
                }
            }
            return true;
        }

        public static string BoardToString(int[][] board)
        {
            string boardString = "";
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    switch(board[i][j])
                    {
                        case -1:
                            boardString += "██";
                            break;
                        case 0:
                            boardString += "  ";
                            break;
                        case 1:
                            boardString += "v ";
                            break;
                        case 2:
                            boardString += "> ";
                            break;
                        case 3:
                            boardString += "x ";
                            break;
                        default:
                            boardString += $"{(char)(board[i][j])} ";
                            break;

                    }
                }
                if(i < board.Length-1)
                    boardString += "\n";
            }

            return boardString;
        }

        public override string ToString()
        {
            return BoardToString(board);
        }

        public bool ShrinkDomain(int[] val, Variable variable)
        {
            variable.Domain.HideValueByValue(val, true);
            return variable.Domain.MaskedSize <= 0;
        }
    }
}

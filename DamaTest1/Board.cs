using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamaTest1
{
    public enum Piece { Empty, BlackChecker, BlackKing, WhiteChecker, WhiteKing };

    public class Board
    {
        public Piece[,] B;
        public int[] x;
        public Board()
        {
            B = new Piece[8, 8];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    B[i, j] = Piece.BlackChecker;
                }
            }
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    B[i, j] = Piece.Empty;
                }
            }
            for (int i = 6; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    B[i, j] = Piece.WhiteChecker;
                }
            }
        }
        public Board(Board OtherBoard)
        {
            B = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    B[i, j] = OtherBoard.B[i, j];
                }
            }
        }
        public int countBlackPieces()
        {
            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (B[i, j] == Piece.BlackChecker || B[i, j] == Piece.BlackKing)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
        public int countWhitePieces()
        {
            int counter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (B[i, j] == Piece.WhiteChecker || B[i, j] == Piece.WhiteKing)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
        //winning if no {black / white} checkers are on the board
        public bool BlackWins()
        {
            if (countWhitePieces() == 0) return true;
            return false;
        }
        public bool WhiteWins()
        {
            if (countBlackPieces() == 0) return true;
            return false;
        }
        //returning a list of possible {Moves / Eats}
        public List<Board> GetPossibleBlackMoves()
        {
            List<Board> x = new List<Board>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (B[i, j] == Piece.BlackKing)
                    {
                        x.AddRange(BlackKingCanEat(i,j));
                    }
                    else if (B[i, j] == Piece.BlackChecker)
                    {
                        x.AddRange(BlackCheckerCanEat(i, j));
                    }
                }
            }
            //if cant eat , check if can move
            if (x.Count == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (B[i, j] == Piece.BlackKing)
                        {
                            x.AddRange(BlackKingCanMove(i, j));
                        }
                        else if (B[i, j] == Piece.BlackChecker)
                        {
                            x.AddRange(BlackCheckerCanMove(i, j));
                        }
                    }
                }
            }
            return x;
        }
        public List<Board> GetPossibleWhiteMoves()
        {
            List<Board> x = new List<Board>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (B[i, j] == Piece.WhiteKing)
                    {
                        x.AddRange(WhiteKingCanEat(i, j));
                    }
                    else if (B[i, j] == Piece.WhiteChecker)
                    {
                        x.AddRange(WhiteCheckerCanEat(i, j));
                    }
                }
            }
            if (x.Count == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (B[i, j] == Piece.WhiteKing)
                        {
                            x.AddRange(WhiteKingCanMove(i, j));
                        }
                        else if (B[i, j] == Piece.WhiteChecker)
                        {
                            x.AddRange(WhiteCheckerCanMove(i, j));
                        }
                    }
                }
            }
            return x;
        }
        public List<Board> BlackKingCanEat(int r, int c)
        {
            //r = current row , c = current column 
            List<Board> x = new List<Board>();
            //first eating down
            for (int i = r + 1; i < 7; i++)
            {
                if (B[i, c] != Piece.Empty)
                {
                    if ((B[i, c] == Piece.WhiteChecker || B[i, c] == Piece.WhiteKing) && B[i + 1, c] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[i, c] = Piece.Empty;
                        for (int k = i + 1; k < 8; k++)
                        {
                            if (a.B[k, c] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[k, c] = Piece.BlackKing;
                            List<Board> y = cd.BlackKingCanEat(k, c);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            for (int i = r - 1; i > 0; i--)
            {
                if (B[i, c] != Piece.Empty)
                {
                    if ((B[i, c] == Piece.WhiteChecker || B[i, c] == Piece.WhiteKing) && B[i - 1, c] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[i, c] = Piece.Empty;
                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (a.B[k, c] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[k, c] = Piece.BlackKing;
                            List<Board> y = cd.BlackKingCanEat(k, c);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            for (int j = c + 1; j < 7; j++)
            {
                if (B[r, j] != Piece.Empty)
                {
                    if ((B[r, j] == Piece.WhiteChecker || B[r, j] == Piece.WhiteKing) && B[r, j + 1] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[r, j] = Piece.Empty;
                        for (int k = j + 1; k <8; k++)
                        {
                            if (a.B[r, k] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[r , k] = Piece.BlackKing;
                            List<Board> y = cd.BlackKingCanEat(r, k);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            for (int j = c - 1; j > 0; j--)
            {
                if (B[r, j] != Piece.Empty)
                {
                    if ((B[r, j] == Piece.WhiteChecker || B[r, j] == Piece.WhiteKing) && B[r, j - 1] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[r, j] = Piece.Empty;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (a.B[r, k] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[r, k] = Piece.BlackKing;
                            List<Board> y = cd.BlackKingCanEat(r, k);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            return x;
        }
        public List<Board> BlackCheckerCanEat(int r, int c)
        {
            List<Board> x = new List<Board>();
            if (r < 6 && (B[r + 1, c] == Piece.WhiteChecker || B[r + 1, c] == Piece.WhiteKing) && B[r + 2, c] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r + 2, c] = Piece.BlackChecker;
                a.B[r + 1, c] = Piece.Empty;
                List<Board> y = a.BlackCheckerCanEat(r + 2, c);

                if (y.Count == 0)
                {
                    x.Add(a);
                }
                else
                {
                    x.AddRange(y);
                }
            }
            if (c < 6 && (B[r, c + 1] == Piece.WhiteChecker || B[r, c + 1] == Piece.WhiteKing) && B[r, c + 2] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c + 2] = Piece.BlackChecker;
                a.B[r, c + 1] = Piece.Empty;
                List<Board> y = a.BlackCheckerCanEat(r, c + 2);

                if (y.Count == 0)
                {
                    x.Add(a);
                }
                else
                {
                    x.AddRange(y);
                }
            }
            if (c > 1 && (B[r, c - 1] == Piece.WhiteChecker || B[r, c - 1] == Piece.WhiteKing) && B[r, c - 2] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c - 2] = Piece.BlackChecker;
                a.B[r, c - 1] = Piece.Empty;
                List<Board> y = a.BlackCheckerCanEat(r, c - 2);

                if (y.Count == 0)
                {
                    x.Add(a);
                }
                else
                {
                    x.AddRange(y);
                }
            }
            return x;
        }
        public List<Board> BlackKingCanMove(int r, int c)
        {
            List<Board> x = new List<Board>();
            for (int i = r + 1; i < 8; i++)
            {
                if (B[i, c] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[i, c] = Piece.BlackKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            for (int i = r - 1; i >= 0; i--)
            {
                if (B[i, c] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[i, c] = Piece.BlackKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            for (int j = c + 1; j < 8; j++)
            {
                if (B[r, j] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[r, j] = Piece.BlackKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            for (int j = c - 1; j >= 0; j--)
            {
                if (B[r, j] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[r, j] = Piece.BlackKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            return x;
        }
        public List<Board> BlackCheckerCanMove(int r, int c)
        {
            List<Board> x = new List<Board>();
            if (r < 7 && B[r + 1, c] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r + 1, c] = Piece.BlackChecker;
                x.Add(a);
            }
            if (c < 7 && B[r, c + 1] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c + 1] = Piece.BlackChecker;
                x.Add(a);
            }
            if (c > 0 && B[r, c - 1] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c - 1] = Piece.BlackChecker;
                x.Add(a);
            }
            return x;
        }
        public List<Board> WhiteKingCanEat(int r, int c)
        {
            List<Board> x = new List<Board>();
            for (int i = r + 1; i < 7; i++)
            {
                if (B[i, c] != Piece.Empty)
                {
                    if ((B[i, c] == Piece.BlackChecker || B[i, c] == Piece.BlackKing) && B[i + 1, c] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[i, c] = Piece.Empty;
                        for (int k = i + 1; k < 8; k++)
                        {
                            if (a.B[k, c] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[k, c] = Piece.WhiteKing;
                            List<Board> y = cd.WhiteKingCanEat(k, c);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            for (int i = r - 1; i > 0; i--)
            {
                if (B[i, c] != Piece.Empty)
                {
                    if ((B[i, c] == Piece.BlackChecker || B[i, c] == Piece.BlackKing) && B[i - 1, c] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[i, c] = Piece.Empty;
                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (a.B[k, c] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[k, c] = Piece.WhiteKing;
                            List<Board> y = cd.WhiteKingCanEat(k, c);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            for (int j = c + 1; j < 7; j++)
            {
                if (B[r, j] != Piece.Empty)
                {
                    if ((B[r, j] == Piece.BlackChecker || B[r, j] == Piece.BlackKing) && B[r, j + 1] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[r, j] = Piece.Empty;
                        for (int k = j + 1; k < 8; k++)
                        {
                            if (a.B[r, k] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[r, k] = Piece.WhiteKing;
                            List<Board> y = cd.WhiteKingCanEat(r, k);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            for (int j = c - 1; j > 0; j--)
            {
                if (B[r, j] != Piece.Empty)
                {
                    if ((B[r, j] == Piece.BlackChecker || B[r, j] == Piece.BlackKing) && B[r, j - 1] == Piece.Empty)
                    {
                        Board a = new Board(this);
                        a.B[r, c] = Piece.Empty;
                        a.B[r, j] = Piece.Empty;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (a.B[r, k] != Piece.Empty) break;
                            Board cd = new Board(a);
                            cd.B[r, k] = Piece.WhiteKing;
                            List<Board> y = cd.WhiteKingCanEat(r, k);
                            if (y.Count == 0)
                            {
                                x.Add(cd);
                            }
                            else
                            {
                                x.AddRange(y);
                            }
                        }
                    }
                    else break;
                }
            }
            return x;
        }
        public List<Board> WhiteCheckerCanEat(int r, int c)
        {
            List<Board> x = new List<Board>();
            if (r > 1 && (B[r - 1, c] == Piece.BlackChecker || B[r - 1, c] == Piece.BlackKing) && B[r - 2, c] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r - 2, c] = Piece.WhiteChecker;
                a.B[r - 1, c] = Piece.Empty;
                List<Board> y = a.WhiteCheckerCanEat(r - 2, c);

                if (y.Count == 0)
                {
                    x.Add(a);
                }
                else
                {
                    x.AddRange(y);
                }
            }
            if (c < 6 && (B[r, c + 1] == Piece.BlackChecker || B[r, c + 1] == Piece.BlackKing) && B[r, c + 2] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c + 2] = Piece.WhiteChecker;
                a.B[r, c + 1] = Piece.Empty;
                List<Board> y = a.WhiteCheckerCanEat(r, c + 2);

                if (y.Count == 0)
                {
                    x.Add(a);
                }
                else
                {
                    x.AddRange(y);
                }
            }
            if (c > 1 && (B[r, c - 1] == Piece.BlackChecker || B[r, c - 1] == Piece.BlackKing) && B[r, c - 2] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c - 2] = Piece.WhiteChecker;
                a.B[r, c - 1] = Piece.Empty;
                List<Board> y = a.WhiteCheckerCanEat(r, c - 2);

                if (y.Count == 0)
                {
                    x.Add(a);
                }
                else
                {
                    x.AddRange(y);
                }
            }
            return x;
        }
        public List<Board> WhiteKingCanMove(int r, int c)
        {
            List<Board> x = new List<Board>();
            for (int i = r + 1; i < 8; i++)
            {
                if (B[i, c] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[i, c] = Piece.WhiteKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            for (int i = r - 1; i >= 0; i--)
            {
                if (B[i, c] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[i, c] = Piece.WhiteKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            for (int j = c + 1; j < 8; j++)
            {
                if (B[r, j] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[r, j] = Piece.WhiteKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            for (int j = c - 1; j >= 0; j--)
            {
                if (B[r, j] == Piece.Empty)
                {
                    Board a = new Board(this);
                    a.B[r, c] = Piece.Empty;
                    a.B[r, j] = Piece.WhiteKing;
                    x.Add(a);
                }
                else
                {
                    break;
                }
            }
            return x;
        }
        public List<Board> WhiteCheckerCanMove(int r, int c)
        {
            List<Board> x = new List<Board>();
            if (r > 0 && B[r - 1, c] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r - 1, c] = Piece.WhiteChecker;
                x.Add(a);
            }
            if (c < 7 && B[r, c + 1] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c + 1] = Piece.WhiteChecker;
                x.Add(a);
            }
            if (c > 0 && B[r, c - 1] == Piece.Empty)
            {
                Board a = new Board(this);
                a.B[r, c] = Piece.Empty;
                a.B[r, c - 1] = Piece.WhiteChecker;
                x.Add(a);
            }
            return x;
        }
        // convert checkers at the extreme ends of the board to kings
        public void convertBlackCheckersToKings()
        {
            for (int i = 0; i < 8; i++)
            {
                if (B[7, i] == Piece.BlackChecker)
                {
                    B[7, i] = Piece.BlackKing;
                }
            }
        }
        public void convertWhiteCheckersToKings()
        {
            for (int i = 0; i < 8; i++)
            {
                if (B[0, i] == Piece.WhiteChecker)
                {
                    B[0, i] = Piece.WhiteKing;
                }
            }
        }
        //calculating feature values
        public void calculateX()
        {
            x = new int[Game.NumOfFeatures + 1];
            x[0] = 1;
            for (int i = 1; i < (Game.NumOfFeatures + 1); i++)
            {
                x[i] = 0;
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (B[i, j] == Piece.BlackChecker)
                    {
                        x[1]++;
                        if (BlackCheckerCanEat(i, j).Count > 0) x[5]++;
                    }
                    else if (B[i, j] == Piece.WhiteChecker)
                    {
                        x[2]++;
                        if (WhiteCheckerCanEat(i, j).Count > 0) x[6]++;
                    }
                    else if (B[i, j] == Piece.BlackKing)
                    {
                        x[3]++;
                        if (BlackKingCanEat(i, j).Count > 0) x[5]++;
                    }
                    else if (B[i, j] == Piece.WhiteKing)
                    {
                        x[4]++;
                        if (WhiteKingCanEat(i, j).Count > 0) x[6]++;
                    }
                }
            }
            x[7] = x[3] * x[3];
            x[8] = x[4] * x[4];
        }
        //predict current Features values
        public double getPredict(double[] theta)
        {
            double result = 0;
            for (int i = 0; i < theta.Length; i++)
            {
                result += theta[i] * x[i];
            }
            return result;
        }
        //return an array of features to be used in other place
        public double[] getFeatures()
        {
            double[] res = new double[Game.NumOfFeatures + 1];
            for (int i = 0; i < (Game.NumOfFeatures + 1); i++)
            {
                res[i] = x[i];
            }
            return res;
        }
        public override bool Equals(object obj)
        {
            Board b = (Board)obj;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (B[i, j] != b.B[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

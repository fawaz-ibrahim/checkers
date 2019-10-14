using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DamaTest1
{
    public class Game
    {
        public static Random r = new Random();
        public static double[] theta;
        public double[] theta_inverse;
        public static int NumOfFeatures = 8;
        public double lambda;
        public static List<double[]> thetasList;
        double alpha;
        int iter;
        int nGames;
        public Game(double alpha, double lambda, int iter, int nGames)
        {
            thetasList = new List<double[]>();
            theta = new double[NumOfFeatures + 1];
            theta_inverse = new double[NumOfFeatures + 1];
            this.alpha = alpha;
            this.iter = iter;
            this.nGames = nGames;
            this.lambda = lambda;
            for (int i = 0; i < (NumOfFeatures + 1); i++)
            {
                theta[i] = 0;
            }
            int black_win = 0;
            int white_win = 0;
            List<Board> takenMoves = new List<Board>();
            for (int i = 0; i < nGames; i++)
            {
                int Xscounter = 0;      // calculate game steps
                List<Board> player1;
                List<Board> player2;
                bool blackWin = false;
                bool whiteWin = false;
                double[,] Xs;
                double[] Ys;
                List<double[]> theXs = new List<double[]>();
                List<double> theYs = new List<double>();
                Board b = new Board();
                int round = r.Next(0, 2);   //0 for black , 1 for white
                while (true && !blackWin && !whiteWin && Xscounter < 5000)
                {
                    Xscounter++;
                    //just first round , select the player to start first
                    if (round != 1)
                    {
                        player1 = b.GetPossibleBlackMoves();
                        if (player1.Count == 0) //if player has no moves , loses
                        {
                            whiteWin = true;
                            break;
                        }
                        else
                        {
                            foreach (Board a in player1)    //for every possible move/eat of black
                            {
                                a.convertBlackCheckersToKings();
                                a.convertWhiteCheckersToKings();
                                a.calculateX();
                                if (a.BlackWins())  //if winning board , ofcorse must play
                                {
                                    blackWin = true;
                                    b = a;
                                    break;
                                }
                            }
                            if (!blackWin)
                            {
                                // sort moves ASC according to value
                                player1.Sort((node1, node2) => node2.getPredict(theta).CompareTo(node1.getPredict(theta)));
                                //if (player1.Count >= 2 && player1[0].getPredict(theta) != player1[1].getPredict(theta))
                                //{
                                //    MessageBox.Show(player1[0].getPredict(theta) + " > " + player1[1].getPredict(theta));
                                //}

                                // if i have moves with the same estimated benifit , chose on randomly
                                double opt = player1[0].getPredict(theta);
                                int counter = 1;
                                for (int l = 1; l < player1.Count; l++)
                                {
                                    if (player1[l].getPredict(theta) == opt)
                                    {
                                        counter++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                //counter is the number of boards that have the same predict
                                b = player1[r.Next(0, counter)];
                                //int movesTryCounter = 0;
                                //while (player1.Count > 1 && takenMoves.Contains(b) && movesTryCounter < counter)
                                //{
                                //    movesTryCounter++;
                                //    b = player1[r.Next(0, counter)];
                                //}
                                //if (movesTryCounter >= counter)
                                //{
                                //    b = player1[r.Next(0, player1.Count)];
                                //}
                                //if (takenMoves.Count == 100)
                                //{
                                //    takenMoves.RemoveAt(0);
                                //}
                                //takenMoves.Add(b);
                                
                            }
                            theXs.Add(b.getFeatures());
                            double JustAVariable = 0;
                            theYs.Add(JustAVariable);
                        }
                        //showboard("Black ", b);
                        if (blackWin || whiteWin) break;
                    }
                    round = 0;
                    player2 = b.GetPossibleWhiteMoves();
                    if (player2.Count == 0)
                    {
                        blackWin = true;
                        break;
                    }
                    else
                    {
                        foreach (Board a in player2)
                        {
                            a.convertBlackCheckersToKings();
                            a.convertWhiteCheckersToKings();
                            a.calculateX();
                            if (a.WhiteWins())
                            {
                                whiteWin = true;
                                b = a;
                                break;
                            }
                        }
                        if (!whiteWin)
                        {
                            // sort moves DESC according to value
                            player2.Sort((node1, node2) => node1.getPredict(theta).CompareTo(node2.getPredict(theta)));
                            //if (player2.Count >= 2 && player2[0].getPredict(theta) != player2[1].getPredict(theta))
                            //{
                            //    MessageBox.Show(player2[0].getPredict(theta) + " < " + player2[1].getPredict(theta));
                            //}
                            //player2.Sort((node1, node2) => node1.getPredict(theta_inverse).CompareTo(node2.getPredict(theta_inverse)));
                            double opt = player2[0].getPredict(theta);
                            //double opt = player2[0].getPredict(theta_inverse);
                            int counter = 1;
                            for (int l = 1; l < player2.Count; l++)
                            {
                                if (player2[l].getPredict(theta) == opt)
                                //if (player2[l].getPredict(theta_inverse) == opt)
                                {
                                    counter++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            b = player2[r.Next(0, counter)];
                            //int movesTryCounter = 0;
                            //while (player2.Count > 1 && takenMoves.Contains(b) && movesTryCounter < counter)
                            //{
                            //    movesTryCounter++;
                            //    b = player2[r.Next(0, counter)];
                            //}
                            //if (movesTryCounter >= counter)
                            //{
                            //    b = player2[r.Next(0, player2.Count)];
                            //}
                            //if (takenMoves.Count == 100)
                            //{
                            //    takenMoves.RemoveAt(0);
                            //}
                            //takenMoves.Add(b);
                        }
                        //theXs.Add(b.getFeatures());
                        //double JustAVariable = 0;
                        //theYs.Add(JustAVariable);
                    }
                    //showboard("White ", b);
                    if (blackWin || whiteWin) break;
                }
                double value = 0;
                if (blackWin)
                {
                    black_win++;
                    value = 100;
                }
                if (whiteWin)
                {
                    white_win++;
                    value = -100;
                }
                //if (Xscounter > 4998)
                //{
                //    showboard("tie", b);
                //}
                Xs = new double[theYs.Count, (NumOfFeatures + 1)];
                //filling the features array from the list , just to perform Linear Regression
                for (int fillXs = 0; fillXs < theYs.Count; fillXs++)
                {
                    for (int fillFeatures = 0; fillFeatures < (Game.NumOfFeatures + 1); fillFeatures++)
                    {
                        Xs[fillXs, fillFeatures] = theXs[fillXs][fillFeatures];
                    }
                }
                Ys = new double[theYs.Count];
                //winning board would have 100 , losing -100 , tie 0
                Ys[theYs.Count - 1] = value;
                b.calculateX();
                //estimate the winning board
                value = b.getPredict(theta);
                for (int fillingYs = theXs.Count - 2; fillingYs >= 0; fillingYs--)
                {
                    // Y[i] = predict(Features[i+1])
                    Ys[fillingYs] = value;
                    value = 0;
                    for (int c = 0; c < (Game.NumOfFeatures + 1); c++)
                    {
                        value += theta[c] * theXs[fillingYs][c];
                    }
                }

                Console.WriteLine(i + " : " + theYs.Count.ToString());
                string thetas = "";
                for (int asd = 0; asd < (Game.NumOfFeatures + 1); asd++)
                {
                    thetas += "x[" + asd + "] = " + theta[asd] + "\n";
                }
                Console.WriteLine(thetas);


                LinearRegression(Xs, Ys);
                double [] temp = new double[NumOfFeatures+1];
                theta.CopyTo(temp, 0);
                thetasList.Add(temp);
                //LMS(Xs, Ys);

                //{
                //    theta_inverse[0] = -theta[0];
                //    theta_inverse[1] = -theta[2];
                //    theta_inverse[2] = -theta[1];
                //    theta_inverse[3] = -theta[4];
                //    theta_inverse[4] = -theta[3];
                //    theta_inverse[5] = -theta[6];
                //    theta_inverse[6] = -theta[5];
                //    theta_inverse[7] = -theta[8];
                //    theta_inverse[8] = -theta[7];
                //}
            }
            String ss = "Result: Game\r\n";
            for (int j = 0; j < (Game.NumOfFeatures + 1); j++)
            {
                ss += "theta[" + j + "] = " + theta[j] + "\r\n";
            }
            ss += "\r\n\r\n Black : " + black_win + "\r\n White : " + white_win;
            MessageBox.Show(ss);
        }

        public Game(int x)
        {

        }

        public void showboard(String role, Board b)
        {
            String s = role + " Board : \r\n";
            for (int i1 = 0; i1 < 8; i1++)
            {
                for (int i2 = 0; i2 < 8; i2++)
                {
                    switch (b.B[i1, i2])
                    {
                        case Piece.Empty: s += "_\t";
                            break;
                        case Piece.BlackChecker: s += "b\t";
                            break;
                        case Piece.BlackKing: s += "A\t";
                            break;
                        case Piece.WhiteChecker: s += "w\t";
                            break;
                        case Piece.WhiteKing: s += "M\t";
                            break;
                    }
                }
                s += "\r\n\r\n";
            }
            MessageBox.Show(s);
        }

        public void LMS(double[,] Xs, double[] Ys)
        {
            for (int i = 0; i < Xs.GetLength(0); i++)
            {
                double Vb = 0;
                for (int j = 0; j < (NumOfFeatures + 1); j++)
                {
                    Vb += theta[j] * Xs[i, j];
                }
                double Vbtrain = Ys[i];
                for (int j = 0; j < (NumOfFeatures + 1); j++)
                {
                    theta[j] += alpha * (Vbtrain - Vb) * Xs[i, j];
                }
            }
        }

        public void LinearRegression(double[,] Xs, double[] Ys)
        {
            double[] h;
            for (int i = 0; i < iter; i++)
            {
                h = CalcH(Xs);
                //MessageBox.Show("h = " + h[0].ToString());
                double[,] G = grad(h, Ys, Xs);
                //MessageBox.Show("grad = " + G[0, 0].ToString());
                for (int j = 0; j < (Game.NumOfFeatures + 1); j++)
                {
                    theta[j] -= alpha * G[0, j];
                    //MessageBox.Show("theata [" + j +"]= " + theta[j].ToString());
                }
            }

        }

        public double[] CalcH(double[,] Xs)
        {
            double[] h = new double[Xs.GetLength(0)];
            for (int i = 0; i < Xs.GetLength(0); i++)
            {
                h[i] = 0;
                for (int j = 0; j < (Game.NumOfFeatures + 1); j++)
                {
                    h[i] += Xs[i, j] * theta[j];
                }
            }
            return h;
        }

        public double[,] MatrixMul(double[,] x, double[,] y)
        {
            int m = x.GetLength(0);
            int n = y.GetLength(1);
            int c = x.GetLength(1);
            double[,] temp = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    temp[i, j] = 0;
                    for (int k = 0; k < c; k++)
                    {
                        temp[i, j] += x[i, k] * y[k, j];
                    }
                }
            }
            return temp;
        }

        public double[,] grad(double[] h, double[] Ys, double[,] Xs)
        {
            double[,] delta = new double[1, Xs.GetLength(0)];
            for (int i = 0; i < Xs.GetLength(0); i++)
            {
                delta[0, i] = h[i] - Ys[i];
            }
            double[,] grad = MatrixMul(delta, Xs);
            for (int i = 0; i < (Game.NumOfFeatures + 1); i++)
            {
                grad[0, i] += (i==0)? 0 : lambda * theta[i];
                grad[0, i] /= Xs.GetLength(0);
            }
            return grad;
        }
    }
}

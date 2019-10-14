using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DamaTest1
{
    public partial class Form1 : Form
    {
        public enum Player { Black, White };
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game g = new Game(0.001 , 0 , 100 , 1000);
            theta = Game.theta;
            listBox1.Items.Clear();
            foreach (double[] d in Game.thetasList)
            {
                String s = "";
                for (int i = 0; i <= Game.NumOfFeatures; i++)
                {
                    s += d[i] + " , ";
                }
                listBox1.Items.Add(s);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backGround = draw_Stripes();
            Game.theta = new double[7];
            Game.theta[0] = 5.028;
            Game.theta[1] = 7.363;
            Game.theta[2] = -1.484;
            Game.theta[3] = 6.686;
            Game.theta[4] = -1.08299;
            Game.theta[5] = 0.7374;
            Game.theta[6] = -5.7;
            theta = Game.theta;
        }
        Board game;
        Player currPlayer;
        public double[] theta;
        private void button2_Click(object sender, EventArgs e)
        {
            game = new Board();
            images = new List<Image>();
            draw_all(game);
            save_image();
            if (Game.r.Next(0, 2) == 0)
            {
                currPlayer = Player.Black;
                timer1.Enabled = true;
            }
            else
            {
                currPlayer = Player.White;
            }
        }
        Bitmap backGround;
        public Bitmap draw_Stripes()
        {
            Bitmap b = new Bitmap(400, 400);

            bool wood = true;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Color c = new Color();
                    if (wood)
                    {
                        c = Color.FromArgb(255, 195, 0);
                    }
                    else
                    {
                        c = Color.FromArgb(238, 91, 0);
                    }
                    wood = !wood;
                    for (int h = (i * 50); h < ((i + 1) * 50); h++)
                    {
                        for (int w = (j * 50); w < ((j + 1) * 50); w++)
                        {
                            b.SetPixel(w, h, c);
                        }
                    }
                }
                wood = !wood;
            }
            return b;
        }
        public void draw_piece(int i , int j,Piece p , Bitmap b)
        {
            int w = j * 50 + 10;
            int h = i * 50 + 10;
            Graphics g = Graphics.FromImage(b);
            if (p == Piece.BlackChecker)
            {
                Pen pen = new Pen(Brushes.Black);
                g.DrawEllipse(pen, new Rectangle(w, h, 30, 30));
                g.FillEllipse(Brushes.Black, new Rectangle(w, h, 30, 30));
            }
            else if (p == Piece.BlackKing)
            {
                g.FillPolygon(Brushes.Black, new Point[] { new Point(w, h), new Point(w, h + 30), new Point(w + 15, h + 30)});
                g.FillPolygon(Brushes.Black, new Point[] { new Point(w, h + 30), new Point(w + 30, h + 30), new Point(w + 15, h) });
                g.FillPolygon(Brushes.Black, new Point[] { new Point(w + 15, h + 30), new Point(w + 30, h + 30), new Point(w + 30, h) });
            }
            else if (p == Piece.WhiteChecker)
            {
                Pen pen = new Pen(Brushes.White);
                g.DrawEllipse(pen, new Rectangle(w, h, 30, 30));
                g.FillEllipse(Brushes.White, new Rectangle(w, h, 30, 30));
            }
            else if (p == Piece.WhiteKing)
            {
                g.FillPolygon(Brushes.White, new Point[] { new Point(w, h), new Point(w, h + 30), new Point(w + 15, h + 30) });
                g.FillPolygon(Brushes.White, new Point[] { new Point(w, h + 30), new Point(w + 30, h + 30), new Point(w + 15, h) });
                g.FillPolygon(Brushes.White, new Point[] { new Point(w + 15, h + 30), new Point(w + 30, h + 30), new Point(w + 30, h) });
            }
            pictureBox1.Image = b;
        }
        public void draw_all(Board b)
        {
            Bitmap x = new Bitmap(backGround);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    draw_piece(i, j, b.B[i, j] , x);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (game.WhiteWins())
            {
                timer1.Enabled = false;
                MessageBox.Show("You Win!");
                timer1.Enabled = false;
            }
            else if (game.BlackWins())
            {
                timer1.Enabled = false;
                MessageBox.Show("You Lose!");
                timer1.Enabled = false;
            }
            else
            {
                List<Board> player;
                bool blackWin = false;
                player = game.GetPossibleBlackMoves();
                if (player.Count == 0)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You Win!");
                    timer1.Enabled = false;
                }
                else
                {
                    foreach (Board a in player)
                    {
                        a.convertBlackCheckersToKings();
                        a.convertWhiteCheckersToKings();
                        a.calculateX();
                        if (a.BlackWins())
                        {
                            game = a;
                            save_image();
                            draw_all(game);
                            blackWin = true;
                            timer1.Enabled = false;
                            MessageBox.Show("You Lose!");
                            timer1.Enabled = false;
                            break;
                        }
                    }
                    if (!blackWin)
                    {
                        player.Sort((node1, node2) => node2.getPredict(theta).CompareTo(node1.getPredict(theta)));
                        double opt = player[0].getPredict(theta);
                        int counter = 1;
                        for (int l = 1; l < player.Count; l++)
                        {
                            if (player[l].getPredict(theta) == opt)
                            {
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                            
                        }
                        //MessageBox.Show(counter.ToString());
                        game = player[Game.r.Next(0, counter)];
                        save_image();
                        draw_all(game);
                        currPlayer = Player.White;

                        player = game.GetPossibleWhiteMoves();
                        if (player.Count == 0)
                        {
                            currPlayer = Player.Black;
                            timer1.Enabled = false;
                            MessageBox.Show("You Lose!");
                            timer1.Enabled = false;
                        }
                    }
                }
            }
            timer1.Enabled = false;
        }

        int from_W = -1;
        int from_H = -1;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (currPlayer == Player.White)
            {
                int w = e.Location.X / 50;
                int h = e.Location.Y / 50;
                if (game.B[h, w] == Piece.WhiteChecker || game.B[h, w] == Piece.WhiteKing)
                {
                    from_H = h;
                    from_W = w;
                }
                else if (game.B[h, w] == Piece.Empty && from_W != -1 && from_H != -1)
                {
                    Board b = new Board(game);
                    b.B[from_H, from_W] = Piece.Empty;
                    b.B[h, w] = game.B[from_H, from_W];
                    List<Board> player = game.GetPossibleWhiteMoves();
                    if (player.Contains(b))
                    {
                        b.convertBlackCheckersToKings();
                        b.convertWhiteCheckersToKings();
                        game = b;
                        save_image();
                        draw_all(b);
                        currPlayer = Player.Black;
                        timer1.Enabled = true;
                    }
                    else
                    {
                        bool found = false;
                        if (game.B[from_H, from_W] == Piece.WhiteKing)
                        {
                            player = game.WhiteKingCanEat(from_H, from_W);
                        }
                        else if (game.B[from_H, from_W] == Piece.WhiteChecker)
                        {
                            player = game.WhiteCheckerCanEat(from_H, from_W);
                        }
                        foreach (Board x in player)
                        {
                            if (b.B[from_H, from_W] == x.B[from_H, from_W] && b.B[h, w] == x.B[h, w])
                            {
                                found = true;
                                game = x;
                                game.convertBlackCheckersToKings();
                                game.convertWhiteCheckersToKings();
                                save_image();
                                draw_all(game);
                                currPlayer = Player.Black;
                                timer1.Enabled = true;
                                break;
                            }
                        }
                        //foreach (Board x in player)
                        //{
                        //    if (b.B[from_H, from_W] == x.B[from_H, from_W] && b.B[h, w] == x.B[h, w])
                        //    {
                        //        found = true;
                        //        game = x;
                        //        break;
                        //    }
                        //}
                        if (!found)
                        {
                            MessageBox.Show("Not Allowed");
                        }
                    }
                    from_H = -1;
                    from_W = -1;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            draw_all(game);
            posss = null;
        }

        public List<Board> posss = null;
        int index = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            if (posss == null)
            {
                posss = game.GetPossibleWhiteMoves();
                index = 0;
                draw_all(posss[index++]);
            }
            else
            {
                if (index >= posss.Count)
                {
                    index = 0;
                }
                draw_all(posss[index++]);
            }
        }

        List<Image> images = new List<Image>();
        private void button5_Click(object sender, EventArgs e)
        {
            if (images_index > 0)
            {
                images_index--;
                pictureBox2.Image = images[images_index];
            }
            else
            {
                pictureBox2.Image = images[images_index];
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (images_index < images.Count - 1)
            {
                images_index++;
                pictureBox2.Image = images[images_index];
            }
            else
            {
                pictureBox2.Image = images[images_index];
            }
        }

        int images_index = 0;
        public void save_image()
        {
            images.Add(pictureBox1.Image);
            pictureBox2.Image = images[images.Count - 1];
            images_index = images.Count - 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                theta = Game.thetasList[listBox1.SelectedIndex];
            }
        }

        
    }
}

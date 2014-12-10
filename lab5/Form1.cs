using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form1 : Form
    {
        //dimensions
        private const float clientSize = 100;
        private const float lineLength = 80;
        private const float block = lineLength / 3;
        private const float offset = 10;
        private const float delta = 5;
        private enum CellSelection { N, O, X };
        private CellSelection[,] grid = new CellSelection[3, 3];
        private float scale;    //current scale factor
        private bool computerGoesNow = false;
        private bool humanTurn = true; //true by default
        private int i_X; //row from human turn
        private int j_X; //column from human turn
        private int i_O; //row from gameEngine 
        private int j_O; //column from gameEngine
        private int[] computerCoords;

        //create an instance of gameEngine class
        lab5.gameEngine gameEngine = new gameEngine();

        public Form1()
        {
            InitializeComponent();
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ApplyTransform(g);

            //draw board
            g.DrawLine(Pens.Black, block, 0, block, lineLength);
            g.DrawLine(Pens.Black, 2 * block, 0, 2 * block, lineLength);
            g.DrawLine(Pens.Black, 0, block, lineLength, block);
            g.DrawLine(Pens.Black, 0, 2 * block, lineLength, 2 * block);

            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    if (grid[i, j] == CellSelection.O) DrawO(i, j, g);
                    else if (grid[i, j] == CellSelection.X) DrawX(i, j, g);

            if (computerGoesNow)
            {
                computerTurn();
                checkWinner();
            }
        }

        private void ApplyTransform(Graphics g)
        {
            scale = Math.Min(ClientRectangle.Width / clientSize,
                ClientRectangle.Height / clientSize);
            if (scale == 0f) return;
            g.ScaleTransform(scale, scale);
            g.TranslateTransform(offset, offset);
        }

        //draw X
        private void DrawX(int i, int j, Graphics g)
        {
            g.DrawLine(Pens.Black, i * block + delta, j * block + delta,
                (i * block) + block - delta, (j * block) + block - delta);
            g.DrawLine(Pens.Black, (i * block) + block - delta,
                j * block + delta, (i * block) + delta, (j * block) + block - delta);

        }

        //draw an O
        private void DrawO(int i, int j, Graphics g)
        {

            g.DrawEllipse(Pens.Black, i * block + delta, j * block + delta,
                block - 2 * delta, block - 2 * delta);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            ApplyTransform(g);
            PointF[] p = { new Point(e.X, e.Y) };
            g.TransformPoints(CoordinateSpace.World,
                CoordinateSpace.Device, p);

            if (p[0].X < 0 || p[0].Y < 0) return;
            i_X = (int)(p[0].X / block);
            j_X = (int)(p[0].Y / block);
            if (i_X > 2 || j_X > 2) return;

            //only allow setting empty cells 
            // if ((grid[i_X, j_X] == CellSelection.N) || (grid[i_O, j_O] == CellSelection.N))
            //we want the game engine to validate each move so dont use above statement
            if (e.Button == MouseButtons.Left)
            {
                if (humanTurn)
                {
                    computerStartsToolStripMenuItem.Enabled = false; //disable after first human move
                    gameEngine.setPoints(i_X, j_X);
                    if (gameEngine.validateMove() == true)
                    {
                        string message = "Place X somewhere else";
                        string caption = "Invalid move!";
                        DialogResult result;

                        // Displays the MessageBox.
                        result = MessageBox.Show(message, caption);
                    }

                    else
                    {
                        grid[i_X, j_X] = CellSelection.X;
                        computerGoesNow = true;
                    }
                }
            }
            Invalidate();
        }

        //exit application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            Invalidate();
        }

        private void computerStartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            computerTurn(); //computer goes first
            computerStartsToolStripMenuItem.Enabled = false;
            Invalidate();
        }

        private void computerTurn()
        {
            computerCoords = gameEngine.makeMove();
            i_O = computerCoords[0];
            j_O = computerCoords[1];
            grid[i_O, j_O] = CellSelection.O;
            computerGoesNow = false;
            humanTurn = true;
            Invalidate();
        }

        private void checkWinner()
        {
            if (gameEngine.detectWinner() == 1)
            {
                string message = "Player X wins!";
                string caption = "We have a winner";
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption);
                humanTurn = false;
                computerGoesNow = false;
            }

            else if (gameEngine.detectWinner() == 2)
            {
                string message = "Computer wins!";
                string caption = "We have a winner";
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption);
                humanTurn = false;
                computerGoesNow = false;
            }

            else if (gameEngine.detectWinner() == 3)
            {
                string message = "It's a tie!";
                string caption = "No one wins. Try a new game";
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption);
                humanTurn = false;
                computerGoesNow = false;
            }
        }

        private void Reset()
        {
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    grid[i, j] = CellSelection.N;
            gameEngine.setPoints(-1, -1);
            computerStartsToolStripMenuItem.Enabled = true;
            computerGoesNow = false;
            humanTurn = true;
            Invalidate();
        }
    }
}

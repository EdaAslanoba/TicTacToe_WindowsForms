using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    class gameEngine
    {
        private int[] humanCoords;
        private int[,] gameBoard = new int[3, 3];
        private bool corner = false;
        private bool edge = false;
        private bool center = false;
        private int[] computer_move; // = new int[2];
        private int emptyCells = 9; //9 available cells at start
        private int winner;
        private bool invalidMove = false;

        private bool computerMoved = false;
        private int[] countX_column;
        private int[] countX_row;
        private int[] countO_column;
        private int[] countO_row;

        private int countX_diagonal1 = 0;
        private int countX_diagonal2 = 0;
        private int countO_diagonal1 = 0;
        private int countO_diagonal2 = 0;

        //game engine constructor
        public gameEngine()
        {
        }

        public void setPoints(int x, int y)
        {
            if ((x == -1) && (y == -1))
            {
                //reset everything
                emptyCells = 9;
                winner = 0;
                gameBoard = new int[3, 3];
                corner = false;
                center = false;
                edge = false;
            }

            else
            {
                //reset invalid move flag
                invalidMove = false;
                humanCoords = new int[] { x, y };
                if ((gameBoard[x, y] == 1) || (gameBoard[x, y] == 2)) //if there is an X or O placed there already, ignore the move and alert the player
                {
                    //send error message
                    invalidMove = true;
                }

                //human made a valid move
                else emptyCells--;
            }
        }

        public bool validateMove()
        {
            if (invalidMove) return true;

            else return false;
        }

        //computer makes an intelligent move
        //detect if there is a blocking move / winning move
        public int[] makeMove()
        {
            //computer starts at bottom left corner
            if (emptyCells == 9)
            {
                gameBoard[0, 2] = 2;
                computer_move = new int[] { 0, 2 };
                emptyCells--;
            }

            else //human starts (by default)
            {
                int i_human = humanCoords[0];
                int j_human = humanCoords[1];
                gameBoard[i_human, j_human] = 1; // a value of 1 indicates there is an X there

                if ((emptyCells == 8) || (emptyCells == 7))
                {
                    //check if X is placed on a corner cell - (0,0) (0,2) (2,0) (2,2)
                    if ((gameBoard[0, 0] == 1) || (gameBoard[2, 0] == 1) || (gameBoard[0, 2] == 1) || (gameBoard[2, 2] == 1))
                    {
                        corner = true;
                    }

                    //if center
                    else if (gameBoard[1, 1] == 1)
                    {
                        center = true;
                    }

                    else //if edge
                    {
                        edge = true;
                    }

                    //making the first intelligent move
                    if (corner)
                    {
                        //place an O at the center
                        gameBoard[1, 1] = 2;
                        computer_move = new int[] { 1, 1 };
                        emptyCells--;
                    }

                    else if (center)
                    {
                        //place an O at a corner
                        gameBoard[0, 0] = 2;
                        computer_move = new int[] { 0, 0 };
                        emptyCells--;
                    }

                    else if (edge)//if edge
                    {
                        if (gameBoard[1, 0] == 1)
                        {
                            gameBoard[1, 2] = 2;
                            computer_move = new int[] { 1, 2 };
                            emptyCells--;
                        }

                        else if (gameBoard[0, 1] == 1)
                        {
                            gameBoard[2, 1] = 2;
                            computer_move = new int[] { 2, 1 };
                            emptyCells--;
                        }

                        else if (gameBoard[2, 1] == 1)
                        {
                            gameBoard[0, 1] = 2;
                            computer_move = new int[] { 0, 1 };
                            emptyCells--;
                        }

                        else if (gameBoard[1, 2] == 1)
                        {
                            gameBoard[1, 0] = 2;
                            computer_move = new int[] { 1, 0 };
                            emptyCells--;
                        }
                    }
                } //8

                else if (emptyCells <= 6)
                {
                    countX_column = new int[] { 0, 0, 0 };
                    countO_column = new int[] { 0, 0, 0 };
                    countX_row = new int[] { 0, 0, 0 };
                    countO_row = new int[] { 0, 0, 0 };

                    for (int i = 0; i < 3; ++i) //column
                    {
                        for (int j = 0; j < 3; ++j) //row
                        {
                            if (gameBoard[i, j] == 1)
                            {
                                countX_column[i]++;
                                countX_row[j]++;
                            }

                            else if (gameBoard[i, j] == 2)
                            {
                                countO_column[i]++;
                                countO_row[j]++;
                            }
                        }

                        if (gameBoard[i, i] == 1)
                        {
                            countX_diagonal1++;
                        }

                        else if (gameBoard[i, i] == 2)
                        {
                            countO_diagonal1++;
                        }
                    }

                    int a = 2;
                    while (a >= 0)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            if (gameBoard[a, j] == 1)
                            {
                                countX_diagonal2++;
                            }

                            else if (gameBoard[a, j] == 2)
                            {
                                countO_diagonal2++;
                            }
                            --a;
                        }
                    }

                    /*WINNING MOVE*/
                    //check for number of Xs and Os
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            //we have 2 O's in a row - make a winning move
                            if ((countO_row[i] == 2) && (gameBoard[j, i] != 2) && (gameBoard[j, i] != 1) && (!computerMoved))
                            {
                                gameBoard[j, i] = 2;
                                computer_move = new int[] { j, i };
                                computerMoved = true;
                                countO_row[i]++; //we have a 3rd O placed - end game condition
                            }

                            //we have 2 O's in a column - make a winning move
                            else if ((countO_column[i] == 2) && (gameBoard[i, j] != 2) && (gameBoard[i, j] != 1) && (!computerMoved))
                            {
                                gameBoard[i, j] = 2;
                                computer_move = new int[] { i, j };
                                computerMoved = true;
                                countO_column[i]++; //we have a 3rd O placed - end game condition
                            }
                        }
                    }

                    //check diagonal win - algorithm was messed up - hardcode diagonal positions
                    //if ((countO_diagonal1 == 2) || (countO_diagonal2 == 2))
                    //{
                    if (!computerMoved) //winning move not detected
                    {
                        if ((gameBoard[0, 0] == 2) && (gameBoard[1, 1] == 2) && (gameBoard[2, 2] != 1) && (!computerMoved))
                        {
                            gameBoard[2, 2] = 2;
                            computer_move = new int[] { 2, 2 };
                            computerMoved = true;
                            countO_diagonal1++;
                        }

                        else if ((gameBoard[1, 1] == 2) && (gameBoard[2, 2] == 2) && (gameBoard[0, 0] != 1) && (!computerMoved))
                        {
                            gameBoard[0, 0] = 2;
                            computer_move = new int[] { 0, 0 };
                            computerMoved = true;
                            countO_diagonal1++;
                        }

                        else if ((gameBoard[0, 0] == 2) && (gameBoard[2, 2] == 2) && (gameBoard[1, 1] != 1) && (!computerMoved))
                        {
                            gameBoard[1, 1] = 2;
                            computer_move = new int[] { 1, 1 };
                            computerMoved = true;
                            countO_diagonal1++;
                        }

                        else if ((gameBoard[2, 0] == 2) && (gameBoard[1, 1] == 2) && (gameBoard[0, 2] != 1) && (!computerMoved))
                        {
                            gameBoard[0, 2] = 2;
                            computer_move = new int[] { 0, 2 };
                            computerMoved = true;
                            countO_diagonal2++;
                        }

                        else if ((gameBoard[1, 1] == 2) && (gameBoard[0, 2] == 2) && (gameBoard[2, 0] != 1) && (!computerMoved))
                        {
                            gameBoard[2, 0] = 2;
                            computer_move = new int[] { 2, 0 };
                            computerMoved = true;
                            countO_diagonal2++;
                        }

                        else if ((gameBoard[0, 2] == 2) && (gameBoard[2, 0] == 2) && (gameBoard[1, 1] != 1) && (!computerMoved))
                        {
                            gameBoard[1, 1] = 2;
                            computer_move = new int[] { 1, 1 };
                            computerMoved = true;
                            countO_diagonal2++;
                        }
                    }

                    if (!computerMoved) //winning move not detected 
                    {
                        /*BLOCKING MOVE*/
                        for (int i = 0; i < 3; ++i)
                        {
                            for (int j = 0; j < 3; ++j)
                            {
                                //we have 2 X's in a row - make a blocking move
                                if ((countX_row[i] == 2) && (gameBoard[j, i] != 1) && (gameBoard[j, i] != 2) && (!computerMoved))
                                {
                                    gameBoard[j, i] = 2;
                                    computer_move = new int[] { j, i };
                                    computerMoved = true;
                                }

                                //we have 2 X's in a column - make a blocking move
                                else if ((countX_column[i] == 2) && (gameBoard[i, j] != 1) && (gameBoard[i, j] != 2) && (!computerMoved))
                                {
                                    gameBoard[i, j] = 2;
                                    computer_move = new int[] { i, j };
                                    computerMoved = true;
                                }
                            }
                        }
                        //check diagonal blocking move
                        if (!computerMoved)
                        {
                            if ((gameBoard[0, 0] == 1) && (gameBoard[1, 1] == 1) && (gameBoard[2, 2] != 2) && (!computerMoved))
                            {
                                gameBoard[2, 2] = 2;
                                computer_move = new int[] { 2, 2 };
                                computerMoved = true;
                            }

                            else if ((gameBoard[1, 1] == 1) && (gameBoard[2, 2] == 1) && (gameBoard[0, 0] != 2) && (!computerMoved))
                            {
                                gameBoard[0, 0] = 2;
                                computer_move = new int[] { 0, 0 };
                                computerMoved = true;
                            }

                            else if ((gameBoard[0, 0] == 1) && (gameBoard[2, 2] == 1) && (gameBoard[1, 1] != 2) && (!computerMoved))
                            {
                                gameBoard[1, 1] = 2;
                                computer_move = new int[] { 1, 1 };
                                computerMoved = true;
                            }

                            else if ((gameBoard[2, 0] == 1) && (gameBoard[1, 1] == 1) && (gameBoard[0, 2] != 2) && (!computerMoved))
                            {
                                gameBoard[0, 2] = 2;
                                computer_move = new int[] { 0, 2 };
                                computerMoved = true;
                            }

                            else if ((gameBoard[1, 1] == 1) && (gameBoard[0, 2] == 1) && (gameBoard[2, 0] != 2) && (!computerMoved))
                            {
                                gameBoard[2, 0] = 2;
                                computer_move = new int[] { 2, 0 };
                                computerMoved = true;
                            }

                            else if ((gameBoard[0, 2] == 1) && (gameBoard[2, 0] == 1) && (gameBoard[1, 1] != 2) && (!computerMoved))
                            {
                                gameBoard[1, 1] = 2;
                                computer_move = new int[] { 1, 1 };
                                computerMoved = true;
                            }
                        }

                    }

                    if (!computerMoved) //no blocking or winning move detected. make a legal move - first space available
                    {
                        for (int ii = 0; ii < 3; ++ii)
                        {
                            for (int jj = 0; jj < 3; ++jj)
                            {
                                //find the first available legal move and place an O there
                                if ((gameBoard[ii, jj] != 1) && (gameBoard[ii, jj] != 2) && (!computerMoved))
                                {
                                    gameBoard[ii, jj] = 2;
                                    computer_move = new int[] { ii, jj };
                                    computerMoved = true;
                                }
                            }
                        }
                    }

                    if (computerMoved)
                    {
                        emptyCells--; //finally made a move
                        computerMoved = false;
                    }
                } //6-4-2
            }
            return computer_move;
        }

        //detect if there is a winner or a tie
        public int detectWinner()
        {
            //X wins
            if (((gameBoard[0, 0] == 1) && (gameBoard[1, 0] == 1) && (gameBoard[2, 0] == 1)) ||
                ((gameBoard[0, 1] == 1) && (gameBoard[1, 1] == 1) && (gameBoard[2, 1] == 1)) ||
                ((gameBoard[0, 2] == 1) && (gameBoard[1, 2] == 1) && (gameBoard[2, 2] == 1)) ||
                ((gameBoard[0, 0] == 1) && (gameBoard[0, 1] == 1) && (gameBoard[0, 2] == 1)) ||
                ((gameBoard[1, 0] == 1) && (gameBoard[1, 1] == 1) && (gameBoard[1, 2] == 1)) ||
                ((gameBoard[2, 0] == 1) && (gameBoard[2, 1] == 1) && (gameBoard[2, 2] == 1)) ||
                ((gameBoard[0, 0] == 1) && (gameBoard[1, 1] == 1) && (gameBoard[2, 2] == 1)) ||
                ((gameBoard[2, 0] == 1) && (gameBoard[1, 1] == 1) && (gameBoard[0, 2] == 1)))
            {
                winner = 1;
            }

            else if //O wins
                (((gameBoard[0, 0] == 2) && (gameBoard[1, 0] == 2) && (gameBoard[2, 0] == 2)) ||
                ((gameBoard[0, 1] == 2) && (gameBoard[1, 1] == 2) && (gameBoard[2, 1] == 2)) ||
                ((gameBoard[0, 2] == 2) && (gameBoard[1, 2] == 2) && (gameBoard[2, 2] == 2)) ||
                ((gameBoard[0, 0] == 2) && (gameBoard[0, 1] == 2) && (gameBoard[0, 2] == 2)) ||
                ((gameBoard[1, 0] == 2) && (gameBoard[1, 1] == 2) && (gameBoard[1, 2] == 2)) ||
                ((gameBoard[2, 0] == 2) && (gameBoard[2, 1] == 2) && (gameBoard[2, 2] == 2)) ||
                ((gameBoard[0, 0] == 2) && (gameBoard[1, 1] == 2) && (gameBoard[2, 2] == 2)) ||
                ((gameBoard[2, 0] == 2) && (gameBoard[1, 1] == 2) && (gameBoard[0, 2] == 2)))
            {
                winner = 2;
            }

            else //tie
            {
                if (emptyCells == 0)
                    winner = 3;
            }
            return winner;
        }
    }
}




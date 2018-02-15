using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSudoku
{
    class Game
    {
        // The game board.
        Board gameBoard;
        // Temporary variable used for creating the game board.
        Board testBoard;

        Game()
        {
            gameBoard = new Board(); testBoard = new Board();
            CreateBoard();
        }

        private void CreateBoard()
        {
            // Place the first 9 numbers onto the board.
            for (int i = 0; i < 9; i++)
            {
                int[] cell = gameBoard.RandomRemainingCell;
                gameBoard.PlaceAt(i, cell); testBoard.PlaceAt(i, cell);
            }

            // Place about 18 more random 

            // Fill in the remaining cells using tricks iteratively.
            while (!testBoard.IsFull)
            {
                Board board = testBoard;

                // Apply tricks to fill board as much as possible.
                FillLoneOption(testBoard); FillAxes(testBoard);

                // If no more tricks can be applied, place a random int on both boards.
                if (board == testBoard)
                {
                    int[] cell = testBoard.RandomRemainingCell;
                    int value = testBoard.RandomValue(cell);

                    gameBoard.PlaceAt(value, cell); testBoard.PlaceAt(value, cell);
                }
            }
        }

        private void FillLoneOption(Board testBoard)
        {
            throw new NotImplementedException();
        }

        private void FillAxes(Board testBoard)
        {
            throw new NotImplementedException();
        }

        private int RandomValue => (new Random()).Next(9);

    }
}

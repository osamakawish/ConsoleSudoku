using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSudoku
{
    class GameBoard
    {
        // The game board of integers.
        private int[,] board;
        // IMPLEMENT LATER: Stores noted integers. Which the user can add ass possible options.
        //private int[,][] notes;
        // Index n-1: Determines how many times n has been used on board.
        private int[] count;
        // The set of unused coordinats.
        private HashSet<int[]> remainingCoords;

        GameBoard()
        {
            // Fill up remainingCoords variable to include every coordinate.
            AllCoords();
            // Create the game board.
            CreateBoard();
        }

        private void AllCoords()
        {
            for(int x=0; x<9 ; x++)
            {
                for(int y=0; y<9; x++)
                {
                    remainingCoords.Add(new int[2] { x, y });
                }
            }
        }

        private void CreateBoard()
        {
            // The 9x9 game board. Initially empty.
            board = new int[9, 9];
            // The lcoation variable: to iterate over the entire board. 
            int[] location = { 1, 1 };
            // Check if game is solvable from here.
            bool solvable = IsSolvable();
            // Add appropriate integer between 1 and 9 over every cell until game is completable.
            while (location[0] < 9 && location[1] < 9)
            {
                // Gets the index in array notaion at at the provided location.
                int[] coordinates = GetIndices(location);
                // Check if game is solvable from here.
                solvable = IsSolvable();
                // If game is solvable, break loop.
                if (solvable) { break; }
                // Increment location to a new random untaken one.
                location[0]++; location[1]++;
            }
        }

        public int[] GetIndices(int[] location) {
            return new int[2] { location[0] - 1, location[1] - 1};
        }

        private bool IsSolvable()
        {
            // Get maximum coordinates of the game board.
            int Xmax = board.GetLength(0);
            int Ymax = board.GetLength(1);

            // Check for solvability.


            return true;
        }
    }
}

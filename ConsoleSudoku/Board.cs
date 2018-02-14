using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSudoku
{
    class Board
    {
        public enum Axis { row, col, box };

        // Initiate the board (to be presented to player) and testboard (for algorithmic efficiency).
        int[,] board = new int[9, 9];
        // IMPLEMENT LATER: Stores noted integers. Which the user can add ass possible options.
        //private int[,][] notes;
        // Index n-1: Determines how many times n has been used on board.
        private int[] count;
        // The set of unused coordinats.
        private HashSet<int[]> remainingCoords;
        // Stores the locations of all integers.
        private bool[,,] locations;

        Board()
        {
            // Initiate count variable.
            count = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // Everything is stored regarding integer n+1 in index n. 
            bool[,,] A = new bool[9, 3, 9]; // A[7, 2, 4] = true : 8 (=7+1) is contained in box (Axis 2) 5 (=4+1).
            // Fill up remainingCoords variable to include every coordinate. 
            AddCoords();
        }

        private void AddCoords()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; x++)
                {
                    // Place 0 at the cell.
                    PlaceAt(0, new int[2] { x, y });
                    // Add each possible coordinates as a remaining one.
                    remainingCoords.Add(new int[] { x, y });
                }
            }
        }

        // Get array index based on board x,y coordinates.
        public static int[] GetIndices(int[] location)
        {
            return new int[2] { location[0]--, location[1]-- };
        }

        // Get board x,y coordinates based on array indices.
        public static int[] GetLocation(int[] indices)
        {
            return new int[2] { indices[0]++, indices[1]++ };
        }

        // Implements the locations variable more intuitively.
        private void AddLocation(int i, Axis axis, int axisNumber)
        {
            locations[i, (int)axis, axisNumber - 1] = true;
        }

        // Provide box number given coordinates of box.
        private int Box(int[] cell)
        {
            return 3 * cell[1] + cell[0];
        }

        public int GetCount(int i)
        {
            return count[i - 1];
        }

        private void PlaceAt(int i, int[] cell)
        {
            if (remainingCoords.Contains(cell)) { remainingCoords.Remove(cell); }

            // Place it in the board.
            board[cell[0], cell[1]] = i;
            // Update locations variable accordingly.
            AddLocation(i, Axis.row, cell[0]);
            AddLocation(i, Axis.col, cell[1]);
            AddLocation(i, Axis.box, Box(cell));
            // Update count variable.
            count[i - 1]++;
        }
    }
}

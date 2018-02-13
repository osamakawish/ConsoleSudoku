using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSudoku
{
    public enum Axis { row, col, box };

    class GameBoard
    {
        // The game board of integers, for providing to be played.
        private int[,] board;
        // The test board, to attempt and provide solutions to the game board.
        private int[,] testBoard;
        // IMPLEMENT LATER: Stores noted integers. Which the user can add ass possible options.
        //private int[,][] notes;
        // Index n-1: Determines how many times n has been used on board.
        private int[] count;
        // The set of unused coordinats.
        private HashSet<int[]> remainingCoords;
        // Stores the locations of all integers.
        private bool[,,] locations;

        GameBoard()
        {
            // Initiate the board (to be presented to player) and testboard (for algorithmic efficiency).
            board = new int[9, 9]; testBoard = new int[9, 9];
            // Everything is stored regarding integer n+1 in index n. 
            bool[,,] A = new bool[9, 9, 3]; // A[7, 2, 3] = true : 8 (=7+1) is contained in box (Axis 2) 4 (=3+1).
            // Fill up remainingCoords variable to include every coordinate. 
            AddCoords(); 
            // Create the game board.
            CreateBoard();
        } 

        private void AddCoords()
        {
            for(int x=0; x<9; x++)
            {
                for(int y=0; y<9; x++)
                {
                    // Add each possible coordinates as a remaining one.
                    remainingCoords.Add(new int[] { x, y });
                    // Declare each possible input as 0 to represent no number is placed here.
                    board[x, y] = 0; testBoard[x, y] = 0;
                }
            }
        }

        private void CreateBoard()
        {
            // The location variable: to iterate over the entire board. 
            int[] location;
            // Get a random unused location.
            location = GetRandomRemainingLocation();

            // Add appropriate integer between 1 and 9 over every cell until game is completable.
            while (IsSolvable() && location[0] < 9 && location[1] < 9)
            {
                // Gets the index in array notaion at at the provided location.
                int[] coordinates = GetIndices(location);
                // Increment location to a new random untaken one.
                location = GetRandomRemainingLocation();
            }
        }

        // Gets a random location among the remaining ones.
        private int[] GetRandomRemainingLocation()
        {
            // Initiate randomizer.
            Random random = new Random();
            // Convert remainingCoords:array for random indexing.
            int[][] toArray = remainingCoords.ToArray();
            // Pick a random location.
            int[] location = toArray[random.Next(toArray.Length)];
            // This pair of coordinates is now taken. So now remove it from list of available coords.
            remainingCoords.Remove(location);

            // Return the coordinates.
            return location;
        }

        public int[,] GetBox(int[] coords)
        {
            // The coords variable must be a pair of valid coordinates.
            if (coords.Length != 2) { throw new Exception(""); }
            
            // Initiate the box to be returned after construction.
            int[,] box = new int[3, 3];

            // Get the minimum index on board to construct the box.
            int Xmin = coords[0] - (coords[0] % 3);
            int Ymin = coords[1] - (coords[1] % 3);

            // Store the appropriate numbers into the box.
            for (int x=0; x<3; x++)
            {
                for (int y=0; y<3; y++)
                {
                    box[x, y] = board[Xmin + x, Ymin + y];
                }
            }

            // Return the constructed box.
            return box;
        }

        // Get array index based on board x,y coordinates.
        public int[] GetIndices(int[] location) {
            return new int[2] { location[0]--, location[1]--};
        }

        // Get board x,y coordinates based on array indices.
        public int[] GetLocation(int[] indices)
        {
            return new int[2] { indices[0]++, indices[1]++ };
        }

        private bool IsSolvable()
        {
            // Get maximum coordinates of the game board.
            int Xmax = board.GetLength(0);
            int Ymax = board.GetLength(1);

            // Check for solvability by applying several different sudoku tricks.
            FillBoard();
            
            return IsFull(testBoard);
        }

        // Check if test is a full board.
        private bool IsFull(int[,] test)
        {
            for (int x=0; x<9; x++)
            {
                for (int y=0; y<9; y++)
                {
                    if (test[x,y] == 0) { return false; }
                }
            }
            return true;
        }

        // Fill up the board as much as possible.
        private int[,] FillBoard()
        {
            int[,] current = board;
            int[,] next = CoverBoard();
            while(current != next) { current = next; next = CoverBoard(); }
            return current;
        }

        // One single iteration of filling all the cells on the board.
        private int[,] CoverBoard()
        {
            // Trick 1: Test all boxes, rows, and columns and fill directly.
            
            // Trick 2: Fill up remaining cells.

            // Trick 3: Fill up only remaining options for given cells.

        }

        // Add the integer i to the apporpriate location.
        private void Place(int i, int[] location)
        {

        }
    }
}

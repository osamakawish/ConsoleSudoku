using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSudoku
{
    /// <summary>
    ///  
    /// </summary>
    class Board
    {
        public enum Axis { row, col, box };

        /// <summary>
        /// Multiple variables are used for the sake of algorithmic efficiency.
        /// </summary>
        // Initiate the board (to be presented to player) and testboard (for algorithmic efficiency).
        int[,] board = new int[9, 9];
        // Stores noted integers. Which the user can add ass possible options.
        private HashSet<int>[,] availableValues;
        // Index n-1: Determines how many times n has been used on board.
        private int[] count;
        // The set of unused coordinats.
        private HashSet<int[]> remainingCoords;
        // Stores the locations of all integers.
        private bool[,,] locations;
        // Contains contents of each box. Boxes and cells number left -> right, up -> down.
        private int[,] box = new int[9, 9]; // ie. box[3,5] returns value of cell 6 of box 4;

        /// <summary>
        /// Initiate the 9x9 Sudoku board.
        /// </summary>
        public Board()
        {
            // Initiate count variable.
            count = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // Everything is stored regarding integer n+1 in index n. 
            locations = new bool[9, 3, 9]; // A[7, 2, 4] = true : 8 (=7+1) is contained in box (Axis 2) 5 (=4+1).
            // Fill up remainingCoords variable to include every coordinate. 
            AddCoords();
        }

        /// <summary>
        /// Test.
        /// </summary>
        private void AddCoords()
        {
            // Initiate a hashset of all values.
            HashSet<int> allValues = new HashSet<int>();
            for (int i = 0; i < 9; i++) { allValues.Add(i); }

            // Fill up all the variables, updating each field.
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; x++)
                {
                    // Place 0 at the cell.
                    PlaceAt(0, new int[2] { x, y });
                    // Add each possible coordinates as a remaining one.
                    remainingCoords.Add(new int[] { x, y });
                    // All values are available for each given cell.
                    availableValues[x, y] = allValues;
                }
            }
        }

        // Provides a more intuitive approach to indexing locations array.
        private bool Locations(int i, Axis axis, int axisNumber) => locations[i - 1, (int)axis, axisNumber];

        // Get array index based on board x,y coordinates.
        public static int[] GetIndices(int[] location) => new int[2] { location[0]--, location[1]-- };

        // Get board x,y coordinates based on array indices.
        public static int[] GetLocation(int[] indices) => new int[2] { indices[0]++, indices[1]++ };

        /// <summary>
        /// Implements the locations variable more intuitively.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="axis"></param>
        /// <param name="axisNumber"></param>
        private void AddLocation(int i, Axis axis, int axisNumber)
        {
            locations[i, (int)axis, axisNumber--] = true;
        }

        /// <summary>
        /// Provide cell index in box notation.
        /// Result between 1 and 9.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int Cell(int[] cell) => 3 * (cell[1]%3) + (cell[0]%3) + 1;
        /// <summary>
        /// Provide box index in box notation. 
        /// Result between 1 and 9.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int Box(int[] cell) => 3 * cell[1] + cell[0] - (Cell(cell) - 1) + 1;

        /// <summary>
        /// Returns the number of times integer i has been placed on the board.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int Count(int i) => count[i - 1];

        /// <summary>
        /// Converts planar (x,y) notation to box notation.
        /// </summary>
        /// <param name="planarCoords"></param>
        /// <returns></returns>
        public int[] PlanarToBox(int[] planarCoords)
        {
            int[] boxCoords = new int[2];

            // Get box number.
            boxCoords[0] = Box(planarCoords);
            // Get cell number in box.
            boxCoords[1] = Cell(planarCoords);

            return boxCoords;
        }

        /// <summary>
        /// Converts box notation to planar (x,y) notation.
        /// </summary>
        /// <param name="boxCoords"></param>
        /// <returns></returns>
        public int[] BoxToPlanar(int[] boxCoords)
        {
            int[] planarCoords = new int[2];

            // Get row.
            planarCoords[0] = (boxCoords[0] / 3) + (boxCoords[1] / 3);
            // Get column.
            planarCoords[1] = (boxCoords[0] % 3) + (boxCoords[1] % 3);

            return planarCoords;
        }

        /// <summary>
        /// Place integer i at cell, while updating all appropriate variables.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="cell"></param>
        public void PlaceAt(int i, int[] cell)
        {
            // Update remainingCoords set if necessary.
            if (remainingCoords.Contains(cell)) { remainingCoords.Remove(cell); }

            // Place it in the board.
            board[cell[0], cell[1]] = i;
            // Update box variable.
            box[Box(cell), Cell(cell)] = i;
            // Update locations variable accordingly.
            AddLocation(i, Axis.row, cell[0]);
            AddLocation(i, Axis.col, cell[1]);
            AddLocation(i, Axis.box, Box(cell));
            // Update count variable.
            count[i - 1]++;

            // Update the list of available values.
            availableValues[cell[0], cell[1]] = new HashSet<int>();
            for (int n=0; n<9; n++)
            {
                // Update available values in the same row or column.
                availableValues[n, cell[1]].Remove(i);
                availableValues[cell[0], n].Remove(i);

                // Convert planar notation to box notation.
                int[] boxCell = PlanarToBox(cell);
                // Update available values in the same box.
                for (int m=0; m<9; m++)
                {
                    // Consider the other cell in box notation.
                    int[] otherCell = new int[2] { Box(cell)-1, m };
                    // If the other cell is not this cell,
                    if (otherCell != boxCell)
                    {
                        // remove i from its list of available values.
                        int[] planarOtherCell = BoxToPlanar(otherCell);
                        availableValues[planarOtherCell[0], planarOtherCell[1]].Remove(i);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true iff the board is full.
        /// </summary>
        public bool IsFull => remainingCoords.Count == 0;

        /// <summary>
        /// Get a random remaining cell on the board.
        /// </summary>
        public int[] RandomRemainingCell
        {
            get
            {
                // Initiate randomizer
                Random randomizer = new Random();
                // Convert the hashset to an array for indexing.
                int[][] arraySet = remainingCoords.ToArray();
                // Pick a random index.
                int index = randomizer.Next(arraySet.Length);

                // Return the coordinates of the random remaining cell.
                return arraySet[index];
            }
        }

        // Return the boxes in the neighborhood of the box with provided boxNumber.
        private int[] BoxNeighborhood(int boxNumber)
        {

            // For a more general algorithm, consider splitting into horizontal and vertical case.
            
            switch (boxNumber)
            {
                case 1: return new int[5] { 1, 2, 3, 4, 7 };
                case 2: return new int[5] { 1, 2, 3, 5, 8 };
                case 3: return new int[5] { 1, 2, 3, 6, 9 };
                case 4: return new int[5] { 1, 4, 5, 6, 7 };
                case 5: return new int[5] { 2, 4, 5, 6, 8 };
                case 6: return new int[5] { 3, 4, 5, 6, 9 };
                case 7: return new int[5] { 1, 4, 7, 8, 9 };
                case 8: return new int[5] { 2, 5, 7, 8, 9 };
                case 9: return new int[5] { 3, 6, 7, 8, 9 };
            }

            throw new Exception();
        }

        // A more intuitive row and column getter.
        public int Row(int[] cell) => cell[0]++;
        public int Column(int[] cell) => cell[1]++;

        // Return an array of available values for the given cell.
        public int[] AvailableValues(int[] cell)
        {
            // The list of available values for a given cell.
            List<int> list = new List<int>();
            // The row and column values for the cell.
            int cellRow = Row(cell); int cellCol = Column(cell);

            // For each integer i,
            for (int i=0; i<9; i++)
            {
                // If i is not in the row or column,
                if (!Locations(i,Axis.row,cellRow) && !Locations(i,Axis.col,cellCol))
                {
                    // or box
                    if (!BoxContains(Box(cell),i))
                    {
                        // Add i to list of available values
                        list.Add(i);
                    }
                }
            }

            // Return the list of available values at the given cell.
            return list.ToArray();
        }

        // Check if the box contains the given value.
        private bool BoxContains(int boxNumber, int value)
        {
            for (int i=0; i<9; i++)
            {
                if (box[boxNumber,i]==value) { return true; }
            }
            return false;
        }

        // Return a random available value at the given cell.
        public int RandomValue(int[] cell)
        {
            // Get the array of available values.
            int[] values = AvailableValues(cell);
            // Pick a random one from it.
            Random random = new Random();
            int index = random.Next(values.Length);

            // Return the randomized available value.
            return values[index];
        }

        // Check if the board is solved.
        public bool IsSolved
        {
            get
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int axisNumber = 0; axisNumber < 9; axisNumber++)
                    {
                        // If it's not in the same box, row, col, or used 9 times,
                        if ( !( count[i] == 9 && Locations(i,Axis.row,axisNumber) &&
                            Locations(i,Axis.col,axisNumber) && Locations(i,Axis.box,axisNumber) ) )
                        {
                            return false;
                        }
                    }
                }

                // If every number has been used 9 times, and the boxes, columns, and rows have no false
                // values, then return true.
                return true;
            }
        }
    }
}

using Minesweeper.Models;
using System;
using System.Linq;

namespace Minesweeper.Implementation
{
    public class StaticBoardGenerator : IBoardGenerator
    {
        private readonly string _boardDefinition;
        private readonly int rowCountNumber = 3;

        public StaticBoardGenerator(string boardDefinition)
        {
            
            // check if board definition is null or empty
            if (string.IsNullOrEmpty(boardDefinition))
                throw new ArgumentException("Board definition cannot be null or empty.");

            // split into rows
            var rows = boardDefinition.Split(',');
            int rowCount = rows.Length;

            // check for min 3 rows
            if (rowCount < rowCountNumber)
            {
                throw new ArgumentException("Board must have at least three rows.");
            }

            // check all rows have same number of columns
            int expectedColumnCount = rows[0].Length;
            if (rows.Any(row => row.Length != expectedColumnCount))
            {
                throw new ArgumentException("Board must have equal amount of columns.");
            }

            if (expectedColumnCount <  rowCountNumber)
            {
                throw new ArgumentException("Board must have at least three columns.");
            }


            AssertToAtLeastOneMine(boardDefinition);

            _boardDefinition = boardDefinition;
        }

        private static void AssertToAtLeastOneMine(string boardDefinition)
        {
            if (!boardDefinition.Contains('m'))
            {
                throw new ArgumentException("Board must have at least one mine.");
            }
        }

        public Board GenerateBoard()
        {
            var rows = _boardDefinition.Split(',');
            int rowCount = rows.Length;
            int columnCount = rows[0].Length;

            // count mines and create board
            int mineCount = rows.Sum(row => row.Count(tile => tile == 'm'));
            Board board = new Board(columnCount, rowCount, mineCount);

            // populate tiles
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columnCount; colIndex++)
                {
                    board.Tiles[rowIndex, colIndex] = new Tile(rows[rowIndex][colIndex] == 'm');
                }
            }

            return board;
        }
    }
}

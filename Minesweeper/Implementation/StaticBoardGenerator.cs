using Minesweeper.Models;
using System;
using System.Linq;

namespace Minesweeper.Implementation
{
    public class StaticBoardGenerator : IBoardGenerator
    {
        private readonly string _boardDefinition;

        public StaticBoardGenerator(string boardDefinition)
        {
            if (string.IsNullOrWhiteSpace(boardDefinition))
                throw new ArgumentException("Board definition cannot be null or empty.");

            _boardDefinition = boardDefinition;
        }

        public Board GenerateBoard()
        {
            // Split the board definition into rows
            var rows = _boardDefinition.Split(',');
            int rowCount = rows.Length;
            int expectedColumnCount = rows[0].Length;

            // Check if the board definition has at least three rows
            if (rowCount < 3)
            {
                throw new ArgumentException("Board must have at least three rows.");
            }

            // Check each row to ensure it has the same number of columns
            foreach (var row in rows)
            {
                if (row.Length != expectedColumnCount)
                {
                    throw new ArgumentException("All rows must have the same number of columns.");
                }
            }

            // Check for at least one mine in the board definition
            if (!rows.Any(row => row.Contains('m')))
            {
                throw new ArgumentException("Board must have at least one mine.");
            }

            // Count the number of mines and create the Board object
            int mineCount = rows.Sum(row => row.Count(tile => tile == 'm'));
            
            // Create the Board object
            Board board = new Board(expectedColumnCount, rowCount, mineCount);

            // Populate the board with Tile objects
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < expectedColumnCount; col++)
                {
                    bool isMine = rows[row][col] == 'm';
                    board.Tiles[row, col] = new Tile(isMine);
                }
            }

            return board;
        }
    }
}

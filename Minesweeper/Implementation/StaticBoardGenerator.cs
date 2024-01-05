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
            // splitt opp brettdefinisjonen i rader
            var rows = _boardDefinition.Split(',');
            int expectedColumnCount = rows[0].Length;

            // sjekk hver eneste rad for å se om de har samme antall kolonner
            foreach (var row in rows)
            {
                if (row.Length != expectedColumnCount)
                {
                    throw new ArgumentException("All rows must have the same number of columns.");
                }
            }

            // miner på brettet
            if (!rows.Any(row => row.Contains('m')))
            {
                throw new ArgumentException("Board must have at least one mine.");
            }


            int mineCount = rows.Sum(row => row.Count(tile => tile == 'm'));
            return new Board(expectedColumnCount, rows.Length, mineCount);
        }
    }
}

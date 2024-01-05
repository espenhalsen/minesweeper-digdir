using Minesweeper.Models;
using System;

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
        //TODO: Implement this method

        return new Board(0, 0, 0);
    }
}
}

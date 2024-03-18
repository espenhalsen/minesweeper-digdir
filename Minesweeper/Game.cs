using Minesweeper.Models;
using System;

namespace Minesweeper;

public class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public IPlayer Player { get; private set; }
    private Board _board { get; set; }
    private bool _isFirstTileRevealed { get; set; } = false;

    public Game(IBoardGenerator boardGenerator, IPlayer player)
    {
        _board = boardGenerator.GenerateBoard();
        Player = player;
    }

    public void RevealTile(int row, int column)
    {
        if (!_isFirstTileRevealed)
        {
            StartTime = DateTime.Now;
            _isFirstTileRevealed = true;
        }

        var tile = _board.Tiles[row, column];
        if (tile.IsMine)
        {
            EndTime = DateTime.Now;
            throw new MineExplodedException();
        }

        _board.RevealTile(row, column);
    }

    public void FlagTile(int row, int column)
    {
        _board.FlagTile(row, column);
        if (_board.GameWon)
        {
            EndTime = DateTime.Now;
        }
    }

    public double GetSecondsUsed()
    {
        if (!StartTime.HasValue) return 0;
        var endTime = EndTime ?? DateTime.Now;
        return (endTime - StartTime.Value).TotalSeconds;
    }
}

public class MineExplodedException : Exception
{
    public MineExplodedException() : base("A mine has been revealed!") { }
}

﻿using Minesweeper.Models;

namespace Minesweeper.Implementation;

public class StaticBoardGenerator : IBoardGenerator
{

    private readonly string[] _rows;
    private readonly int _minesCount;
    private readonly string _boardDefinition;
    private const char mineCharacter = 'm';
    private const int minimumMines = 1;
    private const int minimumColumns = 3;
    private const int minimumRows = 3;


    public StaticBoardGenerator(string boardDefinition)
    {
        if (string.IsNullOrEmpty(boardDefinition))
        {
            throw new ArgumentNullException("Board definition cannot be null or empty");
        }
        _boardDefinition = boardDefinition;
        _rows = _boardDefinition.Split(',');
        _minesCount = _boardDefinition.Count(c => c == mineCharacter);
        if (_rows.Length < minimumRows)
        {
            throw new ArgumentException("Board definition must have at least three rows");
        }
        if (_rows.Any(row => row.Length != _rows[0].Length))
        {
            throw new ArgumentException("Board definition must have equal amount of columns");
        }
        if (_rows[0].Length < minimumColumns)
        {
            throw new ArgumentException("Board must have at least three columns");
        }
        if (_minesCount < minimumMines)
        {
            throw new ArgumentException("Board must have at least one mine");
        }


    }

    public Board GenerateBoard()
    {
        int rowsCount = _rows.Length;
        int columnsCount = _rows[0].Length;

        Board board = new(columnsCount, rowsCount, _minesCount);
        for (int rowIndex = 0; rowIndex < board.Rows; rowIndex++)
        {
            for (int columIndex = 0; columIndex < board.Columns; columIndex++)
            {
                bool isMine = _rows[rowIndex][columIndex] == mineCharacter;

                board.Tiles[rowIndex, columIndex] = new Tile(isMine);
            }
        }

        return board;
    }
}

import React from 'react';
import { FaFlag } from "react-icons/fa";
import { getIconForAdjacentMines } from "./utils"; // Make sure this is implemented to handle mines and numbers

const GameBoard = ({ tileData, revealTile, flagTile, unflagTile }) => {
  // Click handlers call the appropriate functions passed down as props
  const handleTileClick = (tile) => {
    if (!tile.isRevealed && !tile.isFlagged) {
      revealTile(tile.row, tile.col);
    }
  };

  const handleTileRightClick = (event, tile) => {
    event.preventDefault();
    if (!tile.isRevealed) {
      tile.isFlagged ? unflagTile(tile.row, tile.col) : flagTile(tile.row, tile.col);
    }
  };

  // Create a grid with buttons for each tile
  return (
    <div className="grid">
      {tileData.map((tile, index) => (
        <button
          key={index}
          className={`tile ${tile.isRevealed ? 'revealed' : ''} ${tile.isFlagged ? 'flagged' : ''}`}
          onClick={() => handleTileClick(tile)}
          onContextMenu={(e) => handleTileRightClick(e, tile)}
        >
          {tile.isFlagged && !tile.isRevealed ? <FaFlag /> : getIconForAdjacentMines(tile)}
        </button>
      ))}
    </div>
  );
};

export default GameBoard;

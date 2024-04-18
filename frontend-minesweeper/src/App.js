import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';
import winner from './winner.gif';
import explode from './explode.gif';

function App() {
  const [apiResponse, setApiResponse] = useState(null);
  const [tileData, setTileData] = useState([]);
  const [level, setLevel] = useState(0);
  const [gameId, setGameId] = useState(null);
  const [gameWon, setGameWon] = useState(false);
  const [gameOver, setGameOver] = useState(false);  
  const [time, setTime] = useState(0);
  const [timerOn, setTimerOn] = useState(false);
  const [showPopup, setShowPopup] = useState(false);

  const API_HOST = 'http://localhost:5008';
  const gameSettings = [
    { rows: 9, columns: 9, mines: 3 },
    { rows: 18, columns: 18, mines: 15 },
    { rows: 60, columns: 60, mines: 99 },
  ];

  useEffect(() => {
    const timer = timerOn && setInterval(() => setTime(time + 1), 1000);
    return () => clearInterval(timer);
  }, [timerOn, time]);

  useEffect(() => {
    createNewGame();
  }, [level]);

  useEffect(() => {
    checkVictory();
    if (apiResponse && apiResponse.gameWon) {
      console.log('Du vant!'); }
    if (apiResponse && apiResponse.mineExploded) {
      console.log('Spillet er over');
      setGameOver(true);
      setTimerOn(false);
      setShowPopup(true);
      setTimeout(() => setShowPopup(false), 1000); 
      const updatedTiles = tileData.map(tile => ({
        ...tile,
        isRevealed: tile.mine ? true : tile.isRevealed
      }));
      setTileData(updatedTiles);
    }
  }, [apiResponse]);

  const createNewGame = () => {
    setApiResponse(null);
    setGameWon(false);
    setGameOver(false);
    setShowPopup(false);
    setTileData([]);
    setTime(0);
    setTimerOn(false);
    const data = { level, userName: "sigmaronny" };

    axios.post(`${API_HOST}/games/`, data)
      .then(response => {
        setApiResponse(response.data);
        setGameId(response.data.gameId);
        if (response.data.board && response.data.board.tiles) {
          setTileData(response.data.board.tiles);
        }
        setTimerOn(true);
      })
      .catch(error => console.error("Feil ved oppretting av spill:", error));
  };

  const handleTileAction = (row, column, move) => {
    if (gameWon || gameOver) { return; }
    const flaggedTiles = tileData.filter(tile => tile.isFlagged).length;
    if (move === 1 && flaggedTiles >= gameSettings[level].mines) {
      return;
    }
    const MoveData = { row, column, move };
    axios.put(`${API_HOST}/games/${gameId}`, MoveData)
      .then(response => {
        setApiResponse(response.data);
        if (response.data.board && response.data.board.tiles) {
          setTileData(response.data.board.tiles);
        }
        checkVictory();
      })
      .catch(error => console.error("Feil ved oppdatering av flis:", error));
  };

  const checkVictory = () => {
    const totalTiles = gameSettings[level].rows * gameSettings[level].columns;
    const totalMines = gameSettings[level].mines;
    const revealedTiles = tileData.filter(tile => tile.isRevealed).length;

    if (revealedTiles === totalTiles - totalMines) {
      setGameWon(true);
      setTimerOn(false);
    }
  };

  const getNumberColor = (number) => {
    const colors = ["", "#0000FF", "#008000", "#FF0000", "#800080", "#800000", "#40E0D0", "#000000", "#808080"];
    return colors[number];
  };

  return (
    <div className="App">
      <header className="App-header">
        <div className='game'>
          <h1>Altinn II<br />Bomberydder</h1>
          <select className='level-select' value={level} onChange={e => setLevel(Number(e.target.value))}>
            <option value={0}>Nybegynner</option>
            <option value={1}>Mellomnivå</option>
            <option value={2}>Ekspert</option>
          </select>
          <div className='game-info'>
            <div className='timer'>{String(time).padStart(3, '0')}</div>
            <button className='reset' onClick={createNewGame}>{gameWon ? <img src={winner} height='30px' alt='' /> : gameOver ? '😵' : '🙂'}</button>
            <div className='mines'>{String(gameSettings[level].mines - tileData.filter(t => t.isFlagged).length).padStart(3, '0')}</div>
          </div>

          <div className='grid' style={{
            gridTemplateColumns: `repeat(${gameSettings[level].columns}, 30px)`,
            gridTemplateRows: `repeat(${gameSettings[level].rows}, 30px)`
          }}>
            {tileData.map((tile, index) => (
              <div key={index}
                onMouseDown={e => {
                  e.preventDefault();
                  const move = e.button === 2 ? (tile.isFlagged ? 2 : 1) : 0;
                  handleTileAction(tile.row, tile.col, move);
                }}
                onContextMenu={e => e.preventDefault()}
                className={`tile ${tile.exploded ? 'exploded' : ''} ${tile.isFlagged ? 'flagged' : ''} ${tile.isRevealed ? 'revealed' : ''}`}
                style={{ color: getNumberColor(tile.adjacentMines) }}>
                {tile.isFlagged ? '🚩' : (tile.mine && tile.isRevealed ? '💣' : (tile.isRevealed ? (tile.adjacentMines ? tile.adjacentMines : '') : ''))}
              </div>
            ))}
          </div>
          {showPopup && (
            <div className="popup">
              <img src={gameOver ? explode : winner} height="600vh"alt='' /
>
            </div>
          )}
        </div>

      </header>
    </div>
  );
}

export default App;

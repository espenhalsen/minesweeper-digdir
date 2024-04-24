import axios from 'axios';

const API_HOST = 'http://localhost:5008';

export const createNewGame = async (level) => {
  try {
    const response = await axios.post(`${API_HOST}/games/`, { level });
    return response.data;
  } catch (error) {
    console.error("Error creating new game:", error);
    throw error;
  }
};

export const makeMove = async (gameId, row, col, move) => {
  try {
    const response = await axios.put(`${API_HOST}/games/${gameId}`, { row, col, move });
    return response.data;
  } catch (error) {
    console.error("Error making a move:", error);
    throw error;
  }
};

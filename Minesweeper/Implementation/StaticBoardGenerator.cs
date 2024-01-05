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
            // her sjekker den om brettdefinisjonen er null eller tom.
            // og hvis den er det, kastes en ArgumentException for å indikere at boardDefinition ikke kan være tom.
            if (string.IsNullOrWhiteSpace(boardDefinition))
                throw new ArgumentException("Board definition cannot be null or empty.");

            // Deler brettdefinisjonen inn i rader basert på komma.
            var rows = boardDefinition.Split(',');
            // sjekker om antallet rader er null, som indikerer et ugyldig brett.
            // hvis den er det kastes en ArgumentException for å indikere at brettet må ha minst en rad.
            // dette (skal) løse feilen "GenerateBoard_InvalidTooFewRows_ShouldThrowException" :p.
            if (rows.Length == 0) 
                throw new ArgumentException("Board must have at least one row.");

            _boardDefinition = boardDefinition;
        }

        // Metoden for å generere brettet. Denne metoden er ikke implementert enda, men den kommer
        public Board GenerateBoard()
        {

            return new Board(0, 0, 0); 
        }
    }
}

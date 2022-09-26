using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_Console.Entities.Enums;

namespace Chess_Console.Entities
{
     abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; set; }
        public int NumberOfMoves { get; set; }
        public Board Board { get; set; }

        public Piece()
        {

        }

        public Piece(Color color, Board board)
        {
            Position = null;
            Color = color;
            Board = board;
            NumberOfMoves = 0;
        }

        public void IncrementMovements()
        {
            NumberOfMoves++;
        }

        public void DecrementMovements()
        {
            NumberOfMoves--;
        }

        public bool ThereIsPossibleMovements()
        {
            bool[,] mat = PossibleMovements();
            for(int i = 0; i < Board.Lines; i++)
            {
                for(int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanMoveTo(Position pos)
        {
            return PossibleMovements()[pos.Line, pos.Column];
        }

        public abstract bool[,] PossibleMovements();
        
    }
}

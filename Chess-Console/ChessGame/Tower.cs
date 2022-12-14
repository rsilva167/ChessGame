using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_Console.Entities;
using Chess_Console.Entities.Enums;

namespace Chess_Console.ChessGame
{
    internal class Tower : Piece
    {
        public Tower(Board board, Color color) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "T";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];
            Position pos = new Position(0, 0);

            //Above
            pos.SetValues(pos.Line - 1, pos.Column);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.Piece(pos) != null && Board.Piece(pos).Color != Color)
                {
                    break;
                }

                pos.SetValues(pos.Line - 1, pos.Column);

            }

            //Right
            pos.SetValues(pos.Line, pos.Column + 1);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.Piece(pos) != null && Board.Piece(pos).Color != Color)
                {
                    break;
                }

                pos.SetValues(pos.Line, pos.Column + 1);

            }

            //Under
            pos.SetValues(pos.Line + 1, pos.Column);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.Piece(pos) != null && Board.Piece(pos).Color != Color)
                {
                    break;
                }

                pos.SetValues(pos.Line + 1, pos.Column);

            }

            //Left
            pos.SetValues(pos.Line, pos.Column - 1);
            while (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
                if (Board.Piece(pos) != null && Board.Piece(pos).Color != Color)
                {
                    break;
                }

                pos.SetValues(pos.Line, pos.Column - 1);
            }
            return mat;
        }
    }
}

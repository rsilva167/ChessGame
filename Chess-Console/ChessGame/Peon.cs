using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_Console.Entities;
using Chess_Console.Entities.Enums;

namespace Chess_Console.ChessGame
{
    internal class Peon : Piece
    {
        private PlayingChess Match;
        public Peon(Board board, Color color, PlayingChess match)
            : base(color, board)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool ThereIsEnemy(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p != null && p.Color != Color;
        }

        private bool Freeway(Position pos)
        {
            return Board.Piece(pos) == null;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];
            Position pos = new Position(0, 0);
            
            if(Color == Color.White)
            {
                pos.SetValues(pos.Line - 1, pos.Column);
                if (Board.ValidPosition(pos) && Freeway(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.SetValues(pos.Line - 2, pos.Column);
                if (Board.ValidPosition(pos) && Freeway(pos) && NumberOfMoves == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.SetValues(pos.Line - 1, pos.Column - 1);
                if (Board.ValidPosition(pos) && ThereIsEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.SetValues(pos.Line - 1, pos.Column + 1);
                if (Board.ValidPosition(pos) && ThereIsEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // #Special play: en passant
                if (Position.Line == 3)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if(Board.ValidPosition(left) && ThereIsEnemy(left) && Board.Piece(left) == Match.VulnerableEnPassant)
                    {
                        mat[left.Line - 1, left.Column] = true;
                    }

                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.ValidPosition(right) && ThereIsEnemy(right) && Board.Piece(right) == Match.VulnerableEnPassant)
                    {
                        mat[right.Line - 1, right.Column] = true;
                    }
                }
            }

            else
            {
                pos.SetValues(pos.Line + 1, pos.Column);
                if (Board.ValidPosition(pos) && Freeway(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.SetValues(pos.Line + 2, pos.Column);
                if (Board.ValidPosition(pos) && Freeway(pos) && NumberOfMoves == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.SetValues(pos.Line + 1, pos.Column + 1);
                if (Board.ValidPosition(pos) && ThereIsEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.SetValues(pos.Line + 1, pos.Column - 1);
                if (Board.ValidPosition(pos) && ThereIsEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
                // #Special play: en passant
                if (Position.Line == 4)
                {
                    Position left = new Position(Position.Line, Position.Column - 1);
                    if (Board.ValidPosition(left) && ThereIsEnemy(left) && Board.Piece(left) == Match.VulnerableEnPassant)
                    {
                        mat[left.Line + 1, left.Column] = true;
                    }

                    Position right = new Position(Position.Line, Position.Column + 1);
                    if (Board.ValidPosition(right) && ThereIsEnemy(right) && Board.Piece(right) == Match.VulnerableEnPassant)
                    {
                        mat[right.Line + 1, right.Column] = true;
                    }
                }
            }

            return mat;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_Console.Entities;
using Chess_Console.Entities.Enums;

namespace Chess_Console.ChessGame
{
    internal class King : Piece
    {
        private PlayingChess Match;
        public King(Board board, Color color, PlayingChess match): base(color, board)
        {
            Match = match;
        }

        /*public King(Board board, Color color) : base(color, board)
        {
        }*/

        public override string ToString()
        {
            return "K";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p == null || p.Color != Color;
        }

        private bool TestTowerToRock(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p != null && p is Tower && p.Color == Color && p.NumberOfMoves == 0;
        }
        public override bool[,] PossibleMovements()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];
            Position pos = new Position(0, 0);

            //Above
            pos.SetValues(pos.Line - 1, pos.Column);
            if(Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //Ne
            pos.SetValues(pos.Line - 1, pos.Column + 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //Right
            pos.SetValues(pos.Line, pos.Column + 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //Se
            pos.SetValues(pos.Line + 1, pos.Column + 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //Under
            pos.SetValues(pos.Line + 1, pos.Column);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //So
            pos.SetValues(pos.Line + 1, pos.Column - 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //Left
            pos.SetValues(pos.Line, pos.Column - 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //No
            pos.SetValues(pos.Line - 1, pos.Column - 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            //SpecialPlay -small rock
            if(NumberOfMoves == 0 && !Match.Check)
            {
                Position posT1 = new Position(Position.Line, Position.Column + 3);
                if(TestTowerToRock(posT1))
                {
                    Position p1 = new Position(Position.Line, Position.Column + 1);
                    Position p2 = new Position(Position.Line, Position.Column + 2);

                    if(Board.Piece(p1) == null && Board.Piece(p2) == null)
                    {
                        mat[Position.Line, Position.Column + 2] = true;
                    }


                }

                //SpecialPlay - big rock
                    Position posT2 = new Position(Position.Line, Position.Column - 4);
                    if (TestTowerToRock(posT2))
                    {
                        Position p1 = new Position(Position.Line, Position.Column - 1);
                        Position p2 = new Position(Position.Line, Position.Column - 2);
                        Position p3 = new Position(Position.Line, Position.Column - 3);
                        if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                        {
                            mat[Position.Line, Position.Column - 2] = true;
                        }


                    }               
            }

            return mat;
        }

    }
}

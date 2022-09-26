using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess_Console.Entities;
using Chess_Console.Entities.Enums;

namespace Chess_Console.ChessGame
{
    internal class PlayingChess
    {
        public Board Board { get; private set; }

        public int Turn { get; private set; }

        public Color ActualPlayer { get; private set; }
        public bool Finished { get; private set; }

        private HashSet<Piece> Pieces;

        private HashSet<Piece> Catched;
        public bool Check { get; private set; }

        public Piece VulnerableEnPassant { get; private set; }

        public PlayingChess()
        {
            Board = new Board(8, 8);
            Turn = 1;
            ActualPlayer = Color.White;
            Finished = false;
            Check = false;
            VulnerableEnPassant = null;
            Pieces = new HashSet<Piece>();
            Catched = new HashSet<Piece>();
            PutPieces();
        }

        public Piece ExecuteMovements(Position origin, Position destiny)
        {
            Piece p = Board.RemovePieces(origin);
            p.IncrementMovements();
            Piece CapturedPiece = Board.RemovePieces(destiny);
            Board.SetPieces(p, destiny);
            if(CapturedPiece != null)
            {
                Catched.Add(CapturedPiece);
            }

            // Special play - small rock
            if(p is King && destiny.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Line, origin.Column + 3);
                Position destinyT = new Position(origin.Line, origin.Column + 1);
                Piece T = Board.RemovePieces(originT);
                T.IncrementMovements();
                Board.SetPieces(T, destinyT);
            }

            // Special play - big rock
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Line, origin.Column - 4);
                Position destinyT = new Position(origin.Line, origin.Column - 1);
                Piece T = Board.RemovePieces(originT);
                T.IncrementMovements();
                Board.SetPieces(T, destinyT);
            }

            // Special play - En passant
            if(p is Peon)
            {
                if(origin.Column != destiny.Column && CapturedPiece == null)
                {
                    Position posP;
                    if(p.Color == Color.White)
                    {
                        posP = new Position(destiny.Line + 1, destiny.Column);
                    }
                    else
                    {
                        posP = new Position(destiny.Line - 1, destiny.Column);
                    }
                    CapturedPiece = Board.RemovePieces(posP);
                    Catched.Add(CapturedPiece);
                }
            }


            return CapturedPiece;
        }

        public void UndoTheMovement(Position origin, Position destiny, Piece capturedPiece)
        {
            Piece p = Board.RemovePieces(destiny);
            p.DecrementMovements();
            if(capturedPiece != null)
            {
                Board.SetPieces(capturedPiece, destiny);
                Catched.Remove(capturedPiece);
            }
            Board.SetPieces(p, origin);

            // Special play - small rock
            if (p is King && destiny.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Line, origin.Column + 3);
                Position destinyT = new Position(origin.Line, origin.Column + 1);
                Piece T = Board.RemovePieces(originT);
                T.DecrementMovements();
                Board.SetPieces(T, destinyT);
            }
            // Special play - Big rock

            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Line, origin.Column - 4);
                Position destinyT = new Position(origin.Line, origin.Column - 1);
                Piece T = Board.RemovePieces(originT);
                T.DecrementMovements();
                Board.SetPieces(T, destinyT);
            }

            // Special play - En passant
            if(p is Peon)
            {
                if(origin.Column != destiny.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece peon = Board.RemovePieces(destiny);
                    Position posP;
                    if(p.Color == Color.White)
                    {
                        posP = new Position(3, destiny.Column);
                    }
                    else
                    {
                        posP = new Position(4, destiny.Column);
                    }
                    Board.SetPieces(peon, posP);
                }
            }

        }
        public void MakeAPlay(Position origin,Position destiny)
        {
            Piece capturedPiece = ExecuteMovements(origin, destiny);
            if(IsInCheck(ActualPlayer))
            {
                UndoTheMovement(origin, destiny, capturedPiece);
                throw new BoardExceptions("You can't put yourself in check!");
            }

            if(IsInCheck(Adversary(ActualPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            if (CheckmateTest(Adversary(ActualPlayer)))
            {
                Finished = true;
            }

            else
            {

                Turn++;
                ChangeTurn();
            }
            Piece p = Board.Piece(destiny);
            if(p is Peon && (destiny.Line == origin.Line - 2 || destiny.Line == origin.Line + 2))
            {
                VulnerableEnPassant = p;
            }
            else
            {
                VulnerableEnPassant = null;
            }
        }

        public void ValidateHomePosition(Position pos)
        {
            if(Board.Piece(pos) == null)
            {
                throw new BoardExceptions("There is no piece in the chosen origin position!");
            }

            if(ActualPlayer != Board.Piece(pos).Color)
            {
                throw new BoardExceptions("The original piece chosen is not yours!");

            }

            if(!Board.Piece(pos).ThereIsPossibleMovements())
            {
                throw new BoardExceptions("There are no possible moves for the chosen source piece!");

            }
        }

        public void ValidateDestinyPosition(Position origin, Position destiny)
        {
            if(!Board.Piece(origin).CanMoveTo(destiny))
            {
                throw new BoardExceptions("Invalid target position!");
            }
        }

        private void ChangeTurn()
        {
            if (ActualPlayer == Color.White)
            {
                ActualPlayer = Color.Black;
            }
            else
            {
                ActualPlayer = Color.White;
            }
        }

        public HashSet<Piece> CatchedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach(Piece x in Catched)
            {
                if(x.Color == color)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Piece> PiecesInGame(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Pieces)
            {
                if (x.Color == color)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(CatchedPieces(color));
            return aux;
        }

        private Color Adversary(Color color)
        {
            if(color == Color.White)
            {
                return Color.Black;
            }
            return Color.White;
        }

        private Piece Rei(Color color)
        {
            foreach(Piece x in PiecesInGame(color))
            {
                if(x is King)
                {
                    return x;
                }
            }
            return null;
        }

        public bool IsInCheck (Color color)
        {
            Piece R = Rei(color);
            if(R == null)
            {
                throw new BoardExceptions("There is no king of color" + color + " on the board!");
            }

            foreach(Piece x in PiecesInGame(Adversary(color)))
            {
                bool[,] mat = x.PossibleMovements();
                if (mat[R.Position.Line, R.Position.Column])
                {
                    return true;
                }
            }

            return false;
        }

        public bool CheckmateTest(Color cor)
        {
            if(!IsInCheck(cor))
            {
                return false;
            }
            foreach(Piece x in PiecesInGame(cor))
            {
                bool[,] mat = x.PossibleMovements();
                for(int i = 0; i < Board.Lines; i++)
                {
                    for(int j = 0; j < Board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = x.Position;
                            Position destiny = new Position(i, j);
                            Piece capturedPiece = ExecuteMovements(origin, destiny);
                            bool testCheck = IsInCheck(cor);
                            UndoTheMovement(origin, destiny, capturedPiece);
                            if(!testCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }


        public void SetNewPiece(char column, int line, Piece piece)
        {
            Board.SetPieces(piece, new ChessPosition(column, line).ToPosition());
            Pieces.Add(piece);
        }
        private void PutPieces()
        {
            SetNewPiece('a', 1, new Tower(Board, Color.White));
            SetNewPiece('b', 1, new Knight(Board, Color.White));
            SetNewPiece('c', 1, new Bishop(Board, Color.White));
            SetNewPiece('d', 1, new King(Board, Color.White, this));
            SetNewPiece('e', 1, new Queen(Board, Color.White));
            SetNewPiece('f', 1, new Bishop(Board, Color.White));
            SetNewPiece('g', 1, new Knight(Board, Color.White));
            SetNewPiece('h', 1, new Tower(Board, Color.White));
            SetNewPiece('a', 2, new Peon(Board, Color.White, this));
            SetNewPiece('b', 2, new Peon(Board, Color.White, this));
            SetNewPiece('c', 2, new Peon(Board, Color.White, this));
            SetNewPiece('d', 2, new Peon(Board, Color.White, this));
            SetNewPiece('e', 2, new Peon(Board, Color.White, this));
            SetNewPiece('f', 2, new Peon(Board, Color.White, this));
            SetNewPiece('g', 2, new Peon(Board, Color.White, this));
            SetNewPiece('h', 2, new Peon(Board, Color.White, this));

            SetNewPiece('a', 8, new Tower(Board, Color.Black));
            SetNewPiece('b', 8, new Knight(Board, Color.Black));
            SetNewPiece('c', 8, new Bishop(Board, Color.Black));
            SetNewPiece('d', 8, new King(Board, Color.Black, this));
            SetNewPiece('e', 8, new Queen(Board, Color.Black));
            SetNewPiece('f', 8, new Bishop(Board, Color.Black));
            SetNewPiece('g', 8, new Knight(Board, Color.Black));
            SetNewPiece('h', 8, new Tower(Board, Color.Black));
            SetNewPiece('a', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('b', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('c', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('d', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('e', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('f', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('g', 7, new Peon(Board, Color.Black, this));
            SetNewPiece('h', 7, new Peon(Board, Color.Black, this));

        }
    }
}

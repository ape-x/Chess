using System;
using System.Security.Cryptography;
using static BoardGame.Core.Chess;

namespace BoardGame.Core
{
    public class BoardGameFactory : IBoardGameFactory
    {
        public IPieceFactory PieceFactory { get ; set; }

        public BoardGameFactory(IPieceFactory _PieceFactory)
        {
            PieceFactory = _PieceFactory;
        }

        public IBoardGame CreateBoardGame(BoardGameTypes game)
        {
            switch (game)
            {
                case BoardGameTypes.CHESS:


                    RandomNumberGenerator prng = new RNGCryptoServiceProvider();
                    byte[] idBytes = new byte[30];
                    prng.GetBytes(idBytes, 0, 30);


                    var BoardGame = new Chess
                    {
                        Id = Convert.ToBase64String(idBytes),
                        PieceFactory = PieceFactory,
                        Turn = 0,
                        Players = new Player[2]
                    };
                    BoardGame.Players[0] = new Player { Team = ChessTeams.WHITE };
                    BoardGame.Players[1] = new Player { Team = ChessTeams.BLACK };
                    BoardGame.Board = new IPiece[8, 8];


                    for (int i = 0; i < 8; i++)
                    {
                        BoardGame.Board[1, i] = PieceFactory.CreatePiece(1, i, ChessTeams.BLACK, ChessPiece.PAWN);
                        BoardGame.Board[6, i] = PieceFactory.CreatePiece(6, i, ChessTeams.WHITE, ChessPiece.PAWN);
                    }
                    BoardGame.Board[0, 0] = PieceFactory.CreatePiece(0, 0, ChessTeams.BLACK, ChessPiece.ROOK );
                    BoardGame.Board[0, 1] = PieceFactory.CreatePiece(0, 1, ChessTeams.BLACK, ChessPiece.KNIGHT);
                    BoardGame.Board[0, 2] = PieceFactory.CreatePiece(0, 2, ChessTeams.BLACK, ChessPiece.BISHOP);
                    BoardGame.Board[0, 3] = PieceFactory.CreatePiece(0, 3, ChessTeams.BLACK, ChessPiece.QUEEN);
                    BoardGame.Board[0, 4] = PieceFactory.CreatePiece(0, 4, ChessTeams.BLACK, ChessPiece.KING);
                    BoardGame.Board[0, 5] = PieceFactory.CreatePiece(0, 5, ChessTeams.BLACK, ChessPiece.BISHOP);
                    BoardGame.Board[0, 6] = PieceFactory.CreatePiece(0, 6, ChessTeams.BLACK, ChessPiece.KNIGHT);
                    BoardGame.Board[0, 7] = PieceFactory.CreatePiece(0, 7, ChessTeams.BLACK, ChessPiece.ROOK);


                    BoardGame.Board[7, 0] = PieceFactory.CreatePiece(7, 0, ChessTeams.WHITE, ChessPiece.ROOK);
                    BoardGame.Board[7, 1] = PieceFactory.CreatePiece(7, 1, ChessTeams.WHITE, ChessPiece.KNIGHT);
                    BoardGame.Board[7, 2] = PieceFactory.CreatePiece(7, 2, ChessTeams.WHITE, ChessPiece.BISHOP);
                    BoardGame.Board[7, 3] = PieceFactory.CreatePiece(7, 3, ChessTeams.WHITE, ChessPiece.QUEEN);
                    BoardGame.Board[7, 4] = PieceFactory.CreatePiece(7, 4, ChessTeams.WHITE, ChessPiece.KING);
                    BoardGame.Board[7, 5] = PieceFactory.CreatePiece(7, 5, ChessTeams.WHITE, ChessPiece.BISHOP);
                    BoardGame.Board[7, 6] = PieceFactory.CreatePiece(7, 6, ChessTeams.WHITE, ChessPiece.KNIGHT);
                    BoardGame.Board[7, 7] = PieceFactory.CreatePiece(7, 7, ChessTeams.WHITE, ChessPiece.ROOK);


                    return BoardGame;

                default:
                    return null;

            }
        }


    }


    public enum BoardGameTypes
    {
        CHESS
    }
}

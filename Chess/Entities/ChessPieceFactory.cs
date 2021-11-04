using System;
using System.Collections.Generic;

namespace BoardGame.Core
{
    public class ChessPieceFactory : IPieceFactory
    {
        public IPiece Piece { get; set; }
        public IRuleContainer RuleContainer { get; set; }


        public ChessPieceFactory(IRuleContainer InjRuleContainer)
        {
            RuleContainer = InjRuleContainer;
        }

        public IPiece CreatePiece(int Line, int Column, string Team, ChessPiece Type)
        {
            switch (Type)
            {
                case ChessPiece.PAWN:
                    Piece = new Pawn(Line, Column, Team);
                    Piece.Rules = new List<IRuleContainer.Rule>();
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.PAWN));

                    return Piece;

                case ChessPiece.KNIGHT :
                    Piece = new Knight(Line, Column, Team);
                    Piece.Rules = new List<IRuleContainer.Rule>();
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.KNIGHT));

                    return Piece;

                case ChessPiece.BISHOP :
                    Piece = new Bishop(Line, Column, Team);
                    Piece.Rules = new List<IRuleContainer.Rule>();
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.BISHOP));
                     
                    return Piece;

                case ChessPiece.ROOK :
                    Piece = new Rook(Line, Column, Team);
                    Piece.Rules = new List<IRuleContainer.Rule>();
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.ROOK));

                    return Piece;

                case ChessPiece.QUEEN :
                    Piece = new Queen(Line, Column, Team);
                    Piece.Rules = new List<IRuleContainer.Rule>();
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.BISHOP));
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.ROOK));

                    return Piece;

                case ChessPiece.KING :
                    Piece = new King(Line, Column, Team);
                    Piece.Rules = new List<IRuleContainer.Rule>();
                    Piece.Rules.Add(RuleContainer.GetRule(ChessRule.KING));

                    return Piece;

                default:
                    return null;
            }

        }
    }


    public enum ChessPiece
    {
        PAWN,
        KNIGHT,
        ROOK,
        BISHOP,
        QUEEN,
        KING
           
    }

}

using System;
namespace BoardGame.Core
{
    public interface IPieceFactory
    {
        public IRuleContainer RuleContainer { get; set; }
        public IPiece CreatePiece(int Line, int Column, string Team, ChessPiece Type);

    }
}

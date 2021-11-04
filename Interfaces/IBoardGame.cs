using System;

namespace BoardGame.Core
{
    public interface IBoardGame
    {

       public string Id { get; set; }
       public IPieceFactory PieceFactory { get; set; }
       public IPiece[,] Board { get; set; }
       public Player[] Players { get; set; }
       public Result ValidateMove(Move move); 

    }
}

using System;
namespace BoardGame.Core
{
    public interface IBoardGameFactory
    {
        public IBoardGame CreateBoardGame(BoardGameTypes game);
    }
}

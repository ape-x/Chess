using System;
namespace BoardGame.Core
{
    public class Move
    {

        public int FromLine { get; set; }
        public int FromColumn { get; set; }
        public int ToLine { get; set; }
        public int ToColumn { get; set; }
        public Player Player { get; set; }

    }
}

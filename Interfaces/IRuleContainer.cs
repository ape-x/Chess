using System;
using System.Collections.Generic;

namespace BoardGame.Core
{
    public interface IRuleContainer
    {

        public delegate bool Rule(int Line, int Column, int newLine, int newColumn, IPiece[,] Board);
        public Rule GetRule(Enum rule);

    }
}

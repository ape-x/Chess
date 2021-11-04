using System;
using System.Collections.Generic;

namespace BoardGame.Core
{
    public interface IPiece
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Team { get; set; }
        public List<IRuleContainer.Rule> Rules { get; set; }


        public IPiece Clone();
        public bool ValidateMove(int newLine, int newColumn, IPiece[,] Board);
    }



}

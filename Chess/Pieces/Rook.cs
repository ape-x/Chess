using System;
using System.Collections.Generic;

namespace BoardGame.Core
{
    public class Rook : IPiece
    {

        public List<IRuleContainer.Rule> Rules { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Team { get; set; }


        public Rook(int line, int column, string team)
        {
            Line = line;
            Column = column;
            Team = team;
        }

        public  bool ValidateMove(int newLine, int newColumn, IPiece[,] Board)
        {
            foreach (var rule in Rules)
                if (rule(Line, Column, newLine, newColumn, Board))
                    return true;

            return false;
        }

        public IPiece Clone()
        {
            return new Rook(Line, Column, Team) { Rules = Rules };
        }
    }
}

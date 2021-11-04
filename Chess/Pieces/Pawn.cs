using System;
using System.Collections.Generic;

namespace BoardGame.Core
{
    public class Pawn : IPiece
    {


        public List<IRuleContainer.Rule> Rules { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Team { get; set; }


        public Pawn(int line, int column, string team)
        {
            Team = team;
            Line = line;
            Column = column;

        }

        public bool ValidateMove(int newLine, int newColumn, IPiece[,] Board)
        {
            foreach (var rule in Rules)
                if (rule(Line, Column, newLine, newColumn, Board))
                    return true;

            return false;
        }

        public IPiece Clone()
        {
            return new Pawn(Line, Column, Team) { Rules = Rules };
        }
    }
}

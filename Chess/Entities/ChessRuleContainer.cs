using System;
using System.Collections.Generic;
using static BoardGame.Core.Chess;

namespace BoardGame.Core
{
    public class ChessRuleContainer : IRuleContainer
    {

        public IRuleContainer.Rule GetRule(Enum rule)
        {
            switch (rule)
            {
                case ChessRule.BISHOP :
                    return Bishop;

                case ChessRule.ROOK :
                    return Rook;

                case ChessRule.KNIGHT :
                    return Knight;

                case ChessRule.KING :
                    return King;

                case ChessRule.PAWN :
                    return Pawn;

                default:
                    return null;
            }
        }

        private bool Bishop(int Line, int Column, int newLine, int newColumn, IPiece[,] Board)
        {

            int lineDistance = Line > newLine ? Line - newLine : newLine - Line;
            int columnDistance = Column > newColumn ? Column - newColumn : newColumn - Column;

            //Drumul nu este diagonal
            if (((Line != newLine && Column != newColumn) && (lineDistance != columnDistance)) || (Line == newLine || Column == newColumn))
                return false;

            //Dreapta jos
            if (Line < newLine && Column < newColumn) // Pornim de la +-1 ptr ca suntem deja la Line/Column
                for (int i = Line + 1, j = Column + 1; i < newLine || j < newColumn; i++, j++)
                    if (Board[i, j] != null)
                        return false;

            //Dreapta sus
            if (Line > newLine && Column < newColumn)
                for (int i = Line - 1, j = Column + 1; i > newLine && j < newColumn; i--, j++)
                    if (Board[i, j] != null)
                        return false;

            //Stanga sus
            if (Line > newLine && Column > newColumn)
                for (int i = Line - 1, j = Column - 1; i > newLine && j > newColumn; i--, j--)
                    if (Board[i, j] != null)
                        return false;

            //Stanga jos
            if (Line < newLine && Column > newColumn)
                for (int i = Line + 1, j = Column - 1; i < newLine && j > newColumn; i++, j--)
                    if (Board[i, j] != null)
                        return false;

            return true;
        }

        private bool Rook(int Line, int Column, int newLine, int newColumn, IPiece[,] Board)
        {

            int lower, higher;

            //Drum ilegal
            if (newLine != Line && newColumn != Column)
                return false;


            //Deplasarea are loc pe linii
            if (Line != newLine)
            {
                lower = Line < newLine ? Line : newLine;
                higher = Line > newLine ? Line : newLine;

                for (int i = lower + 1; i < higher; i++)
                    if (Board[i, Column] != null)
                        return false;
            }


            //Deplasarea are loc pe coloane
            if (Column != newColumn)
            {
                lower = Column < newColumn ? Column : newColumn;
                higher = Column > newColumn ? Column : newColumn;

                for (int i = lower + 1; i < higher; i++)
                    if (Board[Line, i] != null)
                        return false;
            }


            return true;
        }

        private bool Knight(int Line, int Column, int newLine, int newColumn, IPiece[,] Board)
        {

            int lineDistance = Line > newLine ? Line - newLine : newLine - Line;
            int columnDistance = Column > newColumn ? Column - newColumn : newColumn - Column;

            int higher = lineDistance > columnDistance ? lineDistance : columnDistance;
            int lower = lineDistance < columnDistance ? lineDistance : columnDistance;

            if (higher != 2 || lower != 1)
                return false;

            return true;
        }

        private bool King(int Line, int Column, int newLine, int newColumn, IPiece[,] Board)
        {
            int lineDistance = Line > newLine ? Line - newLine : newLine - Line;
            int columnDistance = Column > newColumn ? Column - newColumn : newColumn - Column;

            if (lineDistance > 1 || columnDistance > 1)
                return false;

            return true;
        }

        private bool Pawn(int Line, int Column, int newLine, int newColumn, IPiece[,] Board)
        {


            int lineDistance = newLine > Line ? newLine - Line : Line - newLine;
            int columnDistance = newColumn > Column ? newColumn - Column : Column - newColumn;

            //Daca destinatia este ocupata de o piesa inamica, iar miscarea este una LINIARA
            if (Board[newLine, newColumn] != null
                && Column == newColumn)
                return false;

            //Prea multe coloane
            if (columnDistance > 1)
                return false;

            //Destinatia traverseaza doua linii, iar piesa a fost deja mutata
            if (lineDistance != 1 && (Line != 6 && Line != 1))
                return false;

            //Traverseaza prea multe linii sau coloane
            if ((lineDistance > 2 && (Line != 6 && Line != 1)) || columnDistance > 1)
                return false;

            if ((Line == 6 || Line == 1) && lineDistance > 2)
                return false;

            //Piesa schimba coloana, fara sa schimbe linia
            if (newColumn != Column && newLine == Line)
                return false;

            //Destinatia nu respecta directia impusa de echipa piesei
            if ((Board[Line, Column].Team == ChessTeams.WHITE && newLine > Line) || (Board[Line, Column].Team == ChessTeams.BLACK && newLine < Line))
                return false;

            //Ruta ilegala : nu exista un inamic in pozitia respectiva
            if (newColumn != Column && (Board[newLine, newColumn] == null || Board[newLine, newColumn].Team == Board[Line, Column].Team))
                return false;

            return true;
        }

    }


    public enum ChessRule
    {
        PAWN,
        KNIGHT,
        BISHOP,
        ROOK,
        QUEEN,
        KING
    }
}

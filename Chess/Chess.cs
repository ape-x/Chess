using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardGame.Core
{
    public class Chess : IBoardGame
    {

        public string Id { get; set; }
        public IPieceFactory PieceFactory { get; set; }
        public int Turn { get; set; }

        public IPiece[,] Board { get; set; }
        public Player[] Players { get; set; }



        public Result ValidateMove(Move move)
        {

            var result = new Result { Date = DateTime.Now, Game = BoardGameTypes.CHESS.ToString(), GameId = Id, Player = move.Player.Team, Value = ResultValues.INVALID };


            if (!IsPlayerTurn(move.Player))
            {
                result.Message = ChessMessages.IsPlayerTurn;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (!IsDestinationOnBoard(move.FromLine, move.FromColumn, move.ToLine, move.ToColumn))
            {
                result.Message = ChessMessages.IsDestinationOnBoard;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (!DoesPieceExist(move.FromLine, move.FromColumn))
            {
                result.Message = ChessMessages.DoesPieceExist;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (!IsSameTeam(move.FromLine, move.FromColumn, move.Player))
            {
                result.Message = ChessMessages.IsSameTeam;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (IsFriendlyFire(move.FromLine, move.FromColumn, move.ToLine, move.ToColumn))
            {
                result.Message = ChessMessages.IsFriendlyFire;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (IsPieceAtDestination(move.FromLine, move.FromColumn, move.ToLine, move.ToColumn))
            {
                result.Message = ChessMessages.IsPieceAtDestination;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (!Board[move.FromLine, move.FromColumn].ValidateMove(move.ToLine, move.ToColumn, Board))
            {
                result.Message = ChessMessages.ValidateMove;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (MoveResultsInCheck(move.FromLine, move.FromColumn, move.ToLine, move.ToColumn))
            {
                result.Message = ChessMessages.MoveResultsInCheck;
                result.Value = ResultValues.INVALID;
                return result;
            }


            if (MoveResultsInCheckmate())
            {
                result.Message = $"{move.Player.Team} wins chess game {Id}";
                result.Value = ResultValues.FINISH;
                return result;
            }



            Turn += 1;
            if (Turn == Players.Length)
                Turn = 0;

            result.Message = $"{Board[move.ToLine, move.ToColumn].ToString()} moves from [{move.FromLine} {move.FromColumn}] to [{move.ToLine} {move.ToColumn}] ";
            result.Value = ResultValues.VALID;
            return result;

        }


        //GAME RULES
        //-  -   -   -   -   -   -   -   -   -   -   -   -


        private bool MoveResultsInCheckmate()
        {
            if (Turn == 0 && IsCheck(GetKing(ChessTeams.BLACK)).Count > 0)
            {
                if (IsCheckmate(GetKing(ChessTeams.BLACK)))
                    return true;
            }


            else if (Turn == 1 && IsCheck(GetKing(ChessTeams.WHITE)).Count > 0)
            {
                if (IsCheckmate(GetKing(ChessTeams.WHITE)))
                    return true;
            }

            return false;
        }



        private bool MoveResultsInCheck(int fromLine, int fromColumn, int toLine, int toColumn)
        {
            IPiece temp = Board[toLine, toColumn];
            MoveIPiece(fromLine, fromColumn, toLine, toColumn);
            if (Turn == 0)
            {
                if (IsCheck(GetKing(ChessTeams.WHITE)).Count > 0)
                {
                    MoveIPiece(toLine, toColumn, fromLine, fromColumn);
                    Board[toLine, toColumn] = temp;
                    return true;
                }
            }
            else if (Turn == 1)
            {
                if (IsCheck(GetKing(ChessTeams.BLACK)).Count > 0)
                {
                    MoveIPiece(toLine, toColumn, fromLine, fromColumn);
                    Board[toLine, toColumn] = temp;
                    return true;
                }
            }
            return false;
        }


        private bool IsPlayerTurn(Player player)
        {
            if ((Turn == 0 && player.Team != ChessTeams.WHITE) || (Turn == 1 && player.Team != ChessTeams.BLACK))
                return false;
            return true;
        }

        private bool IsDestinationOnBoard(int fromLine, int fromColumn, int toLine, int toColumn)
        {

            if (toLine > 7 || toLine < 0 || toColumn > 7 || toColumn < 0 ||
                fromLine > 7 || fromLine < 0 || fromColumn < 0 || fromColumn > 7)
                return false;

            return true;
        }

        private bool DoesPieceExist(int fromLine, int fromColumn)
        {
            if (Board[fromLine, fromColumn] == null)
                return false;
            return true;
        }

        private bool IsSameTeam(int fromLine, int fromColumn, Player player)
        {
            if (Board[fromLine, fromColumn].Team != player.Team)
                return false;
            return true;
        }

        private bool IsFriendlyFire(int fromLine, int fromColumn, int toLine, int toColumn)
        {
            if (Board[toLine, toColumn] != null && Board[toLine, toColumn].Team == Board[fromLine, fromColumn].Team)
                return true;
            return false;
        }

        private bool IsPieceAtDestination(int fromLine, int fromColumn, int toLine, int toColumn)
        {
            if (fromLine == toLine && fromColumn == toColumn)
                return true;
            return false;
        }


        private IPiece GetKing(string Team)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (Board[i, j] != null)
                        if (Board[i, j].Team == Team && Board[i, j] is King)
                            return Board[i, j];

            return null;
        }

        private List<IPiece> IsCheck(IPiece target)
        {
            var adversaries = new List<IPiece>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (Board[i, j] != null)
                        if (Board[i, j] != target && Board[i, j].Team != target.Team && Board[i, j].ValidateMove(target.Line, target.Column, Board))
                        {
                            Console.WriteLine($"{i} {j} can attack {target.Line} {target.Column}");
                            adversaries.Add(Board[i, j]);
                        }
            return adversaries;
        }





        private bool IsCheckmate(IPiece target)
        {

            if (CanKingMove(target))
                return false;

            if (CanAllyDefendKing(target))
                return false;

            Console.WriteLine("Checkmate");

            return true;
        }


        private bool CanKingMove(IPiece target)
        {
            IPiece temp;
            var possiblePositions = new List<(int l, int c)>();
            for (int i = target.Line - 1, k = target.Line + 1, j = target.Column - 1; j <= target.Column + 1; j++)
            {
                if ((i >= 0 && i <= 7) && (j >= 0 && j <= 7))
                    possiblePositions.Add((i, j));
                if ((k >= 0 && k <= 7) && (j >= 0 && j <= 7))
                    possiblePositions.Add((k, j));
            }

            if (target.Column - 1 >= 0)
                possiblePositions.Add((target.Line, target.Column - 1));

            if (target.Column + 1 <= 7)
                possiblePositions.Add((target.Line, target.Column + 1));


            foreach (var position in possiblePositions)
            {
                if (Board[position.l, position.c] == null)
                {
                    temp = target.Clone();
                    MoveIPiece(target.Line, target.Column, position.l, position.c);

                    if (IsCheck(Board[position.l, position.c]).Count == 0)
                    {
                        MoveIPiece(position.l, position.c, temp.Line, temp.Column);
                        return true;
                    }
                    MoveIPiece(position.l, position.c, temp.Line, temp.Column);
                }

                if (Board[position.l, position.c] != null && Board[position.l, position.c].Team != target.Team)
                {
                    temp = Board[position.l, position.c].Clone();
                    var tempTarget = target.Clone();
                    MoveIPiece(target.Line, target.Column, position.l, position.c);
                    if (IsCheck(Board[position.l, position.c]).Count == 0)
                    {
                        MoveIPiece(position.l, position.c, temp.Line, temp.Column);
                        return true;
                    }
                    MoveIPiece(position.l, position.c, tempTarget.Line, tempTarget.Column);
                    Board[temp.Line, temp.Column] = temp;
                }
            }

            return false;
        }

        private bool CanAllyDefendKing(IPiece target)
        {
            IPiece temp;
            var adversariesList = IsCheck(target);
            if (adversariesList.Count > 1)
                return false;

            var adversary = adversariesList[0];

            var path = GetPath(adversary.Line, adversary.Column, target.Line, target.Column);
            path.Add((adversary.Line, adversary.Column));


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null && Board[i, j].Team == target.Team && Board[i, j] != target)
                    {
                        for (int k = 0; k < path.Count; k++)
                        {
                            if (Board[i, j].ValidateMove(path[k].Item1, path[k].Item2, Board))
                            {

                                temp = Board[i, j].Clone();
                                MoveIPiece(i, j, path[k].Item1, path[k].Item2);

                                if (IsCheck(target).Count == 0)
                                {
                                    MoveIPiece(path[k].Item1, path[k].Item2, temp.Line, temp.Column);

                                    return true;
                                }
                                MoveIPiece(path[k].Item1, path[k].Item2, temp.Line, temp.Column);
                            }
                        }
                    }
                }
            }
            return false;
        }


        //-  -   -   -   -   -   -   -   -   -   -   -   -


        private void MoveIPiece(int fromLine, int fromColumn, int toLine, int toColumn)
        {
            Board[toLine, toColumn] = Board[fromLine, fromColumn];
            Board[toLine, toColumn].Line = toLine;
            Board[toLine, toColumn].Column = toColumn;
            Board[fromLine, fromColumn] = null;
        }



        private List<(int, int)> GetDiagonalPath(int fromLine, int fromColumn, int toLine, int toColumn)
        {
            var Path = new List<(int, int)>();

            int lineDistance = fromLine > toLine ? fromLine - toLine : toLine - fromLine;
            int columnDistance = fromColumn > toColumn ? fromColumn - toColumn : toColumn - fromColumn;

            if (((fromLine != toLine && fromColumn != toColumn) && (lineDistance != columnDistance)) || (fromLine == toLine || fromColumn == toColumn))
                return null;

            if (fromLine < toLine && fromColumn < toColumn)
                for (int i = fromLine + 1, j = fromColumn + 1; i < toLine || j < toColumn; i++, j++)
                    Path.Add((i, j));

            if (fromLine > toLine && fromColumn < toColumn)
                for (int i = fromLine - 1, j = fromColumn + 1; i > toLine && j < toColumn; i--, j++)
                    Path.Add((i, j));

            if (fromLine > toLine && fromColumn > toColumn)
                for (int i = fromLine - 1, j = fromColumn - 1; i > toLine && j > toColumn; i--, j--)
                    Path.Add((i, j));

            if (fromLine < toLine && fromColumn > toColumn)
                for (int i = fromLine + 1, j = fromColumn - 1; i < toLine && j > toColumn; i++, j--)
                    Path.Add((i, j));

            return Path;
        }


        private List<(int, int)> GetLinearPath(int fromLine, int fromColumn, int toLine, int toColumn)
        {
            var Path = new List<(int, int)>();
            int lower, higher;


            if (fromLine != toLine)
            {
                lower = fromLine < toLine ? fromLine : toLine;
                higher = fromLine > toLine ? fromLine : toLine;

                for (int i = lower + 1; i < higher; i++)
                    Path.Add((i, fromColumn));
            }

            if (fromColumn != toColumn)
            {
                lower = fromColumn < toColumn ? fromColumn : toColumn;
                higher = fromColumn > toColumn ? fromColumn : toColumn;

                for (int i = lower + 1; i < higher; i++)
                    Path.Add((fromLine, i));

            }


            return Path;
        }

        private List<(int, int)> GetPath(int fromLine, int fromColumn, int toLine, int toColumn)
        {

            if (fromLine == toLine || fromColumn == toColumn)
                return GetLinearPath(fromLine, fromColumn, toLine, toColumn);

            if (fromLine != toLine && fromColumn != toColumn)
                return GetDiagonalPath(fromLine, fromColumn, toLine, toColumn);

            return null;
        }


        public static class ChessTeams
        {
            public static string WHITE = "WHITE";
            public static string BLACK = "BLACK";
        }


        public static class ChessMessages
        {
            public static string DoesPieceExist = "Piece does not exist";
            public static string IsPlayerTurn = "Currently, it's not that player's turn";
            public static string IsSameTeam = "Player is not allowed to move that piece";
            public static string IsFriendlyFire = "Move would result in friendly fire";
            public static string ValidateMove = "Piece could not be moved there";
            public static string MoveResultsInCheck = "The move would result in a self-checkmate";
            public static string IsDestinationOnBoard = "That destination is not on board";
            public static string IsPieceAtDestination = "Piece is already at destination";
        }


    }
}
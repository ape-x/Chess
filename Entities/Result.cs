using System;
namespace BoardGame.Core
{
    public class Result
    {
        public int Id { get; set; }
        public string Game { get; set; }
        public string GameId { get; set; }
        public string Player { get; set; }
        public DateTime Date { get; set; }
        public ResultValues Value { get; set; }
        public string Message { get; set; }
    }

    public enum ResultValues
    {
        INVALID,
        VALID,
        FINISH
    }


}

using Checkers.Core.Models.Enums;

namespace Checkers.Core.Models
{
    public class Result
    {
        public Player Winner { get; set; }
        public EndReason EndReason { get; set; }

        public Result(Player winner, EndReason endReason) => (Winner, EndReason) = (winner, endReason);

        public static Result Win(Player winner) => new Result(winner, EndReason.Win);

        public static Result Draw(EndReason endReason) => new Result(Player.None, endReason);
    }
}
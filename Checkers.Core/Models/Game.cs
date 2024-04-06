using System.Collections.Generic;

namespace Checkers.Core.Models
{
    public class Game
    {
        public GameBoard Board { get; set; }
        public Player[] Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public IList<Move> History { get; set; }
    }
}
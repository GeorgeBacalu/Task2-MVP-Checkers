using Checkers.Core.Models.Enums;

namespace Checkers.Core.Models
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }

        public GameState(Board board, Player player) => (Board, CurrentPlayer) = (board, player);
    }
}
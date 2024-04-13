using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Core.Models
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }

        public GameState(Board board, Player player) => (Board, CurrentPlayer) = (board, player);

        public IEnumerable<Move> GetPieceLegalMoves(Position from) => Board.IsEmpty(from) || Board[from].Color != CurrentPlayer ? Enumerable.Empty<Move>() : Board[from].GetMoves(from, Board);
    
        public void MakeMove(Move move)
        {
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
        }
    }
}
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
        public Result Result { get; private set; } = null;

        public GameState(Board board, Player player) => (Board, CurrentPlayer) = (board, player);

        public List<Move> GetPieceLegalMoves(Position from) => Board.IsEmpty(from) || Board[from].Color != CurrentPlayer ? new List<Move>() : Board[from].GetMoves(from, Board);
    
        public List<Move> GetPlayerLegalMoves(Player player) => Board.GetPlayerPiecePositions(player).SelectMany(pos => Board[pos].GetMoves(pos, Board)).ToList();

        public void MakeMove(Move move)
        {
            move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();
            CheckEndGame();
        }

        private void CheckEndGame()
        {
            if (!Board.GetPlayerPiecePositions(CurrentPlayer).Any())
                Result = Result.Win(CurrentPlayer.Opponent());
        }

        public bool IsGameOver() => Result != null;
    }
}
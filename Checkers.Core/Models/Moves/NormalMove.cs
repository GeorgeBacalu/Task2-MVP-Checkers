using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Pieces;

namespace Checkers.Core.Models.Moves
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position From { get; }
        public override Position To { get; }

        public NormalMove(Position from, Position to) => (From, To) = (from, to);

        public override void Execute(Board board)
        {
            Piece piece = board[From];
            board[To] = piece;
            board[From] = null;
            piece.HasMoved = true;
        }
    }
}
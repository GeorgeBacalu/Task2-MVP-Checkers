using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Pieces;

namespace Checkers.Core.Models.Moves
{
    public class CaptureMove : Move
    {
        public override MoveType Type => MoveType.Jump;
        public override Position From { get; }
        public override Position To { get; }

        public CaptureMove(Position from, Position to) => (From, To) = (from, to);

        public override void Execute(Board board)
        {
            Piece piece = board[From];
            board[To] = piece;
            board[From] = null;
            board[(To + From) / 2] = null;
            piece.HasMoved = true;
        }
    }
}
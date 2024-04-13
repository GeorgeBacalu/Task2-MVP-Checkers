using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Pieces;

namespace Checkers.Core.Models.Moves
{
    public class PawnPromotionMove : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position From { get; }
        public override Position To { get; }
        public bool IsCapturing { get; }

        public PawnPromotionMove(Position from, Position to, bool isCapturing = false) => (From, To, IsCapturing) = (from, to, isCapturing);

        public override void Execute(Board board)
        {
            Piece piece = board[From];
            board[To] = new King(piece.Color);
            board[From] = null;
            if (IsCapturing) board[(To + From) / 2] = null;
            piece.HasMoved = true;
        }
    }
}
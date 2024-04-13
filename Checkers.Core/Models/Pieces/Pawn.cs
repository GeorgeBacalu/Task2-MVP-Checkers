using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Core.Models.Pieces
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }
        private readonly Direction forward;
        private readonly Direction[] dirs = new Direction[] { Direction.East, Direction.West };

        public Pawn(Player color)
        {
            Color = color;
            forward = Color == Player.White ? Direction.South : Direction.North;
        }

        public override Piece Copy() => new Pawn(Color) { HasMoved = HasMoved };

        private static bool CanMoveTo(Position pos, Board board) => Board.IsInBounds(pos) && board.IsEmpty(pos);

        private bool CanCaptureAt(Position pos, Board board)
        {
            if (Board.IsInBounds(pos) && board.IsEmpty(pos))
                foreach (Direction dir in dirs)
                {
                    Position capturedPos = pos - (forward + dir), capturingPos = pos - 2 * (forward + dir);
                    if (Board.IsInBounds(capturedPos) && Board.IsInBounds(capturingPos) && board[capturedPos]?.Color != Color && board[capturingPos]?.Color == Color)
                        return true;
                }
            return false;
        }

        private IEnumerable<Move> GetNonCapturingMoves(Position from, Board board) => dirs
            .Select(dir => from + forward + dir)
            .Where(to => CanMoveTo(to, board))
            .Select<Position, Move>(to =>
            {
                if (to.Row == (Color == Player.White ? 7 : 0)) return new PawnPromotionMove(from, to);
                else return new NormalMove(from, to);
            });

        private IEnumerable<Move> GetCapturingMoves(Position from, Board board) => dirs
            .Select(dir => from + 2 * (forward + dir))
            .Where(to => CanCaptureAt(to, board))
            .Select<Position, Move>(to =>
            {
                if (to.Row == (Color == Player.White ? 7 : 0)) return new PawnPromotionMove(from, to, true);
                else return new CaptureMove(from, to);
            });

        public override IEnumerable<Move> GetMoves(Position from, Board board) => GetNonCapturingMoves(from, board).Concat(GetCapturingMoves(from, board));
    }
}
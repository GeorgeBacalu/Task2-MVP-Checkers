using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Core.Models.Pieces
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }
        private readonly Direction[] dirs = new Direction[] { Direction.NorthEast, Direction.NorthWest, Direction.SouthEast, Direction.SouthWest };

        public King(Player color) => Color = color;

        public override Piece Copy() => new King(Color) { HasMoved = HasMoved };

        private static bool CanMoveTo(Position pos, Board board) => Board.IsInBounds(pos) && board.IsEmpty(pos);

        private bool CanCaptureAt(Position pos, Board board)
        {
            if (Board.IsInBounds(pos) && board.IsEmpty(pos))
                foreach (Direction dir in dirs)
                {
                    Position capturedPos = pos - dir, capturingPos = pos - 2 * dir;
                    if (Board.IsInBounds(capturedPos) && Board.IsInBounds(capturingPos) && board[capturedPos]?.Color != Color && board[capturingPos]?.Color == Color)
                        return true;
                }
            return false;
        }

        private IEnumerable<Move> GetNonCapturingMoves(Position from, Board board) => dirs
            .Select(dir => from + dir)
            .Where(to => CanMoveTo(to, board))
            .Select<Position, Move>(to => new NormalMove(from, to));

        private IEnumerable<Move> GetCapturingMoves(Position from, Board board) => dirs
            .Select(dir => from + 2 * dir)
            .Where(to => CanCaptureAt(to, board))
            .Select<Position, Move>(to => new CaptureMove(from, to));

        public override IEnumerable<Move> GetMoves(Position from, Board board) => GetNonCapturingMoves(from, board).Concat(GetCapturingMoves(from, board));
    }
}
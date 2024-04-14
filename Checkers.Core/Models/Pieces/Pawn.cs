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
            if (!Board.IsInBounds(pos) || !board.IsEmpty(pos)) return false;
            foreach (Direction dir in dirs)
            {
                Position over = pos - (forward + dir), from = pos - 2 * (forward + dir);
                if (Board.IsInBounds(over) && Board.IsInBounds(from) && board[over] != null && board[from] != null && board[over].Color != Color && board[from].Color == Color)
                    return true;
            }
            return false;
        }

        private List<Move> GetNormalMoves(Position from, Board board) => dirs
            .Select(dir => from + forward + dir)
            .Where(to => CanMoveTo(to, board))
            .Select<Position, Move>(to =>
            {
                if (to.Row == (Color == Player.White ? 7 : 0)) return new PawnPromotionMove(from, to);
                else return new NormalMove(from, to);
            })
            .ToList();

        private List<Move> GetJumpMoves(Position from, Board board) => dirs
            .Select(dir => from + 2 * (forward + dir))
            .Where(to => CanCaptureAt(to, board))
            .Select<Position, Move>(to =>
            {
                if (to.Row == (Color == Player.White ? 7 : 0)) return new PawnPromotionMove(from, to, true);
                else return new JumpMove(from, to);
            })
            .ToList();

        private List<Move> GetMultipleJumpMoves(Position from, Position current, List<Position> visited, Board board, List<Move> jumps)
        {
            foreach (Direction dir in dirs)
            {
                Position to = current + 2 * (forward + dir), over = current + forward + dir;
                if (!visited.Contains(to) && Board.IsInBounds(over) && Board.IsInBounds(to) && board[over] != null && board[to] == null && board[over].Color != Color)
                {
                    List<Position> newVisited = new List<Position>(visited) { to };
                    if (to.Row == (Color == Player.White ? 7 : 0))
                    {
                        jumps.Add(new PawnPromotionMove(from, to, true));
                        return jumps;
                    }
                    else jumps.Add(new JumpMove(from, to));
                    GetMultipleJumpMoves(from, to, newVisited, board, jumps);
                }
            }
            return jumps;
        }

        public override List<Move> GetMoves(Position from, Board board, bool allowMultipleJumps)
        {
            List<Move> moves = GetNormalMoves(from, board);
            moves.AddRange(allowMultipleJumps ? GetMultipleJumpMoves(from, from, new List<Position> { from }, board, new List<Move>()) : GetJumpMoves(from, board));
            return moves;
        }
    }
}
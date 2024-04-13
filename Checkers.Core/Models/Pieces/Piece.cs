using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Moves;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Core.Models.Pieces
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected IEnumerable<Position> GetMovesInDirection(Position from, Board board, Direction dir)
        {
            for (Position pos = from + dir; Board.IsInBounds(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }
                Piece piece = board[pos];
                if (piece.Color != Color) yield return pos;
                yield break;
            }
        }

        //protected IEnumerable<Position> GetMovesInDirection(Position from, Board board, Direction dir)
        //{
        //    var reachablePositions = new List<Position>();
        //    for (Position pos = from + dir; Board.IsInBounds(pos); pos += dir)
        //    {
        //        if (board.IsEmpty(pos)) reachablePositions.Add(pos);
        //        else
        //        {
        //            Piece piece = board[pos];
        //            if (piece.Color != Color) reachablePositions.Add(pos);
        //            break;
        //        }
        //    }
        //    return reachablePositions;
        //}

        protected IEnumerable<Position> GetMovesInDirections(Position from, Board board, Direction[] dirs) => dirs.SelectMany(dir => GetMovesInDirection(from, board, dir));
    }
}
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
        private readonly Direction[] directions = new Direction[] { Direction.NorthEast, Direction.NorthWest, Direction.SouthEast, Direction.SouthWest };

        public King(Player color) => Color = color;

        public override Piece Copy() => new King(Color) { HasMoved = HasMoved };

        public override IEnumerable<Move> GetMoves(Position from, Board board) => GetMovesInDirections(from, board, directions).Select(to => new NormalMove(from, to));
    }
}
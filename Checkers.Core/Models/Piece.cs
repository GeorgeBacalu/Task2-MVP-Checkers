using Checkers.Core.Models.Enums;

namespace Checkers.Core.Models
{
    public class Piece
    {
        public Color Color { get; set; }
        public PieceType Type { get; set; }
        public Position Position { get; set; }
    }
}
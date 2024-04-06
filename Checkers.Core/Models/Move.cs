using System.Collections.Generic;

namespace Checkers.Core.Models
{
    public class Move
    {
        public Piece Piece { get; set; }
        public Position From { get; set; }
        public Position To { get; set; }
        public IList<Piece> CapturedPieces { get; set; }
    }
}
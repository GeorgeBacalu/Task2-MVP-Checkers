using Checkers.Core.Models.Enums;
using System.Collections.Generic;

namespace Checkers.Core.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public IList<Piece> Pieces { get; set; }
    }
}
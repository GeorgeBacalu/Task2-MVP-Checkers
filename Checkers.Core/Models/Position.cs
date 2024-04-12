using Checkers.Core.Models.Enums;
using System;

namespace Checkers.Core.Models
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column) => (Row, Column) = (row, column);

        public Player GetSquareColor() => (Row + Column) % 2 == 0 ? Player.White : Player.Red;

        public override bool Equals(object obj) => obj is Position p && Row == p.Row && Column == p.Column;

        public override int GetHashCode() => Tuple.Create(Row, Column).GetHashCode();

        public static bool operator ==(Position p1, Position p2) => p1.Equals(p2);

        public static bool operator !=(Position p1, Position p2) => !(p1 == p2);

        public static Position operator +(Position p, Direction d) => new Position(p.Row + d.RowDelta, p.Column + d.ColumnDelta);
    }
}
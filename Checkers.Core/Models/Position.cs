using Checkers.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace Checkers.Core.Models
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column) => (Row, Column) = (row, column);

        public Player GetSquareColor() => (Row + Column) % 2 == 0 ? Player.White : Player.Red;

        public bool IsInBounds() => Row >= 0 && Row < 8 && Column >= 0 && Column < 8;

        public override bool Equals(object obj) => obj is Position p && Row == p.Row && Column == p.Column;

        public override int GetHashCode() => Tuple.Create(Row, Column).GetHashCode();

        public static bool operator ==(Position p1, Position p2) => EqualityComparer<Position>.Default.Equals(p1, p2);

        public static bool operator !=(Position p1, Position p2) => !(p1 == p2);

        public static Position operator +(Position p1, Position p2) => new Position(p1.Row + p2.Row, p1.Column + p2.Column);

        public static Position operator /(Position p, int scalar) => new Position(p.Row / scalar, p.Column / scalar);

        public static Position operator +(Position p, Direction d) => new Position(p.Row + d.RowDelta, p.Column + d.ColumnDelta);

        public static Position operator -(Position p, Direction d) => new Position(p.Row - d.RowDelta, p.Column - d.ColumnDelta);
    }
}
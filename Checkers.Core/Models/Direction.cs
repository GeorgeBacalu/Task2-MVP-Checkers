namespace Checkers.Core.Models
{
    public class Direction
    {
        public int RowDelta { get; set; }
        public int ColumnDelta { get; set; }

        public readonly static Direction North = new Direction(-1, 0), South = new Direction(1, 0), East = new Direction(0, 1), West = new Direction(0, -1);
        public readonly static Direction NorthEast = North + East, NorthWest = North + West, SouthEast = South + East, SouthWest = South + West;

        public Direction(int rowDelta, int columnDelta) => (RowDelta, ColumnDelta) = (rowDelta, columnDelta);

        public static Direction operator +(Direction d1, Direction d2) => new Direction(d1.RowDelta + d2.RowDelta, d1.ColumnDelta + d2.ColumnDelta);

        public static Direction operator *(int scalar, Direction d) => new Direction(scalar * d.RowDelta, scalar * d.ColumnDelta);
    }
}
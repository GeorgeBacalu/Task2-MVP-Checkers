namespace Checkers.Core.Models
{
    public class GameBoard
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Square[,] Squares { get; set; } = new Square[8, 8];
    }
}
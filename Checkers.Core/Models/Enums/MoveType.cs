namespace Checkers.Core.Models.Enums
{
    public enum MoveType
    {
        Normal, // the piece moves one square diagonally forward. If it reaches the opposite end of the board and becomes a "king", it can move backward as well.
        King, // the king moves one square diagonally both forward and backward.
        Jump, // a single capture where a piece jumps over an opposing piece directly in its diagonal path and removes it from the board.
        MultipleJump, // a series of consecutive captures, potentially through successive jumps over opposing pieces.
        PawnPromotion // a pawn reaches the opposite end of the board and becomes a king.
    }
}
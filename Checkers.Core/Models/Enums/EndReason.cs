namespace Checkers.Core.Models.Enums
{
    public enum EndReason
    {
        Win, // a player wins the game by capturing all of the opponent's pieces.
        ThreefoldRepetition, // if the same position is repeated three times with the same player to move, the game can be declared a draw.
        FiftyMoveRule, // If during the last fifty moves by each player, no pieces have been captured, and no pawn has been moved, the game can be declared a draw.
        InsufficientMaterial, // if neither player has sufficient pieces to force a win (both players only have one piece left, and they cannot corner each other).
    }
}
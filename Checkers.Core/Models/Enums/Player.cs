namespace Checkers.Core.Models.Enums
{
    public enum Player { None, White, Red }

    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player) => player == Player.White ? Player.Red : player == Player.Red ? Player.White : Player.None;
    }
}
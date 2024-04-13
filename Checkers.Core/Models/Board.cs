using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Pieces;
using System;

namespace Checkers.Core.Models
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        public Piece this[int r, int c]
        {
            get => IsInBounds(new Position(r, c)) ? pieces[r, c] : throw new ArgumentOutOfRangeException($"Invalid position ({r}, {c})");
            set => pieces[r, c] = IsInBounds(new Position(r, c)) ? value : throw new ArgumentOutOfRangeException($"Invalid position ({r}, {c})");
        }

        public Piece this[Position p] { get => this[p.Row, p.Column]; set => this[p.Row, p.Column] = value; }

        public static Board Init()
        {
            Board board = new Board();
            board.InitPlayer(Player.White);
            board.InitPlayer(Player.Red);
            return board;
        }

        private void InitPlayer(Player player)
        {
            int[] pieceRows = player == Player.White ? new[] { 0, 1, 2 } : new[] { 5, 6, 7 };
            foreach (int r in pieceRows)
                for (int c = 0; c < pieces.GetLength(1); ++c)
                    if ((r + c) % 2 == 1)
                        this[r, c] = new Pawn(player);
        }

        public static bool IsInBounds(Position p) => p.Row >= 0 && p.Row < 8 && p.Column >= 0 && p.Column < 8;

        public bool IsEmpty(Position p) => this[p] == null;
    }
}
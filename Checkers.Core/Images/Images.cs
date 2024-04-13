using Checkers.Core.Models.Enums;
using Checkers.Core.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Checkers.Core.Images
{
    public static class Images
    {
        private static readonly IDictionary<PieceType, ImageSource> whiteSources = new Dictionary<PieceType, ImageSource>()
        { { PieceType.Pawn, LoadImage("Assets/WhitePawn.png") }, { PieceType.King, LoadImage("Assets/WhiteKing.png") }};
        private static readonly IDictionary<PieceType, ImageSource> redSources = new Dictionary<PieceType, ImageSource>()
        { { PieceType.Pawn, LoadImage("Assets/RedPawn.png") }, { PieceType.King, LoadImage("Assets/RedKing.png") }};
        public static readonly Cursor whiteCursor = LoadCursor("Assets/WhiteCursor.cur");
        public static readonly Cursor blackCursor = LoadCursor("Assets/BlackCursor.cur");

        private static ImageSource LoadImage(string filePath) => new BitmapImage(new Uri(filePath, UriKind.Relative));

        private static Cursor LoadCursor(string filePath) => new Cursor(Application.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream, true);

        private static ImageSource GetImage(Player player, PieceType type) => player == Player.White ? whiteSources[type] : redSources[type];

        public static ImageSource GetImage(Piece piece) => piece == null ? null : GetImage(piece.Color, piece.Type);
    }
}
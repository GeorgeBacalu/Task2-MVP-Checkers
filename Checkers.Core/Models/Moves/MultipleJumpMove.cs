using Checkers.Core.Models.Enums;
using System.Collections.Generic;

namespace Checkers.Core.Models.Moves
{
    public class MultipleJumpMove : Move
    {
        public override MoveType Type => MoveType.MultipleJump;
        public override Position From { get; }
        public override Position To { get; }
        public List<Move> Jumps { get; }

        public MultipleJumpMove(Position from, Position to, List<Move> jumps) => (From, To, Jumps) = (from, to, new List<Move>(jumps));

        public override void Execute(Board board) => Jumps.ForEach(jump => jump.Execute(board));
    }
}
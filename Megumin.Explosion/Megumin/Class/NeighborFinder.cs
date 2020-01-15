using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Megumin
{
    public class NeighborFinder<T> 
        where T : IXZ<int>
    {
        public IEnumerable<T> all { get; set; }

        public T Center { get; set; }

        public T NeighborUpRight { get { return all.FirstOrDefault(t => (t.X == Center.X + 1) && (t.Z == Center.Z + 1)); } }
        public T NeighborDownRight { get { return all.FirstOrDefault(t => (t.X == Center.X + 1) && (t.Z == Center.Z - 1)); } }
        public T NeighborDownLeft { get { return all.FirstOrDefault(t => (t.X == Center.X - 1) && (t.Z == Center.Z - 1)); } }
        public T NeighborUpLeft { get { return all.FirstOrDefault(t => (t.X == Center.X - 1) && (t.Z == Center.Z + 1)); } }
        public T NeighborUp { get { return all.FirstOrDefault(t => (t.X == Center.X) && (t.Z == Center.Z + 1)); } }
        public T NeighborRight { get { return all.FirstOrDefault(t => (t.X == Center.X + 1) && (t.Z == Center.Z)); } }
        public T NeighborDown { get { return all.FirstOrDefault(t => (t.X == Center.X) && (t.Z == Center.Z - 1)); } }
        public T NeighborLeft { get { return all.FirstOrDefault(t => (t.X == Center.X - 1) && (t.Z == Center.Z)); } }
    
        public T this[Sudoku sudoku]
        {
            get
            {
                switch (sudoku)
                {
                    case Sudoku.Up:
                        return NeighborUp;
                    case Sudoku.RightUp:
                        return NeighborUpRight;
                    case Sudoku.Right:
                        return NeighborRight;
                    case Sudoku.RightDown:
                        return NeighborDownRight;
                    case Sudoku.Down:
                        return NeighborDown;
                    case Sudoku.LeftDown:
                        return NeighborDownLeft;
                    case Sudoku.Left:
                        return NeighborLeft;
                    case Sudoku.LeftUp:
                        return NeighborUpLeft;
                    case Sudoku.Center:
                        return Center;
                    default:
                        break;
                }

                return default;
            }
        }
    }
}

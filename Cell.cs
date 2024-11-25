using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes
{
    public class Cell
    {
        public int x { get; set; }
        public int y { get; set; }

        public Cell? North { get; set; }
        public Cell? East { get; set; }
        public Cell? West { get; set; }
        public Cell? South { get; set; }


        public Cell(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public List<Cell> LinkedCells()
        {
            List<Cell> neighbors = new List<Cell>();

            if (North != null)
            {
                neighbors.Add(North);
            }
            if (South != null)
            {
                neighbors.Add(South);
            }
            if (East != null)
            {
                neighbors.Add(East);
            }
            if (West != null)
            {
                neighbors.Add(West);
            }

            return neighbors;
        }

        // Override Equals
        public override bool Equals(object? obj)
        {
            if (obj is Cell other)
            {
                return this.x == other.x && this.y == other.y;
            }
            return false;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}

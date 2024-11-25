using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes
{
    public class Maze
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private Cell[,] maze;

        public Maze()
        {
            this.Width = 10;
            this.Height = 10;
            maze = new Cell[Width, Height];
            CreateGrid();
        }

        public Maze(int w, int h)
        {
            this.Width = w;
            this.Height = h;
            maze = new Cell[Width, Height];
            GenerateMaze();
        }

        public Cell GetCell(int x, int y)
        {
            return maze[x, y];
        }

        public Cell[,] GetAllCells()
        {
            return maze;
        }

        private void CreateGrid()
        {
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    maze[x,y] = new Cell(x,y);
                }
            }
        }

        private void GenerateMaze()
        {
            CreateGrid();
            // kruskal's algorithm
            List<List<Tuple<int, int>>> horiz_walls = new(); // for horizontal walls
            List<List<Tuple<int, int>>> vert_walls = new(); // for vertical walls
            UnionFind uf = new UnionFind(Width * Height); // uf ds for tracking connected cell set
            for (int x = 0; x < Height; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    if (y < Width - 1)
                    {
                        var vert_tmp = new List<Tuple<int, int>>();
                        vert_tmp.Add(new Tuple<int, int>(x, y));
                        vert_tmp.Add(new Tuple<int, int>(x, y + 1));
                        vert_walls.Add(vert_tmp);
                    }

                    if (x < Height - 1)
                    {
                        var horiz_tmp = new List<Tuple<int, int>>();
                        horiz_tmp.Add(new Tuple<int, int>(x, y));
                        horiz_tmp.Add(new Tuple<int, int>(x + 1, y));
                        horiz_walls.Add(horiz_tmp);
                    }
                }
            }

            List<List<Tuple<int,int>>> walls = horiz_walls.Concat(vert_walls).ToList();
            // shuffle the walls
            walls = Shuffle(walls);

            foreach (var wall in walls)
            {
                int c1 = wall[0].Item1 * Height + wall[0].Item2;
                int c2 = wall[1].Item1 * Height + wall[1].Item2;
                
                if (uf.Find(c1) != uf.Find(c2))
                {
                    // remove the wall b/w c1 and c2
                    var cell1 = maze[wall[0].Item1, wall[0].Item2];
                    var cell2 = maze[wall[1].Item1, wall[1].Item2];

                    RemoveWall(cell1, cell2);
                    // union the cell components
                    uf.Union(c1, c2);
                }
            }
        }

        public void RemoveWall(Cell cell1, Cell cell2)
        {
            if (cell1.x == cell2.x)
            {
                // same x, y varies
                if (cell1.y < cell2.y)
                {
                    cell1.East = cell2;
                    cell2.West = cell1;
                }
                else
                {
                    cell1.West = cell2;
                    cell2.East = cell1;
                }
            }
            else if (cell1.y == cell2.y)
            {
                // same y, x varied
                if (cell1.x < cell2.x)
                {
                    cell1.South = cell2;
                    cell2.North = cell1;
                }
                else
                {
                    cell1.North = cell1;
                    cell2.South = cell2;
                }
            }
        }

        private List<T> Shuffle<T>(List<T> values)
        {
            Random random = new Random();
            // fischer yates algorithm
            int n = values.Count;
            while (n > 1)
            {
                n -= 1;
                int k = random.Next(n + 1);
                T value = values[k];
                values[k] = values[n];
                values[n] = value;
            }
            return values;
        }

    }
}

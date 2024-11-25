namespace Mazes
{
    public partial class Form1 : Form
    {
        public int MazeWidth = 10;
        public int MazeHeight = 10;
        public Maze? FormMaze = null;
        public Form1()
        {
            InitializeComponent();
            numericUpDown1.Value = MazeWidth;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MazeWidth = (int)numericUpDown1.Value;
            MazeHeight = (int)numericUpDown1.Value;
        }

        private void generateNewMazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maze newMaze = new Maze(MazeWidth, MazeHeight);
            // Clear any previous controls in the TableLayoutPanel
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = MazeHeight;
            tableLayoutPanel1.ColumnCount = MazeWidth;

            // Configure the TableLayoutPanel to resize dynamically
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            tableLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;


            for (int i = 0; i < MazeHeight; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute,
                                                tableLayoutPanel1.Height / newMaze.Height));
            }
            for (int i = 0; i < MazeWidth; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
                                                    tableLayoutPanel1.Width / newMaze.Width));
            }

            // Add labels to the TableLayoutPanel to represent the maze grid
            for (int y = 0; y < MazeHeight; y++)
            {
                for (int x = 0; x < MazeWidth; x++)
                {
                    Cell cell = newMaze.GetCell(y, x); // Get the current cell from the maze
                    Panel cellPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.White, // Default background color
                        Margin = Padding.Empty,
                    };
                    if (x == 0 && y == 0)
                    {
                        cellPanel.BackColor = Color.Green;
                    }
                    if (x == MazeHeight - 1 && y == MazeWidth - 1)
                    {
                        cellPanel.BackColor = Color.Red;
                    }


                    // Add a Paint event to draw custom borders for the walls
                    cellPanel.Paint += (sender, e) =>
                    {
                        Graphics g = e.Graphics;
                        int thickness = 2; // Thickness of the wall
                        Pen wallPen = new Pen(Color.Black, thickness); // Pen for drawing walls

                        // Calculate the rectangle bounds
                        int width = cellPanel.Width;
                        int height = cellPanel.Height;



                        // Draw walls based on the cell's properties
                        if (cell.North == null)
                        {
                            g.DrawLine(wallPen, 0, 0, width, 0);
                        }

                        if (cell.West == null)
                        {
                            g.DrawLine(wallPen, 0, 0, 0, height);
                        }
                    };
                    tableLayoutPanel1.Controls.Add(cellPanel, x, y);
                }
            }

            FormMaze = newMaze;

        }

        private void bFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormMaze == null)
            {
                MessageBox.Show("Generate a maze first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Reset();
            // solve maze by bfs
            Cell startCell = FormMaze.GetCell(0, 0);
            Cell EndCell = FormMaze.GetCell(MazeHeight - 1, MazeWidth - 1);

            Queue<Tuple<Cell, List<Cell>>> queue = new Queue<Tuple<Cell, List<Cell>>>();
            var startTuple = new Tuple<Cell, List<Cell>>(startCell, new List<Cell> { startCell });
            queue.Enqueue(startTuple);
            HashSet<Cell> visited = new();

            while (queue.Count > 0)
            {
                var cellTuple = queue.Dequeue();
                Cell currCell = cellTuple.Item1;
                List<Cell> currPath = cellTuple.Item2;
                // color explored cell
                Panel currCellPanel = (Panel)tableLayoutPanel1.GetControlFromPosition(currCell.y, currCell.x);
                currCellPanel.BackColor = Color.LightCyan;

                if (currCell == EndCell)
                {
                    // found the path
                    foreach (var cell in currPath)
                    {
                        int x = cell.x;
                        int y = cell.y;
                        Panel cellPanel = (Panel)tableLayoutPanel1.GetControlFromPosition(y, x);
                        cellPanel.BackColor = Color.Blue;
                    }
                    return;
                }
                List<Cell> links = currCell.LinkedCells();
                foreach (var neighbor in currCell.LinkedCells())
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        List<Cell> newPath = new List<Cell>(currPath);
                        newPath.Add(neighbor);
                        queue.Enqueue(new Tuple<Cell, List<Cell>>(neighbor, newPath));
                    }
                }
            }
            // no path found
            MessageBox.Show("No path found.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void dFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormMaze == null)
            {
                MessageBox.Show("Generate a maze first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Reset();
            // solve maze by dfs
            Cell startCell = FormMaze.GetCell(0, 0);
            Cell EndCell = FormMaze.GetCell(MazeHeight - 1, MazeWidth - 1);
            Tuple<Cell, List<Cell>> startTuple = new Tuple<Cell, List<Cell>>(startCell, new List<Cell> { startCell });
            Stack<Tuple<Cell, List<Cell>>> stack = new Stack<Tuple<Cell, List<Cell>>>();
            stack.Push(startTuple);
            HashSet<Cell> visited = new();

            while (stack.Any())
            {
                var currCellTuple = stack.Pop();
                Cell currCell = currCellTuple.Item1;
                List<Cell> currPath = currCellTuple.Item2;
                // color explored cell
                Panel currCellPanel = (Panel)tableLayoutPanel1.GetControlFromPosition(currCell.y, currCell.x);
                currCellPanel.BackColor = Color.LightCyan;

                if (currCell == EndCell)
                {
                    // found the path, color it
                    foreach (var cell in currPath)
                    {
                        int x = cell.x;
                        int y = cell.y;
                        Panel cellPanel = (Panel)tableLayoutPanel1.GetControlFromPosition(y, x);
                        cellPanel.BackColor = Color.Blue;
                    }
                    return;
                }
                foreach (var neighbor in currCell.LinkedCells())
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        List<Cell> newPath = new List<Cell>(currPath);
                        newPath.Add(neighbor);
                        stack.Push(new Tuple<Cell, List<Cell>>(neighbor, newPath));
                    }
                }
            }
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            foreach (var cell in FormMaze.GetAllCells())
            {
                int x = cell.x;
                int y = cell.y;

                Panel cellPanel = (Panel)tableLayoutPanel1.GetControlFromPosition(y, x);
                if (x == 0 && y == 0)
                {
                    cellPanel.BackColor = Color.Green;
                }
                else if (x == MazeHeight - 1 && y == MazeWidth - 1)
                {
                    cellPanel.BackColor = Color.Red;
                }
                else
                {
                    cellPanel.BackColor = Color.White;
                }
            }
        }
    }
}

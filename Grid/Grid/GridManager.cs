using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Grid
{
    public class GridManager
    {
        //Handeling of graphics
        private BufferedGraphics backBuffer;
        private Graphics dc;
        private Rectangle displayRectangle;

        public List<Cell> ClosedList { get; set; }
        public List<Cell> OpenList { get; set; }

        /// <summary>
        /// Amount of rows in the grid
        /// </summary>
        private int cellRowCount;

        /// <summary>
        /// This list contains all cells and open list
        /// </summary>
        private Cell[,] grid;

        //  private Astar aStar;

        /// <summary>
        /// The current click type
        /// </summary>
        private CellType clickType;

        public Cell[,] Grid { get { return grid; } }

        //public BufferedGraphics BackBuffer { get => backBuffer; set => backBuffer = value; }
        //public Graphics Dc { get => dc; set => dc = value; }
        //public Rectangle DisplayRectangle { get => displayRectangle; set => displayRectangle = value; }

        public GridManager(Graphics dc, Rectangle displayRectangle)
        {
            //Create's (Allocates) a buffer in memory with the size of the display
            this.backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);

            //Sets the graphics context to the graphics in the buffer
            this.dc = backBuffer.Graphics;

            //Sets the displayRectangle
            this.displayRectangle = displayRectangle;

            //Sets the row count to then, this will create a 10 by 10 grid.
            cellRowCount = 10;

            CreateGrid(10, 10);
            //aStar = new Astar();
            //Console.WriteLine("{0}{1}{2}");
        }

        public Cell this[int x, int y]
        {
            get
            {
                return Grid[x, y];
            }
            set
            {
                Grid[x, y] = value;
                x = value.Position.X;
                y = value.Position.Y;
            }
        }

        /// <summary>
        /// Renders all the cells
        /// </summary>
        public void Render()
        {
            dc.Clear(Color.White);

            foreach (Cell cell in grid)
            {
                cell.Render(dc);
            }

            //Renders the content of the buffered graphics context to the real context(Swap buffers)
            backBuffer.Render();
        }

        /// <summary>
        /// Creates the grid
        /// </summary>
        public void CreateGrid(int width, int height)
        {
            //Instantiates the list of cells
            grid = new Cell[width, height];

            //Sets the cell size
            int cellSize = displayRectangle.Width / cellRowCount;

            //Creates all the cells
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    this[x, y] = new Cell(new Point(x, y), cellSize);
                }
            }
            CreateOpenList();
        }

        public void CalculateG(Cell input)
        {
            for (int a = Grid[input.Position.X, input.Position.Y].Position.X - 1; a <= Grid[input.Position.X, input.Position.Y].Position.X + 1; a++)
            {
                if (a < Grid.GetLowerBound(0))
                    a++;
                else if (a > Grid.GetUpperBound(0))
                    break;
                for (int b = Grid[input.Position.X, input.Position.Y].Position.Y - 1; b <= Grid[input.Position.X, input.Position.Y].Position.Y + 1; b++)
                {
                    if (b < Grid.GetLowerBound(1))
                        b++;
                    else if (b > Grid.GetUpperBound(1))
                    {
                        break;
                    }
                        
                    if (a != Grid[input.Position.X, input.Position.Y].Position.X || b != Grid[input.Position.X, input.Position.Y].Position.Y)
                    {
                        //Sees if nodes un-diagonal next to current node are walkable
                        if (a == Grid[input.Position.X, input.Position.Y].Position.X && b == Grid[input.Position.X, input.Position.Y].Position.Y - 1 || a == Grid[input.Position.X, input.Position.Y].Position.X - 1 && b == Grid[input.Position.X, input.Position.Y].Position.Y
                            || a == Grid[input.Position.X, input.Position.Y].Position.X && b == Grid[input.Position.X, input.Position.Y].Position.Y || a == Grid[input.Position.X, input.Position.Y].Position.X + 1 && b == Grid[input.Position.X, input.Position.Y].Position.Y)
                        {
                            Grid[a, b].G = 10; // Returns the linear value of 10 to the G-score of the current node
                            if (!OpenList.Contains(Grid[a, b]) || ClosedList.Contains(Grid[a, b]))
                            {
                                OpenList.Add(Grid[a, b]);
                            }
                        }
                        //Otherwise, if diagonal, return G-score of 14
                        else
                        {
                            Grid[a, b].G = 14;
                            if (!OpenList.Contains(Grid[a, b]) || ClosedList.Contains(Grid[a, b]))
                            {
                                OpenList.Add(Grid[a, b]);
                            }
                        }
                    }
                }
            }
        }

        public void CalculateH(Cell endInput)
        {
            int xConst = 0;
            int yConst = 0;
            for (int a = Grid[endInput.Position.X, endInput.Position.Y].Position.X; a >= Grid.GetLowerBound(0); a--)
            {
                for (int b = Grid[endInput.Position.X, endInput.Position.Y].Position.Y; b >= Grid.GetLowerBound(1); b--)
                {
                    if (a != Grid[endInput.Position.X, endInput.Position.Y].Position.X || b != Grid[endInput.Position.X, endInput.Position.Y].Position.Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                    yConst++;
                }
                xConst++;
                yConst = 0;
            }
            xConst = 0;
            yConst = 0;
            for (int a = Grid[endInput.Position.X, endInput.Position.Y].Position.X; a >= Grid.GetLowerBound(0); a--)
            {
                for (int b = Grid[endInput.Position.X, endInput.Position.Y].Position.Y; b <= Grid.GetUpperBound(1); b++)
                {
                    if (a != Grid[endInput.Position.X, endInput.Position.Y].Position.X || b != Grid[endInput.Position.X, endInput.Position.Y].Position.Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                    yConst++;

                }
                xConst++;
                yConst = 0;

            }
            xConst = 0;
            yConst = 0;
            for (int a = Grid[endInput.Position.X, endInput.Position.Y].Position.X; a <= Grid.GetUpperBound(0); a++)
            {
                for (int b = Grid[endInput.Position.X, endInput.Position.Y].Position.Y; b >= Grid.GetLowerBound(1); b--)
                {
                    if (a != Grid[endInput.Position.X, endInput.Position.Y].Position.X || b != Grid[endInput.Position.X, endInput.Position.Y].Position.Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                    yConst++;

                }
                xConst++;
                yConst = 0;
            }
            xConst = 0;
            yConst = 0;
            for (int a = Grid[endInput.Position.X, endInput.Position.Y].Position.X; a <= Grid.GetUpperBound(0); a++)
            {
                for (int b = Grid[endInput.Position.X, endInput.Position.Y].Position.Y; b <= Grid.GetUpperBound(1); b++)
                {
                    if (a != Grid[endInput.Position.X, endInput.Position.Y].Position.X || b != Grid[endInput.Position.X, endInput.Position.Y].Position.Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                    yConst++;
                }
                xConst++;
                yConst = 0;
            }
        }

        /// <summary>
        /// If the mouse clicks on a cell
        /// </summary>
        /// <param name="mousePos"></param>
        public void ClickCell(Point mousePos)
        {
            foreach (Cell cell in grid) //Finds the cell that we just clicked
            {
                if (cell.BoundingRectangle.IntersectsWith(new Rectangle(mousePos, new Size(1, 1))))
                {
                    cell.Click(ref clickType);
                }
            }
        }

        /// <summary>
        /// Creates an open list based on the grid in the game.
        /// </summary>
        public void CreateOpenList()
        {
            OpenList = new List<Cell>();
            //  bool isSTART = Grid.GetType().IsEnum.Equals(CellType.START);
            //  foreach (Cell cell in Grid)
            //{
            //     if (cell.GetType().IsEnum == isSTART)
            //          OpenList.Add(cell); // puts start cell in the open list
            //}

            Console.WriteLine(OpenList);
            Console.Write(OpenList.Capacity); //Debug
        }

        public void CreateClosedList()
        {
            ClosedList = new List<Cell>();
        }

        public void AStar(Cell start, Cell end)
        {
            Cell current = null;
            Cell parent = null;
            CreateClosedList();
            Grid[start.Position.X, start.Position.Y].G = 0;
            OpenList.Add(start);
            CalculateH(end);

            while (OpenList.Count > 0) //When open list is not empty
            {
                if (OpenList.Contains(start))
                {
                    current = start;
                    ClosedList.Add(start);
                    OpenList.Remove(start);
                }

                if (current == end)
                {
                    GeneratePath(parent, current);
                    OpenList.Clear();
                    ClosedList.Clear();

                    // for (int i = 0; i > 0; i++)

                    break;
                }

                CalculateG(current); //Checks the neighboring cells of current cell
                foreach (Cell cell in OpenList) //score of the nodes is calculated
                {
                    if (current == null || cell.F <= current.F && current != parent)
                    {
                        current = cell;
                    }
                }

                OpenList.Remove(current);
                ClosedList.Add(current);
                parent = current;
            }

        }

        public void GeneratePath(Cell parent, Cell current)
        {
            SolidBrush b = new SolidBrush(Color.Red);
            foreach (Cell cell in ClosedList)
            {
                cell.Sprite = Image.FromFile(@"Images\tower.png");
            }
        }

    }
}

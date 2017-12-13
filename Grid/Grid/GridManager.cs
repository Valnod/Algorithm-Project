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

        private Astar aStar;

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

            CreateGrid(10,10);
            aStar = new Astar();
            Console.WriteLine("{0}{1}{2}");
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
            grid = new Cell[width,height];

            //Sets the cell size
            int cellSize = displayRectangle.Width / cellRowCount;

            //Creates all the cells
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    this[x,y] = new Cell(new Point(x, y), cellSize);
                }
            }
            CreateOpenList();
        }

        public void CalculateG(Cell input)
        {
            for (int a = Grid[input.X, input.Y].X - 1; a < Grid[input.X, input.Y].X + 1 && a < Grid.GetUpperBound(0) && a > Grid.GetLowerBound(0); a++)
            {
                for (int b = Grid[input.X, input.Y].Y - 1; b < Grid[input.X, input.Y].Y + 1 && b < Grid.GetUpperBound(1) && b > Grid.GetLowerBound(1); b++)
                {
                    if (a != Grid[input.X, input.Y].X && Grid[input.X, input.Y].Walkable || b != Grid[input.X, input.Y].Y && Grid[input.X, input.Y].Walkable)
                    {
                        if (a == Grid[input.X, input.Y].X && b == Grid[input.X, input.Y].Y - 1 || a == Grid[input.X, input.Y].X - 1 && b == Grid[input.X, input.Y].Y
                            || a == Grid[input.X, input.Y].X && b == Grid[input.X, input.Y].Y || a == Grid[input.X, input.Y].X + 1 && b == Grid[input.X, input.Y].Y)
                        {
                            Grid[a, b].G = 10;
                            if (!OpenList.Contains(Grid[a, b]))
                            {
                                OpenList.Add(Grid[a, b]);
                            }
                        }
                        else
                        {
                            Grid[a, b].G = 14;
                            if (!OpenList.Contains(Grid[a, b]))
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
            for (int a = Grid[endInput.X, endInput.Y].X - xConst; a > Grid.GetLowerBound(0); xConst++)
            {
                for (int b = Grid[endInput.X, endInput.Y].Y - yConst; b > Grid.GetLowerBound(1); yConst++)
                {
                    if (a != Grid[endInput.X, endInput.Y].X || b != Grid[endInput.X, endInput.Y].Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                }
            }
            xConst = 0;
            yConst = 0;
            for (int a = Grid[endInput.X, endInput.Y].X - xConst; a > Grid.GetLowerBound(0); xConst++)
            {
                for (int b = Grid[endInput.X, endInput.Y].Y + yConst; b < Grid.GetUpperBound(1); yConst++)
                {
                    if (a != Grid[endInput.X, endInput.Y].X || b != Grid[endInput.X, endInput.Y].Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                }
            }
            xConst = 0;
            yConst = 0;
            for (int a = Grid[endInput.X, endInput.Y].X + xConst; a > Grid.GetUpperBound(0); xConst++)
            {
                for (int b = Grid[endInput.X, endInput.Y].Y - yConst; b > Grid.GetLowerBound(1); yConst++)
                {
                    if (a != Grid[endInput.X, endInput.Y].X || b != Grid[endInput.X, endInput.Y].Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                }
            }
            xConst = 0;
            yConst = 0;
            for (int a = Grid[endInput.X, endInput.Y].X + xConst; a > Grid.GetUpperBound(0); xConst++)
            {
                for (int b = Grid[endInput.X, endInput.Y].Y + yConst; b < Grid.GetUpperBound(1); yConst++)
                {
                    if (a != Grid[endInput.X, endInput.Y].X || b != Grid[endInput.X, endInput.Y].Y)
                    {
                        Grid[a, b].H = 10 * (xConst + yConst);
                    }
                }
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
<<<<<<< HEAD

        /// <summary>
        /// Creates an open list based on the grid in the game.
        /// </summary>
        public void CreateOpenList()
        {
            OpenList = new List<Cell>();
            bool isSTART = Grid.GetType().IsEnum.Equals(CellType.START);
            foreach (Cell cell in Grid)
            {
                if (cell.GetType().IsEnum == isSTART)
                    OpenList.Add(cell); // puts start cell in the open list
            }

            Console.WriteLine(OpenList);
            Console.Write(OpenList.Capacity); //Debug
        }


        public void CreateClosedList()
        {
            ClosedList = new List<Cell>();

        }

        public Cell AStar(Cell start, Cell end)
        {
            Cell best = null;
            Cell parent = null;
            CreateClosedList();
            CalculateH(end);
            Grid[start.X, start.Y].G = 0;
            ClosedList.Add(start);
            
            parent = start;
            while(OpenList.Count > 0)
            {
                if(best == end)
                {
                    return GeneratePath(parent, best);
                }
                if (OpenList.Contains(start))
                {
                    OpenList.Remove(start);
                }
                
                CalculateG(parent);
                foreach (Cell cell in OpenList)
                {
                    if (best == null || cell.F < best.F)
                    {
                        best = cell;
                    }
                }
                ClosedList.Add(best);
                OpenList.Remove(best);
                parent = best;
            }
            
        }

        public Cell GeneratePath(Cell parent, Cell best)
        {

            
        }


=======
>>>>>>> c5e3c0adba5da362d48216cc54b27f8373c8e9f4
    }
}

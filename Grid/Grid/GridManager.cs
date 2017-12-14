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

        Image portal = Image.FromFile(@"Images\portalA.png"); //Portal
        Image wizard = Image.FromFile(@"Images\wizardFront.png"); //Startobject
        Image tower1 = Image.FromFile(@"Images\tower.png"); //Tower1
        Image tower2 = Image.FromFile(@"Images\portalB.png"); //Endobject
        Image key1 = Image.FromFile(@"Images\key.png"); //First key
        Image key2 = Image.FromFile(@"Images\key.png"); //Second key
        Image tree = Image.FromFile(@"Images\treeSingle.png"); //Tree
        Image trees = Image.FromFile(@"Images\tree.png"); //Multible trees

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

        Random rnd = new Random();
        int randomValueX;
        int randomValue1;
        int randomValue2;

        /// <summary>
        /// The current click type
        /// </summary>
        //private CellType clickType;

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
            //aStar = new Astar();
            //Console.WriteLine("{0}{1}{2}");
            randomValueX = rnd.Next(0, 9);
            randomValue1 = rnd.Next(0, 1);
            randomValue2 = rnd.Next(3, 4);
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
                        //Sees if nodes un-diagonal next to current node are walkable
                        if (a == Grid[input.X, input.Y].X && b == Grid[input.X, input.Y].Y - 1 || a == Grid[input.X, input.Y].X - 1 && b == Grid[input.X, input.Y].Y
                            || a == Grid[input.X, input.Y].X && b == Grid[input.X, input.Y].Y || a == Grid[input.X, input.Y].X + 1 && b == Grid[input.X, input.Y].Y)
                        {
                            Grid[a, b].G = 10; // Returns the linear value of 10 to the G-score of the current node
                            if (!OpenList.Contains(Grid[a, b]))
                            {
                                OpenList.Add(Grid[a, b]);
                            }
                        }
                        //Otherwise, if diagonal, return G-score of 14
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
            //TODO: Find celltype of endInput, e.g.: Grid[x,y].myType -- (myType is CellType)
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
            return null; //TODO: something
        }

        public Cell GeneratePath(Cell parent, Cell best)
        {
            throw new NotImplementedException();//herpaderp
            
        }

        /// <summary>
        /// Create objects
        /// </summary>
        /// <param name="mousePos"></param>
        public void SpriteCell()
        {
            grid[1, 7].sprite = portal;
            grid[2, 7].sprite = wizard;
            grid[2, 2].sprite = tower1;
            grid[8, 5].sprite = tower2;
            grid[randomValueX, randomValue1].sprite = key1;
            grid[randomValueX, randomValue2].sprite = key2;
            grid[4, 5].sprite = trees;
            grid[5, 5].sprite = trees;

            //grid[2, 6].Click(ref clickType); //Wizard
            //grid[2, 3].Click(ref clickType); //Tower1
            //grid[rnd.Next(0, 10), rnd.Next(0, 10)].Click(ref clickType);
        }

        ///// <summary>
        ///// If the mouse clicks on a cell
        ///// </summary>
        ///// <param name="mousePos"></param>
        //public void ClickCell(Point mousePos)
        //{
        //    foreach (Cell cell in grid) //Finds the cell that we just clicked
        //    {
        //        if (cell.BoundingRectangle.IntersectsWith(new Rectangle(mousePos, new Size(1, 1))))
        //        {
        //            cell.Click(ref clickType);
        //        }
        //    }
        //}
    }
}

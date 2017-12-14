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

        Image wizard = Image.FromFile(@"Images\wizardFrontUpdate.png"); //Startobject
        Image portal = Image.FromFile(@"Images\PortalUpdate.png"); //Portal
        Image tower1 = Image.FromFile(@"Images\Tower1Update.png"); //Tower1
        Image tower2 = Image.FromFile(@"Images\Tower2Update.png"); //Endobject
        Image key1 = Image.FromFile(@"Images\Key1Update.png"); //First key
        Image key2 = Image.FromFile(@"Images\Key2Update.png"); //Second key
        Image tree = Image.FromFile(@"Images\TreeUpdate.png"); //Tree
        Image trees = Image.FromFile(@"Images\TreesUpdate.png"); //Multible trees
        Image wall = Image.FromFile(@"Images\WallUpdate.png"); //Wall
        Image path = Image.FromFile(@"Images\PathUpdate.png"); //Path
        Image monster = Image.FromFile(@"Images\threeEyeUpdate.png"); //Monster

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
        int randomValueY;
        int randomValueX2;
        int randomValueY2;

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
            randomValueY = rnd.Next(0);
            randomValueX2 = rnd.Next(6, 9);
            randomValueY2 = rnd.Next(1, 4);
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

            grid[0, 0].Render(dc); grid[0, 1].Render(dc); grid[0, 2].Render(dc); grid[0, 3].Render(dc); grid[0, 4].Render(dc); grid[0, 5].Render(dc); grid[0, 6].Render(dc); grid[0, 7].Render(dc);
            grid[1, 0].Render(dc); grid[1, 1].Render(dc); grid[1, 2].Render(dc); grid[1, 3].Render2(dc); grid[1, 4].Render2(dc); grid[1, 5].Render2(dc); grid[1, 6].Render2(dc); grid[1, 7].Render(dc);
            grid[2, 0].Render(dc); grid[2, 1].Render(dc); grid[2, 2].Render(dc); grid[2, 3].Render2(dc); grid[2, 4].Render(dc); grid[2, 5].Render(dc); grid[2, 6].Render2(dc); grid[2, 7].Render(dc);
            grid[3, 0].Render2(dc); grid[3, 1].Render2(dc); grid[3, 2].Render2(dc); grid[3, 3].Render2(dc); grid[3, 4].Render(dc); grid[3, 5].Render(dc); grid[3, 6].Render2(dc); grid[3, 7].Render(dc);
            grid[4, 0].Render2(dc); grid[4, 1].Render2(dc); grid[4, 2].Render2(dc); grid[4, 3].Render2(dc); grid[4, 4].Render2(dc); grid[4, 5].Render(dc); grid[4, 6].Render2(dc); grid[4, 7].Render(dc);
            grid[5, 0].Render2(dc); grid[5, 1].Render2(dc); grid[5, 2].Render2(dc); grid[5, 3].Render2(dc); grid[5, 4].Render2(dc); grid[5, 5].Render(dc); grid[5, 6].Render2(dc); grid[5, 7].Render(dc);
            grid[6, 0].Render2(dc); grid[6, 1].Render2(dc); grid[6, 2].Render2(dc); grid[6, 3].Render2(dc); grid[6, 4].Render(dc); grid[6, 5].Render(dc); grid[6, 6].Render2(dc); grid[6, 7].Render(dc);
            grid[7, 0].Render(dc); grid[7, 1].Render(dc); grid[7, 2].Render(dc); grid[7, 3].Render2(dc); grid[7, 4].Render(dc); grid[7, 5].Render(dc); grid[7, 6].Render2(dc); grid[7, 7].Render(dc);
            grid[8, 0].Render(dc); grid[8, 1].Render(dc); grid[8, 2].Render(dc); grid[8, 3].Render2(dc); grid[8, 4].Render2(dc); grid[8, 5].Render2(dc); grid[8, 6].Render2(dc); grid[8, 7].Render(dc);
            grid[9, 0].Render(dc); grid[9, 1].Render(dc); grid[9, 2].Render(dc); grid[9, 3].Render(dc); grid[9, 4].Render(dc); grid[9, 5].Render(dc); grid[9, 6].Render(dc); grid[9, 7].Render(dc);

            //foreach (Cell cell in grid)
            //{
            //    cell.Render(dc);
            //}

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
            grid[1, 7].sprite = wizard;
            grid[0, 7].sprite = portal;
            grid[2, 2].sprite = tower1;
            grid[9, 5].sprite = tower2;
            grid[randomValueX, randomValueY].sprite = key1;
            grid[randomValueX2, randomValueY2].sprite = key2;
            grid[2, 5].sprite = trees; grid[3, 5].sprite = trees; grid[4, 5].sprite = trees; grid[5, 5].sprite = trees; grid[6, 5].sprite = trees; grid[7, 5].sprite = trees;
            grid[2, 7].sprite = trees; grid[3, 7].sprite = trees; grid[4, 7].sprite = trees; grid[5, 7].sprite = trees; grid[6, 7].sprite = trees; grid[7, 7].sprite = trees;
            grid[4, 4].sprite = wall; grid[4, 3].sprite = wall; grid[4, 2].sprite = wall; grid[4, 1].sprite = wall;
            grid[5, 4].sprite = wall; grid[5, 3].sprite = wall; grid[5, 2].sprite = wall; grid[5, 1].sprite = wall;
            grid[6, 6].sprite = monster;
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

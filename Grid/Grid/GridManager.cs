using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Stopwatch sw = new Stopwatch();

        public Image wizard = Image.FromFile(@"Images\wizardFrontUpdate.png"); //Startobject
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


        Random rnd = new Random();
        public int randomValueX;
        public int randomValueY;
        public int randomValueX2;
        public int randomValueY2;
        private CellType clickType;
        private bool unwalkablePathOne;
        private bool unwalkablePathTwo;
        private bool unwalkablePathThree;

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

            CreateGrid(10, 10);
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
            CreateClosedList();
        }

        /// <summary>
        /// Calculates the G score, checking for neighbouring cells to an input cell, to calculate the quickest route
        /// </summary>
        /// <param name="input">The cell input of the current destination in the path</param>
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

                    if ((a != Grid[input.Position.X, input.Position.Y].Position.X || b != Grid[input.Position.X, input.Position.Y].Position.Y) && Grid[a, b].Walkable)
                    {
                        //Sees if nodes un-diagonal next to current node are walkable
                        if (a == Grid[input.Position.X, input.Position.Y].Position.X && b == Grid[input.Position.X, input.Position.Y].Position.Y - 1 || a == Grid[input.Position.X, input.Position.Y].Position.X - 1 && b == Grid[input.Position.X, input.Position.Y].Position.Y
                            || a == Grid[input.Position.X, input.Position.Y].Position.X && b == Grid[input.Position.X, input.Position.Y].Position.Y || a == Grid[input.Position.X, input.Position.Y].Position.X + 1 && b == Grid[input.Position.X, input.Position.Y].Position.Y)
                        {
                            Grid[a, b].G = 10; // Returns the linear value of 10 to the G-score of the current node
                            if (!OpenList.Contains(Grid[a, b]) && !ClosedList.Contains(Grid[a, b]))
                            {
                                OpenList.Add(Grid[a, b]);
                            }
                        }
                        //Otherwise, if diagonal, return G-score of 14
                        else
                        {
                            Grid[a, b].G = 14;
                            if (!OpenList.Contains(Grid[a, b]) && !ClosedList.Contains(Grid[a, b]))
                            {
                                OpenList.Add(Grid[a, b]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the Heuristic score which estimates the shortest likely route to destination
        /// </summary>
        /// <param name="endInput">The cell of the end-destination</param>
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

        /// <summary>
        /// A* Algorithm. Calculates shortest path from start point to end point
        /// </summary>
        /// <param name="start">The start point to calculate from</param>
        /// <param name="end">The destination to calculate pathfinding for</param>
        public void AStar(Cell start, Cell end)
        {
            Cell current = null;
            Cell parent = null;

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
                    foreach (Cell cell in Grid)
                    {
                        cell.G = 0;
                        cell.H = 0;
                    }

                    // for (int i = 0; i > 0; i++)

                    break;
                }

                CalculateG(current); //Checks the neighboring cells of current cell
                foreach (Cell cell in OpenList) //score of the nodes is calculated
                {
                    if (current == null || cell.F <= current.F)
                    {
                        current = cell;
                    }
                }
                if (current == parent)
                {
                    bool swapped;
                    do
                    {
                        swapped = false;
                        for (int i = 1; i < OpenList.Count; i++)
                        {
                            if (OpenList[i - 1].F > OpenList[i].F)
                            {
                                Cell temp = OpenList[i - 1];
                                OpenList[i - 1] = OpenList[i];
                                OpenList[i] = temp;
                                swapped = true;
                            }
                        }
                    } while (swapped);
                    current = OpenList[0];


                }
                if (current.Position.X == 2 && current.Position.Y == 6)
                {
                    unwalkablePathOne = true;
                }
                if (current.Position.X == 5 && current.Position.Y == 6)
                {
                    unwalkablePathTwo = true;
                }
                if (current.Position.X == 7 && current.Position.Y == 6)
                {
                    unwalkablePathThree = true;
                }
                OpenList.Remove(current);
                ClosedList.Add(current);
                parent = current;

                if (unwalkablePathOne && unwalkablePathThree && unwalkablePathTwo)
                {
                    for (int i = 2; i <= 7; i++)
                    {
                        Grid[i, 6].Walkable = false;
                    }
                    grid[6, 6].sprite = monster;
                }
            }
        }

        public void GeneratePath(Cell parent, Cell current) //Draws the path that's been found from the closed list
        {
           // List<Cell> oldCell = new List<Cell>(); : for use if we want to clear previous cell.
            SolidBrush b = new SolidBrush(Color.Red);
            foreach (Cell cell in ClosedList)
            {
                cell.sprite = Image.FromFile(@"Images\RedX.png");
                Render();
            }
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
            grid[8, 5].sprite = tower2;
            grid[randomValueX, randomValueY].sprite = key1;
            grid[randomValueX2, randomValueY2].sprite = key2;
            grid[2, 5].sprite = trees; grid[3, 5].sprite = trees; grid[4, 5].sprite = trees; grid[5, 5].sprite = trees; grid[6, 5].sprite = trees; grid[7, 5].sprite = trees;
            grid[2, 7].sprite = trees; grid[3, 7].sprite = trees; grid[4, 7].sprite = trees; grid[5, 7].sprite = trees; grid[6, 7].sprite = trees; grid[7, 7].sprite = trees;
            grid[4, 4].sprite = wall; grid[4, 3].sprite = wall; grid[4, 2].sprite = wall; grid[4, 1].sprite = wall;
            grid[5, 4].sprite = wall; grid[5, 3].sprite = wall; grid[5, 2].sprite = wall; grid[5, 1].sprite = wall;
            // grid[6, 6].sprite = monster;

            foreach (Cell cell in Grid)
            {
                if (cell.Sprite == wall || cell.Sprite == tree || cell.Sprite == trees)
                    cell.Walkable = false;
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
    }
}

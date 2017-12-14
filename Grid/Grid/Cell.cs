using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grid.CellType;

namespace Grid
{
    public enum CellType { START, TOWER1, TOWER2, KEY1, KEY2, WALL, EMPTY };

    public class Cell
    {
        /// <summary>
        /// The grid position of the cell
        /// </summary>
        private Point position;

        public int F
        {
            get
            {
                return G + H;
            }
        }
        public int G { get; set; }
        public int H { get; set; }
        public bool Walkable { get; set; } = true;


        /// <summary>
        /// The size of the cell
        /// </summary>
        private int cellSize;

        public int CellSize
        {
            get { return cellSize; }
            set { cellSize = value; }
        }

        /// <summary>
        /// The cell's sprite
        /// </summary>
        public Image sprite;

        private Image grasTile = Image.FromFile(@"Images\groundTileUpdate.png"); //Default
        private Image pathTile = Image.FromFile(@"Images\PathUpdate.png"); //Path

        public Image Sprite { get { return sprite; } set { sprite = value; } }

        /// <summary>
        /// Sets the celltype to empty as default
        /// </summary>
        public CellType myType = EMPTY;

        //TODO: Maybe make myType as property so A* can check it

        public Point Position { get { return position; } set { position = value; } }

        /// <summary>
        /// The bounding rectangle of the cell
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(position.X * cellSize, position.Y * cellSize, cellSize, cellSize);
            }
        }

        /// <summary>
        /// The cell's constructor
        /// </summary>
        /// <param name="position">The cell's grid position</param>
        /// <param name="size">The cell's size</param>
        public Cell(Point position, int size)
        {
            //Sets the position
            this.position = position;

            //Sets the cell size
            this.cellSize = size;
        }

        /// <summary>
        /// Renders the default cell background
        /// </summary>
        /// <param name="dc">The graphic context</param>
        public void Render(Graphics dc)
        {
            //Draws the rectangles color
            dc.FillRectangle(new SolidBrush(Color.White), BoundingRectangle);

            dc.DrawImage(grasTile, BoundingRectangle); //Gives the cells the background

            //Draws the rectangles border
            dc.DrawRectangle(new Pen(Color.Black), BoundingRectangle);

            //If the cell has a sprite, then we need to draw it
            if (sprite != null)
            {
                dc.DrawImage(sprite, BoundingRectangle);
            }

            //Write's the cells grid position
            dc.DrawString(string.Format("{0}", position), new Font("Arial", 7, FontStyle.Regular), new SolidBrush(Color.Black), position.X * cellSize, (position.Y * cellSize) + 10);
        }

        /// <summary>
        /// Renders the path cell background
        /// </summary>
        /// <param name="dc">The graphic context</param>
        public void Render2(Graphics dc)
        {
            //Draws the rectangles color
            dc.FillRectangle(new SolidBrush(Color.White), BoundingRectangle);

            dc.DrawImage(pathTile, BoundingRectangle); //Gives the cells the background

            //Draws the rectangles border
            dc.DrawRectangle(new Pen(Color.Black), BoundingRectangle);

            //If the cell has a sprite, then we need to draw it
            if (sprite != null)
            {
                dc.DrawImage(sprite, BoundingRectangle);
            }

            //Write's the cells grid position
            dc.DrawString(string.Format("{0}", position), new Font("Arial", 7, FontStyle.Regular), new SolidBrush(Color.Black), position.X * cellSize, (position.Y * cellSize) + 10);
        }

        /// <summary>
        /// Fra lærernes originale eksempel. Bliver ikke rigtig brugt
        /// </summary>
        /// <param name="clickType">The click type</param>
        public void Click(ref CellType clickType)
        {
            if (clickType == START) //If the click type is START
            {
                //sprite = Image.FromFile(@"Images\wizardFront.png");
                myType = clickType;
                clickType = TOWER1;
            }
            else if (clickType == TOWER1 && myType != START) //If the click type is GOAL
            {
                //sprite = Image.FromFile(@"Images\tower.png");
                myType = TOWER1;
                clickType = TOWER2;
            }
            else if (clickType == TOWER2 && myType != START && myType != TOWER1 && myType != TOWER2) //If the click type is WALL
            {
                //sprite = Image.FromFile(@"Images\portalB.png");
                myType = TOWER2;
                clickType = KEY1;
            }
            else if (clickType == KEY1 && myType != START && myType != TOWER1 && myType != TOWER2 && myType != KEY1)
            {
                //sprite = Image.FromFile(@"Images\key.png");
                myType = KEY1;
                clickType = KEY2;
            }
            else if (clickType == WALL && myType == WALL) //If the click type is WALL
            {
                sprite = null;
                myType = EMPTY;
            }
        }
    }
}

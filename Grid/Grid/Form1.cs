using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grid
{
    public partial class Form1 : Form
    {
        private GridManager visualManager;

        bool first = true;
        private bool second = false;
        private bool third = false;
        private bool fourth = false;

        public Form1()
        {
            InitializeComponent();

            //Sets the client size
            ClientSize = new Size(750, 600);

            //Instantiates the visual manager
            visualManager = new GridManager(CreateGraphics(), this.DisplayRectangle);

            visualManager.SpriteCell();
        }

        private void Loop_Tick(object sender, EventArgs e)
        {
            //Draws all our cells
            visualManager.Render();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //Checks if we clicked a cell

            //visualManager.ClickCell(this.PointToClient(Cursor.Position));
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // if it is right click
            {
                if (first == true)
                {
                    //From default start position to randomly placed key's position
                    visualManager.AStar(visualManager.Grid[1, 7], visualManager.Grid[visualManager.randomValueX, visualManager.randomValueY]);
                    visualManager.Grid[visualManager.randomValueX, visualManager.randomValueY].sprite = visualManager.wizard;
                    first = false;
                    second = true;
                }

                else if(second == true)
                {
                    //From randomly placed key's position to default tower 1 position
                    visualManager.AStar(visualManager.Grid[visualManager.randomValueX, visualManager.randomValueY], 
                        visualManager.Grid[2, 2]);
                    visualManager.Grid[2, 2].sprite = visualManager.wizard;
                    second = false; ;
                    third = true;
                }

                else if (third == true)
                {
                    //From default tower 1 position to random key 2 position
                    visualManager.AStar(visualManager.Grid[2,2],
                        visualManager.Grid[visualManager.randomValueX2, visualManager.randomValueY2]);
                    visualManager.Grid[visualManager.randomValueX2, visualManager.randomValueY2].sprite = visualManager.wizard;
                    third = false;
                    fourth = true;
                }

                else if (fourth == true)
                {
                    //From key 2's random position to final tower
                    visualManager.AStar(visualManager.Grid[visualManager.randomValueX2, visualManager.randomValueY2], visualManager.Grid[8, 5]);
                    visualManager.Grid[8, 5].sprite = visualManager.wizard;
                    fourth = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

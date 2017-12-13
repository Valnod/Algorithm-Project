using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    class Astar
    {
        #region Variables
       
        /// <summary>
        /// Closed list with confirmed candidates for the pathfinding
        /// </summary>
        private List<Cell> closedList;

        /// <summary>
        /// List of currently discovered nodes.
        /// </summary>
        private List<Cell> openList;
#endregion

        #region Properties
        //F(x) = G(x) + H(x)
        public int FScore { get; set; }
        public int GScore { get; set; }
        public int HScore { get; set; }

        public List<Cell> ClosedList { get { return closedList; } set { closedList = value; } }
        public List<Cell> OpenList { get { return openList; } set { openList = value; } }
#endregion

        public Astar()
        {
           // CreateClosedList();
            CreateOpenList();
        }

        #region Methods
        
        
        ///// <summary>
        ///// creates a closed list based on grid in the game
        ///// </summary>
        //public void CreateClosedList()
        //{
        //    closedList = new List<Cell>();
        //    closedList = gridManager.Grid; // assigns the grid layout to closed list
        //}
#endregion
    }
}

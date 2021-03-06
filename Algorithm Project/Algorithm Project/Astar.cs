﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Grid
//{
//    class Astar
//    {
//        #region Variables
//        private GridManager gridManager;
//        /// <summary>
//        /// Closed list with confirmed candidates for the pathfinding
//        /// </summary>
//        private List<Node> closedList;

//        /// <summary>
//        /// List of currently discovered nodes.
//        /// </summary>
//        private List<Node> openList;
//        #endregion

//        #region Properties
//        //F(x) = G(x) + H(x)
//        public int FScore { get; set; }
//        public int GScore { get; set; }
//        public int HScore { get; set; }

//        public List<Node> ClosedList { get { return closedList; } set { closedList = value; } }
//        public List<Node> OpenList { get { return openList; } set { openList = value; } }
//        #endregion

//        public Astar()
//        {
//            CreateClosedList();
//            CreateOpenList();
//        }

//        #region Methods
//        /// <summary>
//        /// Creates an open list based on the grid in the game.
//        /// </summary>
//        public void CreateOpenList()
//        {
//            openList = new List<Node>();
//            bool isSTART = gridManager.Grid.GetType().IsEnum.Equals(NodeType.START);
//            foreach (Node n in gridManager.Grid)
//            {
//                if (n.GetType().IsEnum == isSTART)
//                    openList.Add(n); // puts start cell in the open list
//            }

//            Console.WriteLine(openList);
//            Console.Write(openList.Capacity); //Debug
//        }

//        /// <summary>
//        /// creates a closed list based on grid in the game
//        /// </summary>
//        public void CreateClosedList()
//        {
//            closedList = new List<Cell>();
//            closedList = gridManager.Grid; // assigns the grid layout to closed list
//        }
//        #endregion
//    }
//}

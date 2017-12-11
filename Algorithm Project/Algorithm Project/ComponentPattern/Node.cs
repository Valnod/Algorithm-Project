using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Project
{    
    class Node : Component
    {
        public string output;

        /// <summary>
        /// The grid position of the node
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The size of the node
        /// </summary>
        public int nodeSize;

        /// <summary>
        /// The node's sprite
        /// </summary>
        //private Texture2D sprite;

        /// <summary>
        /// The bounding rectangle of the cell
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)position.X * nodeSize, (int)position.Y * nodeSize, nodeSize, nodeSize);
            }
        }

        /// <summary>
        /// The node's constructor
        /// </summary>
        /// <param name="position">The node's grid position</param>
        /// <param name="nodeSize">The node's size</param>
        public Node(GameObject gameObject, Vector2 position, int nodeSize) : base(gameObject)
        {
            //Sets the node's position
            this.position = position;

            //Sets the node's size
            this.nodeSize = nodeSize;

            float results = (position.X * nodeSize) + (position.Y * nodeSize) + 10;
            output = Convert.ToString(results);
        }
    }
}

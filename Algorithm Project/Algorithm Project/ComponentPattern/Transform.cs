using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Algorithm_Project
{
    class Transform : Component
    {
        /// <summary>
        /// The transform's position
        /// </summary>
        public Vector2 position { get; private set; }

        /// <summary>
        /// The constructor of the transform
        /// </summary>
        /// <param name="gameObject">Parent object</param>
        /// <param name="position">The transform's position</param>
        public Transform(GameObject gameObject, Vector2 position) : base(gameObject)
        {
            this.position = position;
        }
    }
}

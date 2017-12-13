using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Project
{
    abstract class Component
    {
        /// <summary>
        /// This component's parent GameObject
        /// </summary>
        public GameObject gameObject { get; private set; }

        /// <summary>
        /// The components constructor
        /// </summary>
        /// <param name="gameObject">The parent GameObject</param>
        public Component(GameObject gameObject)
        {
            //Sets the parent
            this.gameObject = gameObject;
        }
    }
}

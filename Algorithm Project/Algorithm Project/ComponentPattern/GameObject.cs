using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithm_Project
{
    class GameObject
    {
        /// <summary>
        /// The GameObject's transform
        /// </summary>
        public Transform transform { get; private set; }

        /// <summary>
        /// A List that contains all components on this GameObject
        /// </summary>
        private List<Component> components = new List<Component>();

        /// <summary>
        /// The GameObject's constructor
        /// </summary>
        public GameObject()
        {
            //Adds a transform component to the GameObject
            this.transform = new Transform(this, Vector2.Zero);

            AddComponent(transform);    
        }

        /// <summary>
        /// Loads the GameObject's content, this is where we load sounds, sprites etc.
        /// </summary>
        /// <param name="content">The Content form the GameWorld</param>
        public void LoadContent(ContentManager content)
        {
            //Loads all loadable components
            foreach (Component component in components)
            {
                if (component is ILoadable)
                {
                    (component as ILoadable).LoadContent(content);
                }
            }
        }

        public void Update()
        {
            //Update is left blank, we don't need it atm.
        }

        /// <summary>
        /// Draws the GameObject
        /// </summary>
        /// <param name="spriteBatch">The spritebatch from our GameWorld</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Component component in components)
            {
                if (component is IDrawable)
                {
                    (component as IDrawable).Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Adds a component to the GameObject
        /// </summary>
        /// <param name="component">The component to add</param>
        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        /// <summary>
        /// Returns the specified component if it exists
        /// </summary>
        /// <param name="component">The component to find</param>
        /// <returns></returns>
        public Component GetComponent(string component)
        {
            return components.Find(x => x.GetType().Name == component);
        }
    }
}

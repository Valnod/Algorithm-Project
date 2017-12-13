using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Algorithm_Project
{
    class SpriteRenderer : Component, ILoadable, IDrawable
    {
        private Rectangle rectangle;

        private Texture2D sprite;

        private string spriteName;

        private float layerDepth;

        public SpriteRenderer(GameObject gameObject, string spriteName, float layerDepth) : base(gameObject)
        {
            this.spriteName = spriteName;
            this.layerDepth = layerDepth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, gameObject.transform.position, rectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>(spriteName);
            this.rectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }
    }
}

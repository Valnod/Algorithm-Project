using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Algorithm_Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameWorld : Game
    {
        //SpriteFont Font1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle displayRectangle;

        /// <summary>
        /// Amount of rows in the grid
        /// </summary>
        private int tileRowCount;

        /// <summary>
        /// This list contains all nodes
        /// </summary>
        private List<GameObject> grid;

        /// <summary>
        /// Creates a list of GameObjects
        /// </summary>
        private List<GameObject> gameObjects;

        public float deltaTime { get; private set; }

        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        public GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            displayRectangle = GraphicsDevice.PresentationParameters.Bounds;

            //Create the node grid
            tileRowCount = 5;

            //Sets the tile size
            //int tileSize = displayRectangle.Width / tileRowCount;

            //Instantiates the list of GameObjects
            gameObjects = new List<GameObject>();

            //Instantiates the list of nodes
            grid = new List<GameObject>();

            //Adds a GameObject to the game
            GameObject go = new GameObject();

            /*GameObject node = new GameObject();

            node.AddComponent(new SpriteRenderer(node, "groundSingleTile", 1));

            //Creates all the tiles
            for (int x = 0; x <= tileRowCount; x++)
            {
                for (int y = 0; y <= tileRowCount; y++)
                {
                    grid.Add(new Node(node, new Vector2(x, y), tileSize));
                }
            }*/

            CreateGrid();

            //Creates the wizard object
            go.AddComponent(new SpriteRenderer(go, "wizardFront", 1));
            go.transform.position = new Vector2();
            gameObjects.Add(go);

            base.Initialize();
        }

        public void CreateGrid()
        {
            int tileSize = displayRectangle.Width / tileRowCount;

            GameObject node = new GameObject();

            node.AddComponent(new SpriteRenderer(node, "groundSingleTile", 1));

            //Creates all the tiles
            for (int x = 0; x <= tileRowCount; x++)
            {
                for (int y = 0; y <= tileRowCount; y++)
                {
                    node.AddComponent(new Node(node, new Vector2(x, y), tileSize));
                    grid.Add(node);
                    //grid.Add(new Node(node, new Vector2(x, y), tileSize));
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Loads the content for all GameObjects
            foreach (GameObject go in gameObjects)
            {
                go.LoadContent(Content);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Updates all GameObjects
            foreach (GameObject go in gameObjects)
            {
                go.Update();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // TODO: Add your drawing code here

            //Draws all GameObjects
            foreach (GameObject go in gameObjects)
            {
                go.Draw(spriteBatch);
            }

            foreach (GameObject node in grid)
            {
                node.Draw(spriteBatch);
                //Font1 = Content.Load<SpriteFont>("Arial");
                //spriteBatch.DrawString(string.Format(Font1, node.output, node.gameObject.transform.position), node.BoundingRectangle, Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

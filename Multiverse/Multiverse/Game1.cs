using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Multiverse
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.IsFullScreen = false;

        }

        protected override void Initialize()
        {
    
            base.Initialize();
        }

        // coordinate grid
        IsoGrid isoGrid;

        // camera
        Vector2 cameraPosition;
        float scrollSpeed;
        Matrix cameraTransform;
        
        // fonts
        SpriteFont font;

        // cursor
        Cursor cursor;

        // HUD
        List<Button> buttons;

        // path finding
        ReachableArea reach;
        Path path;

        // random seed
        Random random;

        // spacescape
        GridTileRegion tiles;

        // in game objects

        Building dwellBlock;
        Building dwellBlockLarge;
        Building govTower;
        Building Factory;
        Building Arena;

        ModCity modCity;
        ModCity modCity2;
        ModCity homeModCity;

        List<ModCity> modCities;

        DeadObject universe;
        DeadUIObject uiball;

        // city attributes
        Ship trader;
        Ship transport;

        Material metal;
        Material rock;
        Material gems;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // grid & camera
            isoGrid = new IsoGrid(GraphicsDevice,Math.PI/6.0,30,new Vector2(100,100));
            cameraPosition = isoGrid.cameraCentre(new Vector2(11,12),graphics); // initial camera position!
            scrollSpeed = 10.0f;

            // fonts
            font = Content.Load<SpriteFont>("font");

            // cursor
            cursor = new Cursor(Content.Load<Texture2D>("cursor"));

            // HUD
            buttons = new List<Button> { };
            
            // random seed
            random = new Random();

            // spacescape
            tiles = new GridTileRegion(Content.Load<Texture2D>("starTiles"), new Vector2(-1, 0),new Vector2(100,100), isoGrid,random);

            // --- game objects ----------

            // ships
            trader = new Ship("Trading Ship", Content.Load<Texture2D>("cargoShip"));
            transport = new Ship("Transport Ship", Content.Load<Texture2D>("cargoShip"));
            
            // materials
            metal = new Material("Metal");
            rock = new Material("Rock");
            gems = new Material("Gemstones");

            // buildings
            dwellBlock = new Building(Content.Load<Texture2D>("building2"),3,new Vector2(0,0),new Vector2(1,1));
            dwellBlockLarge = new Building(Content.Load<Texture2D>("building5"), 3, new Vector2(0, 0), new Vector2(1, 1));
            govTower = new Building(Content.Load<Texture2D>("building3"), 1, new Vector2(0, 0), new Vector2(3, 3));
            Factory = new Building(Content.Load<Texture2D>("building4"), 2, new Vector2(0, 0), new Vector2(2, 1));
            Arena = new Building(Content.Load<Texture2D>("building1"), 1, new Vector2(0, 0), new Vector2(2, 2));

            // cities
            
            modCity = new ModCity("Customisable City", Content.Load<Texture2D>("smallcity"), Content.Load<Texture2D>("smallcityHigh"), Content.Load<Texture2D>("smallcityClick"), new Vector2(20, 3), isoGrid, new Tuple<float, float, float>(0.33f, 0.33f, 0.34f), new List<Ship> { trader }, new List<Material> { metal }, new List<Tuple<Building, int>> { new Tuple<Building, int>(dwellBlock, 4), new Tuple<Building, int>(dwellBlockLarge, 4), new Tuple<Building, int>(govTower, 1), new Tuple<Building, int>(Arena, 1),new Tuple<Building,int>(Factory,3) }, GraphicsDevice, random);
            modCity2 = new ModCity("Another Customisable City", Content.Load<Texture2D>("smallcity"), Content.Load<Texture2D>("smallcityHigh"), Content.Load<Texture2D>("smallcityClick"), new Vector2(30, 5), isoGrid, new Tuple<float, float, float>(0.33f, 0.33f, 0.34f), new List<Ship> { trader }, new List<Material> { metal }, new List<Tuple<Building, int>> { new Tuple<Building, int>(dwellBlock, 4), new Tuple<Building, int>(dwellBlockLarge, 4), new Tuple<Building, int>(govTower, 1), new Tuple<Building, int>(Arena, 1), new Tuple<Building, int>(Factory, 3) }, GraphicsDevice, random);
            homeModCity = new ModCity("Home Customisable City", Content.Load<Texture2D>("smallcity"), Content.Load<Texture2D>("smallcityHigh"), Content.Load<Texture2D>("smallcityClick"), new Vector2(10, 10), isoGrid, new Tuple<float, float, float>(0.33f, 0.33f, 0.34f), new List<Ship> { trader }, new List<Material> { metal }, new List<Tuple<Building, int>> { new Tuple<Building, int>(dwellBlock, 4), new Tuple<Building, int>(dwellBlockLarge, 4), new Tuple<Building, int>(govTower, 1), new Tuple<Building, int>(Arena, 1), new Tuple<Building, int>(Factory, 3) }, GraphicsDevice, random, true);
            
            modCities = new List<ModCity> { modCity, modCity2,homeModCity };

            universe = new DeadObject(Content.Load<Texture2D>("universe"), new Vector2(50, 44), isoGrid);
            uiball = new DeadUIObject(Content.Load<Texture2D>("rball"), new Vector2(10, 10));


            // accesible region and paths
            reach = new ReachableArea(isoGrid, modCities);
            path = new Path(reach);
        }

        protected override void UnloadContent()
        {
        
        }

        protected override void Update(GameTime gameTime)
        { 
            // check for exit key press
            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                this.Exit();
            }

            // camera scrolling

            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                cameraPosition.X += scrollSpeed;
            }

            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                cameraPosition.X -= scrollSpeed;
            }

            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                cameraPosition.Y -= scrollSpeed;
            }

            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                cameraPosition.Y += scrollSpeed;
            }

            cameraPosition = cursor.Update(cameraPosition,graphics,scrollSpeed);

            // update HUD
            foreach (Button button in buttons)
            {
                button.Update(cursor);
            }

            // update cities and their ships

            

            foreach (ModCity mod in modCities)
            {

                mod.Update(cursor, cameraTransform, buttons, GraphicsDevice, path, isoGrid, homeModCity);

                foreach (Ship shp in mod.ships)
                {
                    shp.Update(homeModCity, isoGrid, gameTime);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
            
            cameraTransform = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0.0f);

            // ----------------------------------------------------------------------------------------------------------

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cameraTransform);

            // draw in-game stuff here...

            isoGrid.Draw(spriteBatch); // grid

            // spacescape
            tiles.Draw(spriteBatch);

            // objects
            
            foreach (ModCity mod in modCities)
            {
                mod.Draw(spriteBatch, font);

                foreach (Ship shp in mod.ships)
                {
                    shp.Draw(spriteBatch, path, GraphicsDevice, isoGrid);
                }
            }

            universe.Draw(spriteBatch);

            spriteBatch.End();

            //------------------------------------------------------------------------------------------------------

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // draw UI here!!!
            

            uiball.Draw(spriteBatch);

            // buttons
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch,font);
            }

            // city HUD when present
            
            foreach (ModCity mod in modCities)
            {
                mod.HUDDraw(spriteBatch, font);
            }

            cursor.Draw(spriteBatch);

            // test text
            spriteBatch.DrawString(font, cursor.position.ToString(), new Vector2(500, 10), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.01f);
            spriteBatch.DrawString(font, cameraPosition.ToString(), new Vector2(500, 30), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.01f);
            //spriteBatch.DrawString(font, city.position.ToString(), new Vector2(500, 50), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.01f);
            //spriteBatch.DrawString(font, city.screenPosition.ToString(), new Vector2(500, 80), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.01f);
           
            spriteBatch.End();

        }
    }
}

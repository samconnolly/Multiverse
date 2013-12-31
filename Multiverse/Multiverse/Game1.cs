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

            graphics.IsFullScreen = true;

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
        City city;
        City otherCity;
        City anotherCity;

        List<City> cities;

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

            // game objects
            trader = new Ship("Trading Ship", Content.Load<Texture2D>("cargoShip"));
            transport = new Ship("Transport Ship", Content.Load<Texture2D>("cargoShip"));
            
            metal = new Material("Metal");
            rock = new Material("Rock");
            gems = new Material("Gemstones");

            city = new City("Home City", Content.Load<Texture2D>("city"), Content.Load<Texture2D>("cityHigh"), Content.Load<Texture2D>("cityClick"),new Vector2(11, 12), isoGrid,new Tuple<float,float,float>(0.1f,0.6f,0.3f),new List<Ship>{trader,transport},new List<Material>{rock,metal},true);
            otherCity = new City("Other City", Content.Load<Texture2D>("city"), Content.Load<Texture2D>("cityHigh"), Content.Load<Texture2D>("cityClick"), new Vector2(25, 27), isoGrid, new Tuple<float, float, float>(0.22f, 0.5f, 0.28f), new List<Ship> { trader }, new List<Material> { metal, gems });
            anotherCity = new City("Another City", Content.Load<Texture2D>("city"), Content.Load<Texture2D>("cityHigh"), Content.Load<Texture2D>("cityClick"), new Vector2(3, 24), isoGrid, new Tuple<float, float, float>(0.2f, 0.4f, 0.4f), new List<Ship> { transport }, new List<Material> { gems });

            cities = new List<City> { city, otherCity, anotherCity };


            universe = new DeadObject(Content.Load<Texture2D>("universe"), new Vector2(50, 44), isoGrid);
            uiball = new DeadUIObject(Content.Load<Texture2D>("rball"), new Vector2(10, 10));


            // accesible region and paths
            reach = new ReachableArea(isoGrid, cities);
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

            foreach (City cty in cities)
            {
                cty.Update(cursor,cameraTransform,buttons,GraphicsDevice,path,isoGrid,city);

                foreach (Ship shp in cty.ships)
                {
                    shp.Update(city, isoGrid, gameTime);
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
            foreach (City cty in cities)
            {
                cty.Draw(spriteBatch,font);;

                foreach (Ship shp in cty.ships)
                {
                    shp.Draw(spriteBatch,path,GraphicsDevice,isoGrid);
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
            foreach (City cty in cities)
            {
                cty.HUDDraw(spriteBatch, font);

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

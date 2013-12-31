using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Multiverse
{
    class City
    {
        private Texture2D tex;
        private Texture2D lowTex;
        private Texture2D highTex;
        private Texture2D clickTex;
        public Vector2 gridposition;
        public Vector2 position;
        public Vector2 screenPosition;
        private Rectangle rect;

        public string name;
        public Vector2 footprint = new Vector2(1, 1);

        public Tuple<float,float,float> politics;
        public List<Ship> ships;
        public List<Material> materials;

        private bool clicked;
        public bool player;

        private Button tradeButton = null;

        private bool tradeDeal = false;

        public City(string name, Texture2D texture,Texture2D highlightTexture, Texture2D clickedTexture, Vector2 position, IsoGrid grid, Tuple<float,float,float> politicalStanding,List<Ship> shipList, List<Material> materialList, bool playerCity = false)
        {
            this.lowTex = texture;
            this.highTex = highlightTexture;
            this.clickTex = clickedTexture;
            this.gridposition = position;
            this.rect = new Rectangle(0, 0, lowTex.Width, lowTex.Height);
            this.position = grid.gridCoords(gridposition) - new Vector2(lowTex.Width / 2.0f, lowTex.Height);

            this.name = name;
            this.player = playerCity;
            
            this.politics = politicalStanding;
            this.ships = shipList;
            this.materials = materialList;

            this.tex = lowTex;
        }

        public void Update(Cursor cursor, Matrix cameraTransform, List<Button> buttons, GraphicsDevice graphicsDevice, Path path,IsoGrid isogrid, City homeCity)
        {
            Vector2 translation = new Vector2(cameraTransform.Translation.X,cameraTransform.Translation.Y);

            screenPosition = position + translation;

            if (cursor.position.X >= screenPosition.X && cursor.position.X <= screenPosition.X + rect.Width
                && cursor.position.Y >= screenPosition.Y && cursor.position.Y <= screenPosition.Y + rect.Height)
            {
                tex = highTex;

                if (cursor.mstate.LeftButton == ButtonState.Pressed)
                {
                    if (cursor.click == false)
                    {
                        cursor.click = true;
                        clicked = true;

                        if (player == false && tradeDeal == false)
                        {
                            tradeButton = new Button(new Vector2(10, 350), 150, 40, "Trade", graphicsDevice);
                            buttons.Add(tradeButton);
                        }
                    }
                }
                        
            }

            else
            {
                tex = lowTex;

                if (tradeButton != null)
                {
                    if (tradeButton.pressed == true)
                    {
                        buttons.Clear();
                        tradeDeal = true;
                        tradeButton = null;

                        ships[0].StartRoute(this,homeCity,isogrid,path);
                    }

                    else if (cursor.mstate.LeftButton == ButtonState.Pressed)
                    {
                        if (cursor.click == false)
                        {
                            cursor.click = true;

                            if (clicked == true)
                            {
                                clicked = false;
                                buttons.Clear();
                            }
                        }
                    }
                }

                else if (cursor.mstate.LeftButton == ButtonState.Pressed)
                {
                    if (cursor.click == false)
                    {
                        

                        if (clicked == true)
                        {
                            cursor.click = true;
                            clicked = false;
                            buttons.Clear();
                        }
                    }
                }
            }

            if (clicked == true)
            {
                tex = clickTex;
            }
        }

            

        public void Draw(SpriteBatch sbatch, SpriteFont font)
        {
            sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            
        }

        public void HUDDraw(SpriteBatch sbatch, SpriteFont font)
        {
            if (clicked == true)
            {
                int y = 30;

                sbatch.DrawString(font, name, new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                y += 20;

                y += 20;
                sbatch.DrawString(font, "Political Standings:", new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                y += 20;

                sbatch.DrawString(font, politics.ToString(), new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                y += 20;

                y += 20;
                sbatch.DrawString(font, "Ships:", new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                y += 20;

                foreach (Ship ship in ships)
                {
                    sbatch.DrawString(font, ship.name.ToString(), new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                    y += 20;
                }

                y += 20;
                sbatch.DrawString(font, "Materials:", new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                y += 20;

                 foreach (Material material in materials)
                {
                    sbatch.DrawString(font, material.name.ToString(), new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                    y += 20;
                }

                 y += 20;
                 sbatch.DrawString(font, "Trade Deal Active:", new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                 y += 20;

                 sbatch.DrawString(font, tradeDeal.ToString(), new Vector2(10, y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.03f);
                 y += 20;
            }
        }
    }
}

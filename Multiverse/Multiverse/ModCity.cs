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
    class ModCity
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
        public Vector2 footprint = new Vector2(4, 4);

        public Tuple<float, float, float> politics;
        public List<Ship> ships;
        public List<Material> materials;
        public List<Tuple<Building, int, Vector2>> buildings = new List<Tuple<Building, int, Vector2>> { };

        private bool clicked;
        public bool player;

        private Button tradeButton = null;

        private bool tradeDeal = false;

        private CityIsoGrid isogrid;
        private Vector2 gridOffset = new Vector2(20, 85);

        public ModCity(string name, Texture2D texture, Texture2D highlightTexture, Texture2D clickedTexture, Vector2 gridposition, IsoGrid grid, Tuple<float, float, float> politicalStanding, List<Ship> shipList, List<Material> materialList, List<Tuple<Building, int>> buildingList, GraphicsDevice graphicsDevice,Random random, bool playerCity = false)
        {
            this.lowTex = texture;
            this.highTex = highlightTexture;
            this.clickTex = clickedTexture;
            this.gridposition = gridposition;
            this.rect = new Rectangle(0, 0, lowTex.Width, lowTex.Height);
            this.position = grid.gridCoords(gridposition) - new Vector2(0, lowTex.Height);

            this.name = name;
            this.player = playerCity;

            this.politics = politicalStanding;
            this.ships = shipList;
            this.materials = materialList;
            
            
            this.tex = lowTex;

            this.isogrid = new CityIsoGrid(graphicsDevice, Math.PI / 6.0, 8, new Vector2(8, 8),position + gridOffset);

            // fill grid with buildings in random positions

            List<Vector2> positions = new List<Vector2>{}; // list of possible positions...

             for (int i=0;i<isogrid.size.X;i++)
            {
                for (int j=0;j<isogrid.size.Y;j++)
                {
                    if (isogrid.usage[i,j] == 0)
                    {
                        positions.Add(new Vector2(i,j));
                    }
                }
             }

            List<Vector2> rpositions = new List<Vector2>{}; // randomised list of possible positions...
            
            foreach (Vector2 p in positions)
            {
                int i = random.Next(rpositions.Count);
                rpositions.Insert(i,p);
            }

            foreach (Tuple<Building, int> buildingType in buildingList) // fit them all in somewhere...
            {
                for (int n = 0; n < buildingType.Item2; n++) // for the number in the city...
                {
                    Vector2 pos = Vector2.Zero;

                    for (int j = 0; j < rpositions.Count; j++) // find a suitable position...
                    {
                        pos = rpositions[j];
                        bool fit = true;

                        for (int l = 0; l < buildingType.Item1.footprint.X; l ++)
                        {
                            for (int k = 0; k < buildingType.Item1.footprint.Y; k++)
                            {
                                if ((int)pos.X + l >= isogrid.usage.GetLength(0) | (int)pos.Y + k >= isogrid.usage.GetLength(1))
                                {
                                    fit = false;
                                }

                                else if (isogrid.usage[(int)pos.X + l, (int)pos.Y + k] == 1)
                                {
                                    fit = false;
                                }
                            }
                        }

                        if (fit == true)
                        {
                            break;
                        }
                    }

                    int type = random.Next(1, buildingType.Item1.variations);

                    buildings.Add(new Tuple<Building, int, Vector2>(buildingType.Item1, type, pos));

                    for (int x = (int)pos.X; x < buildingType.Item1.footprint.X + (int)pos.X; x++)
                    {
                        for (int y = (int)pos.Y; y < buildingType.Item1.footprint.Y + (int)pos.Y; y++)
                        {
                            isogrid.usage[x, y] = 1;
                            rpositions.Remove(pos + new Vector2(x, y));
                        }
                    }
                }
            }
        }

        public void Update(Cursor cursor, Matrix cameraTransform, List<Button> buttons, GraphicsDevice graphicsDevice, Path path, IsoGrid isogrid, ModCity homeCity)
        {
            Vector2 translation = new Vector2(cameraTransform.Translation.X, cameraTransform.Translation.Y);

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

                        ships[0].StartRoute(this, homeCity, isogrid, path);
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
            // rock
            sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            // grid
            //isogrid.Draw(sbatch);

            // buildings
            foreach (Tuple<Building, int, Vector2> building in buildings)
            {
                building.Item1.Draw(sbatch, building.Item3, building.Item2, isogrid);
            }
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

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
    class CityIsoGrid
    {
        Texture2D blank;
        double angle;
        int edge;
        public Vector2 size;
        public Vector2 position;
        public int[,] usage;

        public CityIsoGrid(GraphicsDevice graphicsDevice, double gridAngle, int edgeLength, Vector2 gridSize,Vector2 position)
        {
            this.angle = gridAngle;
            this.edge = edgeLength;
            this.size = gridSize;
            this.position = position;
            this.usage = new int[(int)gridSize.X, (int)gridSize.Y];

            // fill grid with free spaces, except corners

            for (int i=0;i<gridSize.X;i++)
            {
                for (int j=0;j<gridSize.Y;j++)
                {
                    if ((i == 0 && (j == 0 || j== gridSize.Y-1)) || (i == gridSize.X -1 && (j == 0 || j== gridSize.Y-1)))
                    {
                        usage[i,j] = 1;
                    }

                    else
                    {
                         usage[i,j] = 0;
                    }
                }
            }
            

            blank = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Microsoft.Xna.Framework.Color.White });

        }

        public Vector2 gridCoords(Vector2 gridcoords)
        {
            float x = (float)Math.Cos(angle) * edge * (gridcoords.X + 0.5f) - (float)Math.Cos(Math.PI - angle) * edge * (gridcoords.Y + 0.5f);
            float y = (float)Math.Sin(angle) * edge * (gridcoords.X + 0.5f) - (float)Math.Sin(Math.PI - angle) * edge * (gridcoords.Y + 0.5f);

            Vector2 coords = new Vector2(x, y) + position;

            return coords;
        }

        public Vector2 gridEdgeCoords(Vector2 gridcoords)
        {
            float x = (float)Math.Cos(angle) * edge * gridcoords.X - (float)Math.Cos(Math.PI - angle) * edge * gridcoords.Y;
            float y = (float)Math.Sin(angle) * edge * gridcoords.X - (float)Math.Sin(Math.PI - angle) * edge * gridcoords.Y;

            Vector2 coords = new Vector2(x, y) + position;

            return coords;
        }

        public Vector2 cameraCentre(Vector2 gridcoords, GraphicsDeviceManager graphicsDeviceManager)
        {
            Vector2 coords = gridCoords(gridcoords);

            Vector2 offset = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2.0f,
                                            graphicsDeviceManager.PreferredBackBufferHeight / 2.0f);

            return coords - offset;
        }


        public void Draw(SpriteBatch sbatch)
        {
            for (int i = 0; i < size.Y + 1; i++)
            {

                Tuple<Vector2, Vector2> line4 = new Tuple<Vector2, Vector2>(gridEdgeCoords(new Vector2(i, 0)), gridEdgeCoords(new Vector2(i, size.Y))); //CartesianCoords(new Vector2(x, 0)), CartesianCoords(new Vector2(x, (float) rows-1))

                float angle4 = (float)Math.Atan2(line4.Item2.Y - line4.Item1.Y, line4.Item2.X - line4.Item1.X);
                float length4 = Vector2.Distance(line4.Item1, line4.Item2);

                sbatch.Draw(blank, line4.Item1 + new Vector2(0, 0), null, Microsoft.Xna.Framework.Color.Red, angle4, Vector2.Zero, new Vector2(length4, 1.0f), SpriteEffects.None, 0.49f);

            }

            for (int j = 0; j < size.X + 1; j++)
            {
                Tuple<Vector2, Vector2> line4 = new Tuple<Vector2, Vector2>(gridEdgeCoords(new Vector2(0, j)), gridEdgeCoords(new Vector2(size.X, j))); //CartesianCoords(new Vector2(x, 0)), CartesianCoords(new Vector2(x, (float) rows-1))

                float angle4 = (float)Math.Atan2(line4.Item2.Y - line4.Item1.Y, line4.Item2.X - line4.Item1.X);
                float length4 = Vector2.Distance(line4.Item1, line4.Item2);

                sbatch.Draw(blank, line4.Item1 + new Vector2(0, 0), null, Microsoft.Xna.Framework.Color.Red, angle4, Vector2.Zero, new Vector2(length4, 1.0f), SpriteEffects.None, 0.49f);

            }
        }

    }


}



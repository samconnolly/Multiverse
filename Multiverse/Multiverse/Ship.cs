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
    class Ship
    {

        public string name;
        public Vector2 position = new Vector2(-1,-1);
        public Vector2 gridPosition = new Vector2(-1,-1);

        private Texture2D tex;
        private Rectangle rect;

        public bool running;
        public bool forward = true;
        public List<Vector2> drawPath;

        public Vector2 direction = new Vector2(0,1);
        private float targetDistance;
        private Vector2 walkingOffset;
        public Vector2 walkingTarget;
        private float distanceGone;
        public int pathStep;

        private bool drawPaths = false;

        private int timer = 0;
        private int msecsTweenFrames = 120;
        private int currentFrame = 0;
        private int numberOfFrames = 2;
        private int numberOfAnims = 8;
        public int dir = 0;

        private Vector2 drawOffset;

        public Ship(string name, Texture2D texture)
        {
            this.name = name;
            this.tex = texture;
            this.rect = new Rectangle(0, 0, texture.Width / numberOfFrames, texture.Height/ numberOfAnims);
            this.drawOffset = new Vector2(-rect.X / 2, -rect.Y / 2);
        }

        public void StartRoute(ModCity city, ModCity homeCity, IsoGrid isogrid, Path path)
        {
            // calculate a path for movement

            running = true;
            drawPath = path.PathList(homeCity.gridposition + new Vector2(2, 2), city.gridposition + new Vector2(2, 2), isogrid);
            pathStep = 1;
            walkingTarget = drawPath[pathStep];

            gridPosition = drawPath[0];
            position = isogrid.gridCoords( homeCity.position);
        }


        public void Update(ModCity homeCity, IsoGrid isogrid, GameTime gametime)
        {
           
            // continuous movement
            if (running == true)
            {
                direction = (isogrid.gridCoords(walkingTarget) - isogrid.gridCoords(gridPosition));
                
                targetDistance = direction.Length();
                direction.Normalize();

                walkingOffset += direction * gametime.ElapsedGameTime.Milliseconds * 0.02f;
                distanceGone = walkingOffset.Length();

                if (distanceGone < targetDistance)
                {
                    position = isogrid.gridCoords(gridPosition) + walkingOffset;
                }

                else
                {
                    pathStep += 1;


                    if (pathStep < drawPath.Count)
                    {
                        gridPosition = drawPath[pathStep - 1];
                        walkingTarget = drawPath[pathStep];
                        position = isogrid.gridCoords(gridPosition);
                    }

                    else
                    {
                        drawPath.Reverse();
                        pathStep = 1;
                        walkingTarget = drawPath[pathStep];
                        gridPosition = drawPath[pathStep - 1];
                        position = isogrid.gridCoords(gridPosition);
                    }

                    walkingOffset = Vector2.Zero;
                    targetDistance = 0;
                    distanceGone = 0;                                        
                }


            }

            // set direction anim (8 directions!)

            double dot = Vector2.Dot(new Vector2(0, -1), direction); // dot product of direction with vertical

            double angle = Math.Acos(dot); // angle from vertical 

            if (angle == 0) { dir = 0; }
            else if (0 < angle && angle < Math.PI / 2 && direction.X > 0) { dir = 1; }
            else if (angle == Math.PI / 2 && direction.X > 0) { dir = 2; }
            else if (Math.PI / 2 < angle && angle < Math.PI && direction.X > 0) { dir = 3; }
            else if (angle == Math.PI) { dir = 4; }
            else if (Math.PI / 2 < angle && angle < Math.PI && direction.X < 0) { dir = 5; }
            else if (angle == Math.PI / 2 && direction.X < 0) { dir = 6; }
            else if (0 < angle && angle < Math.PI/2 && direction.X < 0) { dir = 7; }

            if (dir != 0)
            {
            }

            // update animation
            timer += gametime.ElapsedGameTime.Milliseconds;

            if (timer >= msecsTweenFrames)
            {
                timer = 0;


                if (currentFrame++ == numberOfFrames - 1)
                {
                    currentFrame = 0;
                }
            }

            rect.X = currentFrame * rect.Width;
            rect.Y = dir * rect.Height;
        }

        public void Draw(SpriteBatch sbatch, Path path, GraphicsDevice graphicsDevice,IsoGrid isogrid)
        {
            if (running == true)
            {
                if (drawPaths == true)
                {
                    path.Draw(graphicsDevice, sbatch, drawPath, Vector2.Zero, isogrid);
                }

                sbatch.Draw(tex, position + drawOffset, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
        }
        
    }
}

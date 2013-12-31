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
    class Cursor
    {

        private Texture2D tex;
        private Rectangle rect;
        public Vector2 position;
        public MouseState mstate;

        public bool click = false;

        public Cursor(Texture2D texture)
        {
            this.tex = texture;
            this.rect = new Rectangle(0, 0, tex.Width, tex.Height);
        }


        public Vector2 Update(Vector2 screenPosition, GraphicsDeviceManager graphics, float scrollSpeed)
        {
            mstate = Mouse.GetState();

            if (click == true && mstate.LeftButton == ButtonState.Released)
            {
                click = false;
            }

            position.X = mstate.X;
            position.Y = mstate.Y;

            if (position.X < 5)
            {
                screenPosition.X -= scrollSpeed;
            }
            if (position.X > graphics.PreferredBackBufferWidth - 5)
            {
                screenPosition.X += scrollSpeed;
            }
            if (position.Y < 5)
            {
                screenPosition.Y -= scrollSpeed;
            }
            if (position.Y > graphics.PreferredBackBufferHeight - 5)
            {
                screenPosition.Y += scrollSpeed;
            }

            return screenPosition;
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.01f);
        }
    }
}

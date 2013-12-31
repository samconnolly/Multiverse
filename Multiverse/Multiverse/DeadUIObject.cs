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
    class DeadUIObject
    {
        Texture2D tex;
        Vector2 position;
        Rectangle rect;

        public DeadUIObject(Texture2D texture, Vector2 position)
        {
            this.tex = texture;
            this.position = position;
            this.rect = new Rectangle(0, 0, tex.Width, tex.Height);
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
        }


    }
}

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
    class DeadObject
    {
        Texture2D tex;
        Vector2 gridposition;
        Vector2 position;
        Rectangle rect;
        
        public DeadObject(Texture2D texture, Vector2 position, IsoGrid grid)
        {
            this.tex = texture;
            this.gridposition = position;
            this.rect = new Rectangle(0,0,tex.Width, tex.Height);
            this.position = grid.gridCoords(gridposition) - new Vector2(tex.Width/2.0f,tex.Height);
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
        }


    }
}

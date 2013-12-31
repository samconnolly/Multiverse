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
    class GridTileRegion
    {
        private Texture2D tex;
        private Vector2 gridPosition;
        private Vector2 region;
        private Rectangle rect;
        private int size = 10;
        private List<Vector2> nums = new List<Vector2>();
        private List<Vector2> pos = new List<Vector2>();

        public GridTileRegion(Texture2D texture, Vector2 gridposition,Vector2 region, IsoGrid grid, Random random)
        {
            this.tex = texture;
            this.rect = new Rectangle(0, 0, tex.Width/size, tex.Height/size);
            this.gridPosition = gridposition;
            this.region = region;

            for (int x = 0; x < region.X; x++)
            {
                for (int y = 0; y < region.Y; y++)
                {
                    nums.Add(new Vector2(random.Next(10),random.Next(10)));
                    pos.Add(grid.gridCoords(gridposition + new Vector2(x,y)));
                }
            }
        }

        public void Draw(SpriteBatch sbatch)
        {
            for (int x = 0; x < (region.X*region.Y); x++)
            {
                    rect.X = (int)nums[x].X*rect.Width;
                    rect.Y = (int)nums[x].Y * rect.Height;

                    Vector2 position = pos[x];

                    sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.85f);
                
            }
        }


    }
}

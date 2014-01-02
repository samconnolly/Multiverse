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
    class Building
    {
        private Texture2D tex;
        private Vector2 position;
        private Vector2 offset;
        public Vector2 footprint;
        private Rectangle rect;
        public int variations;
        public string name;

        public Building(string name, Texture2D texture,int numberOfVariations,Vector2 gridOffset, Vector2 footprint)
        {
            this.tex = texture;
            this.rect = new Rectangle(0, 0, tex.Width/numberOfVariations, tex.Height);
            this.footprint = footprint;
            this.variations = numberOfVariations;
            this.offset = gridOffset;
            this.name = name;
        }

        public void Draw(SpriteBatch sbatch, Vector2 gridposition, int variation, CityIsoGrid grid)
        {
            position = grid.gridEdgeCoords(gridposition) - new Vector2(0, tex.Height) + offset;
            rect.X = rect.Width * (variation - 1);
            sbatch.Draw(tex, position, rect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.4f + gridposition.Y * 0.001f - gridposition.X * 0.0001f);
        }


    }
}
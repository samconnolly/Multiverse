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
    class Button
    {
        Vector2 position;
        int width;
        int height;
        string text;

        public bool pressed = false;

        Texture2D dummyTexture;
        Rectangle rectangle;
        Color textColour = Color.White;
        Color boxColour = Color.Gray;
        Color lineColour = Color.DarkGray;

        public Button(Vector2 position, int width, int height, string text, GraphicsDevice graphicsDevice)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.text = text;

            // button tex
            this.dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.Gray });

            this.rectangle = new Rectangle(0, 0, width, height);
        }


        public void Update(Cursor cursor)
        {
            if (cursor.position.X >= position.X && cursor.position.X <= position.X + width
                && cursor.position.Y >= position.Y && cursor.position.Y <= position.Y + height)
            {
                lineColour = Color.Green;

                if (cursor.mstate.LeftButton == ButtonState.Pressed)
                {
                    if (cursor.click == false)
                    {
                        cursor.click = true;
                        pressed = true;
                    }
                }
            }

            else
            {
                lineColour = Color.DarkGray;
            }

        }

        public void Draw(SpriteBatch sbatch, SpriteFont font)
        {
            // text

            sbatch.DrawString(font, text, position + new Vector2(40, 5), textColour, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.13f);


            // text rectangle
            sbatch.Draw(dummyTexture, position, rectangle, boxColour, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.17f);

            // top line
            Tuple<Vector2, Vector2> line = new Tuple<Vector2, Vector2>(position, (position + new Vector2(rectangle.Width, 0)));

            float angle = (float)Math.Atan2(line.Item2.Y - line.Item1.Y, line.Item2.X - line.Item1.X);
            float length = Vector2.Distance(line.Item1, line.Item2);

            sbatch.Draw(dummyTexture, line.Item1 + new Vector2(0, 0), null, lineColour, angle, Vector2.Zero, new Vector2(length, 3.0f), SpriteEffects.None, 0.15f);

            // right line
            Tuple<Vector2, Vector2> line2 = new Tuple<Vector2, Vector2>((position + new Vector2(rectangle.Width, rectangle.Height)), (position + new Vector2(rectangle.Width, 0)));

            float angle2 = (float)Math.Atan2(line2.Item2.Y - line2.Item1.Y, line2.Item2.X - line2.Item1.X);
            float length2 = Vector2.Distance(line2.Item1, line2.Item2);

            sbatch.Draw(dummyTexture, line2.Item1 + new Vector2(0, 0), null, lineColour, angle2, Vector2.Zero, new Vector2(length2, 3.0f), SpriteEffects.None, 0.15f);

            // left line
            Tuple<Vector2, Vector2> line3 = new Tuple<Vector2, Vector2>(position, (position + new Vector2(0, rectangle.Height)));

            float angle3 = (float)Math.Atan2(line3.Item2.Y - line3.Item1.Y, line3.Item2.X - line3.Item1.X);
            float length3 = Vector2.Distance(line3.Item1, line3.Item2);

            sbatch.Draw(dummyTexture, line3.Item1 + new Vector2(0, 0), null, lineColour, angle3, Vector2.Zero, new Vector2(length3, 3.0f), SpriteEffects.None, 0.15f);

            // bottom line
            Tuple<Vector2, Vector2> line4 = new Tuple<Vector2, Vector2>((position + new Vector2(rectangle.Width, rectangle.Height)), (position + new Vector2(0, rectangle.Height)));

            float angle4 = (float)Math.Atan2(line4.Item2.Y - line4.Item1.Y, line4.Item2.X - line4.Item1.X);
            float length4 = Vector2.Distance(line4.Item1, line4.Item2);

            sbatch.Draw(dummyTexture, line4.Item1 + new Vector2(0, 0), null, lineColour, angle4, Vector2.Zero, new Vector2(length4, 3.0f), SpriteEffects.None, 0.15f);

        }
    }
}

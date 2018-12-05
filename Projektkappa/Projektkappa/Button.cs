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

namespace Projektkappa
{
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Vector2 oldPosition;

        public Vector2 size;
        public Vector2 oldSize;

        private string type;
        bool oldState;

        public Button(GraphicsDevice graphics, string type)
        {
            this.type = type;
            size = new Vector2((graphics.Viewport.Width / 8) + 5, (graphics.Viewport.Height / 30) + 5);
            oldSize = size;
            oldState = false;
        }

        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if(mouseRec.Intersects(rectangle))
            {
                if (size != new Vector2(oldSize.X + 5, oldSize.Y + 5))
                {
                    size = new Vector2(size.X + 1, size.Y + 1);
                    position = new Vector2(position.X - 0.5f, position.Y - 0.5f);
                }
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else
            {
                if (size != new Vector2(oldSize.X, oldSize.Y))
                {
                    size = new Vector2(size.X - 1, size.Y - 1);
                    position = new Vector2(position.X + 0.5f, position.Y + 0.5f);
                }
                isClicked = false;
            }
        }

        public void UpdatePlus(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRec = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRec.Intersects(rectangle))
            {
                if (mouse.LeftButton == ButtonState.Pressed && oldState == false)
                {
                    oldState = true;
                    isClicked = true;
                    if (size != new Vector2(oldSize.X - 3, oldSize.Y - 3))
                    {
                        size = new Vector2(size.X - 1, size.Y - 1);
                        position = new Vector2(position.X + 0.5f, position.Y + 0.5f);
                    }
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    if (size != new Vector2(oldSize.X, oldSize.Y))
                    {
                        size = new Vector2(size.X + 1, size.Y + 1);
                        position = new Vector2(position.X - 0.5f, position.Y - 0.5f);
                    }
                    oldState = false;
                    isClicked = false;
                }
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                if (size != new Vector2(oldSize.X, oldSize.Y))
                {
                    size = new Vector2(size.X + 1, size.Y + 1);
                    position = new Vector2(position.X - 0.5f, position.Y - 0.5f);
                }
                oldState = false;
                isClicked = false;
            }
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(type);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            oldPosition = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}

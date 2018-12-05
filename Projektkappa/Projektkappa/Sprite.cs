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
    // KLASA ODPOWIEDZIALNA ZA RYSOWANIE I WCZYTYWANIE TESKTUR
    public class Sprite
    {
        public Game game;

        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 position;
        private Vector2 size;
        private Color color;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Sprite(Game game)
        {
            this.game = game;
            rectangle = new Rectangle(0, 0, 32, 32);
            color = new Color(255, 255, 255);
        }

        virtual public void LoadContent(ContentManager content, string file)
        {
            texture = content.Load<Texture2D>(file);
        }

        virtual public void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
        virtual public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}

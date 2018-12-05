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
    class Cursor : Sprite
    {
        private Vector2 position;
        public Cursor(Game game) : base(game)
        {
            Size = new Vector2(32, 32);
        }
        
        public void UpdatePosition(MouseState mouse)
        {
            position.X = mouse.X;
            position.Y = mouse.Y; 
        }

        public override void Update(GameTime gameTime)
        {
            Position = position;
            base.Update(gameTime);
        }

    }
}

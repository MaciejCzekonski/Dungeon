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
    class Tiles : Sprite
    {
        private int i;
        public Tiles(Game game, int i, Vector2 position) : base(game)
        {
            this.i = i;
            Size = new Vector2(32, 32);
            Position = position;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void LoadContent(ContentManager contentManager, string file)
        {
            base.LoadContent(contentManager, file + i);
        }
    }
}

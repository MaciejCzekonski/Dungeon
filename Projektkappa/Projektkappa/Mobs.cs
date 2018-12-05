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
    class Mobs
    {
        Game game;
        public Mobs(Game game) { this.game = game; }

        public List<Enemy> mobs = new List<Enemy>();

        Random rand = new Random();
        int ilosc, x, y;

        public void mobGen()
        {
            ilosc = rand.Next(1, 4);
            x = rand.Next(4, 10);
            y = rand.Next(4, 10);
            for (int i = 0; i <= 4; i++)
                mobs.Add(new Enemy(game, new Vector2((x * 32), (y * 32))));
            mobs.Add(new Enemy(game, new Vector2((x * 32), (y * 32))));
            mobs.Add(new Enemy(game, new Vector2((x * 32), (y * 32))));
            mobs.Add(new Enemy(game, new Vector2((x * 32), (y * 32))));

        }

    }
}

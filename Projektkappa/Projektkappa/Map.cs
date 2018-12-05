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
    class Map
    {
        List<Tiles> tiles = new List<Tiles>();
        Game game;
        public Map(Game game) { this.game = game; }

        public int[,] collisionTab;
        // TWORZENIE WZORU MAPY NA PODSTAWIE TABLICY 2D
        public void Generate(int[,] map, int size)
        {
            collisionTab = map;
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0)
                        tiles.Add(new Tiles(game, number, new Vector2(x*size, y*size)));
                }
        }
        public void LoadContent(ContentManager content)
        {
            foreach(var tile in tiles)
                tile.LoadContent(content, "kafel");
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in tiles)
                tile.Update(gameTime);
        }
        // RYSOWANIE MAPY
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var tile in tiles)
            {
                tile.Draw(spriteBatch, gameTime);
            }
        }
            

    }
}

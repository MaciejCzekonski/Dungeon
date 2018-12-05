using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projektkappa
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 centre;
        public int ScreenWidth, screenHeight;

        public Camera(Viewport view)
        {
            this.view = view;
        }

        public void Update(GameTime gameTime, Player player)
        {
            centre = new Vector2(player.position.X + (16) - ScreenWidth/2, player.position.Y + (16) - screenHeight/2);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }

    }
}

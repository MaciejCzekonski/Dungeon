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
    public class Player : Sprite
    {
        public struct Interactives
        {
            public string type;
            public Vector2 Pos;
        }

        public enum moveDirection
        {
            right,
            left,
            up,
            down,
        }
        moveDirection currentMoveDirection = moveDirection.right;

        Texture2D playerleft;
        Texture2D playerup;
        Texture2D playerdown;

        public int Floor = 1;
        public int Gold;
        public int maxFloor;
        public int tempMaxFloor;

        public List<string> WeaponType;
        public Vector2 position;
        private float health = 100;

        public List<Vector2> enemyPos = new List<Vector2>();
        public List<Interactives> interactives = new List<Interactives>();
        public List<int> Interacted = new List<int>();
        public List<bool> hadContact = new List<bool>();

        public int stepCost = 5;
        public int attackCost = 5;
        public float daggerProfinency = 25000;
        public float greatswordProfinency = 25000;
        public float longswordProfinency = 25000;

        public int[,] collisionTab;
        private int Y, X;

        int currTime;
        const int limit = 100;

        const int limitTurn = 0;
        public int currTurn = 10;

        private float playerAP = 25;

        KeyboardState key;

        private bool hasMoved;
        public bool playerTurn;
        public bool onRight;
        public bool onLeft;
        public bool onTop;
        public bool onBottom;
        public bool IntOnRight;
        public bool IntOnLeft;
        public bool IntOnTop;
        public bool IntOnBottom;
        public bool reachedExit;

        public bool isLongsword;
        public bool isGreatsword;
        public bool isDagger;

        public Player(Game game) : base(game)
        {
            Size = new Vector2(32, 32);
            hasMoved = false;
            playerTurn = true;
            onRight = false;
            onLeft = false;
            onBottom = false;
            onTop = false;
            IntOnRight = false;
            IntOnLeft = false;
            IntOnTop = false;
            IntOnBottom = false;
            reachedExit = false;
            isLongsword = true;
            isGreatsword = false;
            isDagger = false;
            WeaponType = new List<string>();
            WeaponType.Add("Longsword");
            Gold = 0;
            //WeaponType.Add("Greatsword");
            //WeaponType.Add("Dagger");
        }

        public override void LoadContent(ContentManager contentManager, string file)
        {
            playerleft = contentManager.Load<Texture2D>(file + "left");
            playerup = contentManager.Load<Texture2D>(file + "up");
            playerdown = contentManager.Load<Texture2D>(file + "down");
            base.LoadContent(contentManager, file);
        }

        public float PlayerAP
        {
            get { return playerAP; }
            set { playerAP = value; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        private void getProf()
        {
            if (isLongsword == true)
                longswordProfinency += (playerAP / 25 + Floor);
            else if (isGreatsword == true)
                greatswordProfinency += (playerAP / 25 + Floor);
            else if (isDagger == true)
                daggerProfinency += (playerAP / 25 + Floor);
        }

        override public void Update(GameTime gameTime)
        {
            key = Keyboard.GetState();
            // KALKULACJA INDEKSU TABLICY NA PODSTAWIE POZYCJI
            X = (int)position.X / 32;
            Y = (int)position.Y / 32;

            Position = position;
            // TIMER
            if(hasMoved == true)
            {
                currTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currTime >= limit && key.IsKeyUp(Keys.Right) && key.IsKeyUp(Keys.Down) && key.IsKeyUp(Keys.Up) && key.IsKeyUp(Keys.Left))
                {
                    currTime = 0;
                    hasMoved = false;
                    return;
                }
            }

            if (key.IsKeyDown(Keys.D1) && WeaponType.Contains("Longsword") && isLongsword == false)
            {
                isDagger = false;
                isGreatsword = false;
                isLongsword = true;
                attackCost = 10;
            }
            else if (key.IsKeyDown(Keys.D2) && WeaponType.Contains("Dagger") && isDagger == false)
            {
                isDagger = true;
                isGreatsword = false;
                isLongsword = false;
                attackCost = 10;
            }
            else if (key.IsKeyDown(Keys.D3) && WeaponType.Contains("Greatsword") && isGreatsword == false)
            {
                isDagger = false;
                isGreatsword = true;
                isLongsword = false;
                attackCost = 10;
            }

            // CHODZENIE, KOLIZJA I ATAKOWANIE
            if (collisionTab[Y, X] == 30)
                reachedExit = true;
            if (playerTurn == true)
            {
                // KOLIZJA Z MOBEM
                foreach(var pos in enemyPos)
                {
                    if (position.X + 32 == pos.X && position.Y == pos.Y)
                        onRight = true;
                    else if (position.X - 32 == pos.X && position.Y == pos.Y)
                        onLeft = true;
                    else if (position.X == pos.X && position.Y - 32 == pos.Y)
                        onTop = true;
                    else if (position.X == pos.X && position.Y + 32 == pos.Y)
                        onBottom = true;
                }
                // KOLIZJA Z PRZEDMIOTEM INTERAKTYWNYM
                foreach (var IntPos in interactives)
                {
                    if (position.X + 32 == IntPos.Pos.X && position.Y == IntPos.Pos.Y)
                        IntOnRight = true;
                    else if (position.X - 32 == IntPos.Pos.X && position.Y == IntPos.Pos.Y)
                        IntOnLeft = true;
                    else if (position.X == IntPos.Pos.X && position.Y - 32 == IntPos.Pos.Y)
                        IntOnTop = true;
                    else if (position.X == IntPos.Pos.X && position.Y + 32 == IntPos.Pos.Y)
                        IntOnBottom = true;
                }

                if (key.IsKeyDown(Keys.Right) == true && hasMoved == false && (collisionTab[Y, X + 1] == 1 || collisionTab[Y, X + 1] == 30))
                {
                    currentMoveDirection = moveDirection.right;
                    if (onRight == false && IntOnRight == false)
                    {
                        position.X += 32f;
                        currTurn -= stepCost;
                        onLeft = false;
                        onBottom = false;
                        onTop = false;
                        IntOnLeft = false;
                        IntOnTop = false;
                        IntOnBottom = false;

                    }
                    else if(onRight == true)
                    {
                        getProf();
                        for (int i = 0; i < enemyPos.Count; i++)
                            if (position.X + 32 == enemyPos[i].X && position.Y == enemyPos[i].Y)
                                hadContact[i] = true;
                        currTurn -= attackCost;
                        onLeft = false;
                        onBottom = false;
                        onTop = false;
                    }
                    else if(IntOnRight == true)
                    {
                        for (int i = 0; i < interactives.Count; i++)
                            if (position.X + 32 == interactives[i].Pos.X && position.Y == interactives[i].Pos.Y)
                                Interacted[i]++;
                        IntOnLeft = false;
                        IntOnTop = false;
                        IntOnBottom = false;
                    }
                    hasMoved = true;
                }
                else if (key.IsKeyDown(Keys.Left) == true && hasMoved == false && (collisionTab[Y, X - 1] == 1 || collisionTab[Y, X - 1] == 30))
                {
                    currentMoveDirection = moveDirection.left;
                    if (onLeft == false && IntOnLeft == false)
                    {
                        position.X -= 32f;
                        currTurn -= stepCost;
                        onRight = false;
                        onBottom = false;
                        onTop = false;
                        IntOnRight = false;
                        IntOnTop = false;
                        IntOnBottom = false;
                    }
                    else if(onLeft == true)
                    {
                        getProf();
                        for (int i = 0; i < enemyPos.Count; i++)
                            if (position.X - 32 == enemyPos[i].X && position.Y == enemyPos[i].Y)
                                hadContact[i] = true;
                        currTurn -= attackCost;
                        onRight = false;
                        onBottom = false;
                        onTop = false;

                    }
                    else if (IntOnLeft == true)
                    {
                        for (int i = 0; i < interactives.Count; i++)
                            if (position.X - 32 == interactives[i].Pos.X && position.Y == interactives[i].Pos.Y)
                                Interacted[i]++;
                        IntOnRight = false;
                        IntOnTop = false;
                        IntOnBottom = false;
                    }
                    hasMoved = true;
                }
                else if (key.IsKeyDown(Keys.Up) == true && hasMoved == false && (collisionTab[Y - 1, X] == 1 || collisionTab[Y - 1, X] == 30))
                {
                    currentMoveDirection = moveDirection.up;
                    if (onTop == false && IntOnTop == false)
                    {
                        position.Y -= 32f;
                        currTurn -= stepCost;
                        onLeft = false;
                        onBottom = false;
                        onRight = false;
                        IntOnRight = false;
                        IntOnLeft = false;
                        IntOnBottom = false;
                    }
                    else if(onTop == true)
                    {
                        getProf();
                        for (int i = 0; i < enemyPos.Count; i++)
                            if (position.Y - 32 == enemyPos[i].Y && position.X == enemyPos[i].X)
                                hadContact[i] = true;
                        currTurn -= attackCost;
                        onLeft = false;
                        onBottom = false;
                        onRight = false;
                    }
                    else if (IntOnTop == true)
                    {
                        for (int i = 0; i < interactives.Count; i++)
                            if (position.X == interactives[i].Pos.X && position.Y - 32 == interactives[i].Pos.Y)
                                Interacted[i]++;
                        IntOnLeft = false;
                        IntOnRight = false;
                        IntOnBottom = false;
                    }
                    hasMoved = true;
                }
                else if (key.IsKeyDown(Keys.Down) == true && hasMoved == false && (collisionTab[Y + 1, X] == 1 || collisionTab[Y + 1, X] == 30))
                {
                    currentMoveDirection = moveDirection.down;
                    if (onBottom == false && IntOnBottom == false)
                    {
                        position.Y += 32f;
                        currTurn -= stepCost;
                        onLeft = false;
                        onRight = false;
                        onTop = false;
                        IntOnRight = false;
                        IntOnLeft = false;
                        IntOnTop = false;
                    }
                    else if(onBottom == true)
                    {
                        getProf();
                        for (int i = 0; i < enemyPos.Count; i++)
                            if (position.Y + 32 == enemyPos[i].Y && position.X == enemyPos[i].X)
                                hadContact[i] = true;
                        currTurn -= attackCost;
                        onLeft = false;
                        onRight = false;
                        onTop = false;
                    }
                    else if (IntOnBottom == true)
                    {
                        for (int i = 0; i < interactives.Count; i++)
                            if (position.X == interactives[i].Pos.X && position.Y + 32 == interactives[i].Pos.Y)
                                Interacted[i]++;
                        IntOnLeft = false;
                        IntOnTop = false;
                        IntOnRight = false;
                    }
                    hasMoved = true;
                }
                /*if (isGreatsword == false)
                if (currTurn < attackCost)
                {
                    currTurn = 10;
                    playerTurn = false;
                }*/
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (currentMoveDirection)
            {
                case moveDirection.right:
                    
                    base.Draw(spriteBatch, gameTime);

                    break;
                case moveDirection.left:

                    spriteBatch.Draw(playerleft, Position, Color.White);

                    break;

                case moveDirection.up:

                    spriteBatch.Draw(playerup, Position, Color.White);

                    break;

                case moveDirection.down:

                    spriteBatch.Draw(playerdown, Position, Color.White);

                    break;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Projektkappa
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        Cursor cursor;

        Player player;

        Map map;

        MapGen mapGen;

        List<int> indeksy = new List<int>();
        List<int> milestones = new List<int>();

        public int Floor = 0;
        int counter = 0;

        // 0 - brak kontynuacji
        // 1 - mozliwa do kontynuacji
        // 2 - nastepne pietro
        // 3 - karczma

        int STATE = 0;

        enum GameState
        {
            MainMenu,
            Playing,
            Koniec,
            Die,
            NextLevel,
        }
        GameState CurrentGameState = GameState.MainMenu;

        int screenWidth = 1650, screenHeight = 1050;

        Button Generate;
        Button End;

        Color backgroundColor = Color.Black;
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);

            camera.screenHeight = screenHeight;
            camera.ScreenWidth = screenWidth;

            cursor = new Cursor(this);

            mapGen = new MapGen();

            Reset();

            base.Initialize();
        }

        protected void Reset()
        {
            map = new Map(this);
            map.Generate(mapGen.Gen(), 32);

            if (STATE == 2)
            {
                player.position = new Vector2(mapGen.playerPositionX, mapGen.playerPositionY);
            }
            else player = new Player(this);

            if (Floor >= 0)
            {
                player.position = new Vector2(mapGen.playerPositionX, mapGen.playerPositionY);
            }
            else player.position = new Vector2(64, 64);
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            //graphics.IsFullScreen = true;
            //IsMouseVisible = true;

            cursor.LoadContent(Content, "Cursor");

            Generate = new Button(graphics.GraphicsDevice, "NewGame");
            Generate.SetPosition(new Vector2(screenWidth/2 - Generate.size.X/2, screenHeight/2));

            End = new Button(graphics.GraphicsDevice, "End");
            End.SetPosition(new Vector2(screenWidth / 2 - End.size.X/2, screenHeight / 2 + 100));

            Generate.LoadContent(Content);

            End.LoadContent(Content);

            if (milestones.Count > 0)
                player.LoadContent(Content, "hero" + (milestones.Count + 1));
            else player.LoadContent(this.Content, "hero");

            map.LoadContent(Content);

            if (counter == 0)
            {
                graphics.ApplyChanges();
                counter += 1;
            }
            // TODO: use this.Content to load your game content here
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState mouse = Mouse.GetState();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    cursor.UpdatePosition(mouse);
                    cursor.Update(gameTime);

                    if (Generate.isClicked == true)
                    {
                        Floor = 0;
                        STATE = 1;
                        CurrentGameState = GameState.Playing;
                        Initialize();
                        Generate.isClicked = false;
                        return;
                    }
                    else if (End.isClicked == true)
                    {
                        CurrentGameState = GameState.Koniec;
                        End.isClicked = false;
                        return;
                    }
                    End.Update(mouse);
                    Generate.Update(mouse);
                    break;
                case GameState.NextLevel:
                    if (Floor == 0)
                    {
                        player.Gold = 0;
                        if (player.maxFloor >= 20)
                            Floor = 10;
                    }
                    Floor++;
                    player.tempMaxFloor = Floor;
                    STATE = 2;
                    Initialize();
                    STATE = 1;
                    CurrentGameState = GameState.Playing;
                    break;

                case GameState.Playing:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        CurrentGameState = GameState.MainMenu;
                    }
                    camera.Update(gameTime, player);
                    if (player.reachedExit == true)
                    {
                        CurrentGameState = GameState.NextLevel;
                        player.reachedExit = false;
                        return;
                    }

                    cursor.UpdatePosition(mouse);
                    cursor.Update(gameTime);

                    map.Update(gameTime);
                    // UPDATE PASKOW ZYCIA


                    if (player.Health - 100 > 0)
                        player.Health = 100;

                    player.collisionTab = map.collisionTab;

                    player.Update(gameTime);

                    if (player.Health <= 0)
                    {
                        player.onBottom = false;
                        player.onLeft = false;
                        player.onRight = false;
                        player.onTop = false;
                        player.IntOnBottom = false;
                        player.IntOnTop = false;
                        player.IntOnLeft = false;
                        player.IntOnRight = false;
                        CurrentGameState = GameState.Die;
                    }
                    break;

                case GameState.Koniec:

                    Exit();

                    break;

                case GameState.Die:

                    if (player.maxFloor < player.tempMaxFloor)
                        player.maxFloor = player.tempMaxFloor;

                    Floor = 0;

                    STATE = 2;

                    Initialize();

                    STATE = 1;

                    player.Health = 100;

                    CurrentGameState = GameState.Playing;

                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            switch (CurrentGameState)
            {
                // RYSOWANIE PODCZAS POBYTU W MENU
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

                    Generate.Draw(spriteBatch, gameTime);
                    End.Draw(spriteBatch, gameTime);

                    cursor.Draw(spriteBatch, gameTime);

                    spriteBatch.End();
                    break;

                // RYSOWANIE PODCZAS GRY
                case GameState.Playing:
                    spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null, null, null, null,
                        camera.transform);
                    map.Draw(spriteBatch, gameTime);

                    player.Draw(spriteBatch, gameTime);

                    spriteBatch.End();

                    spriteBatch.Begin();

                    cursor.Draw(spriteBatch, gameTime);

                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}

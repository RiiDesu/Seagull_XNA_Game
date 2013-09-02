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

namespace Seagulls
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite Background_GAME;
        //Enemy eSeagull;
        List<Enemy> Enemies;
        Player player;

        static float SPAWNTIMER = 3;
        float spawnTimer = SPAWNTIMER;

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
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //eSeagull = new Enemy();
            //eSeagull.LoadContent(this.Content);

            Enemies = new List<Enemy>();
            LoadEnemies();

            player = new Player();
            player.LoadContent(this.Content);

            Background_GAME = new Sprite();
            Background_GAME.LoadContent(this.Content, "bgGame");
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

            // TODO: Add your update logic here
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //eSeagull.Update(gameTime);
            foreach (Enemy enemy in Enemies) 
            { 
                enemy.Update(gameTime); 
            }
            LoadEnemies();
            HandleGame();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            // TODO: Add your drawing code here
            Background_GAME.Draw(this.spriteBatch);
            player.DrawGUI(this.spriteBatch);

            //eSeagull.Draw(this.spriteBatch);
            foreach (Enemy enemy in Enemies) 
            { 
                enemy.Draw(this.spriteBatch); 
            }

            player.Draw(this.spriteBatch);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void LoadEnemies() 
        {
            Random rnd = new Random();
            Vector2 Speed, Pos, Dir;
            
            int Enemy_SPEED_X, Enemy_SPEED_Y,
                Enemy_POSITION_X, Enemy_POSITION_Y, 
                Enemy_DIRECTION_X, Enemy_DIRECTION_Y;
            bool randX, randY;

            // Random speed, x, y
            Enemy_SPEED_X = rnd.Next(200, 400);
            Enemy_SPEED_Y = rnd.Next(150, 300);
            Enemy_POSITION_X = rnd.Next(30, 750);
            Enemy_POSITION_Y = rnd.Next(300);

            // Random dx, dy
            randX = rnd.Next(0, 2) == 0;
            randY = rnd.Next(0, 2) == 0;

            if (!randX) { Enemy_DIRECTION_X = -1; }
            else { Enemy_DIRECTION_X = 1; }
            if (!randY) { Enemy_DIRECTION_Y = -1; }
            else { Enemy_DIRECTION_Y = 1; }

            Speed.X = Enemy_SPEED_X;
            Speed.Y = Enemy_SPEED_Y;
            Pos.X = Enemy_POSITION_X;
            Pos.Y = Enemy_POSITION_Y;
            Dir.X = Enemy_DIRECTION_X;
            Dir.Y = Enemy_DIRECTION_Y;

            if (spawnTimer > SPAWNTIMER)
            {
                spawnTimer = 0;
                if (Enemies.Count() < 10)
                {
                    Enemies.Add(new Enemy(this.Content, Speed, Pos, Dir));
                }
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].Active)
                {
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void HandleGame() 
        {
            int enemyX, enemyY, enemyW, enemyH;
            if (player.MouseClick)
            {
                for (int i = 0; i < Enemies.Count; i++)
                {
                    enemyX = (int)Enemies[i].Position.X;
                    enemyY = (int)Enemies[i].Position.Y;
                    enemyW = (int)Enemies[i].Size.Width;
                    enemyH = (int)Enemies[i].Size.Height;

                    if (CheckCollission(player.x, player.y, enemyX, enemyY, enemyW, enemyH))
                    {
                        if (player.misses == 0) player.score += 300;
                        else player.score += (300 / player.misses);
                        Enemies[i].Active = false;
                    }
                    else player.misses++;
                }
            }
        }

        public bool CheckCollission(int aimX, int aimY, int tX, int tY, int tW, int tH) 
        {
            if ((aimY > tY) && (aimY < (tY + tH)))
            {
                if ((aimX > tX) && (aimX < (tX + tW))) 
                {
                    return true;
                }
            }
            return false;
        }
    }
}

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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        enum ScreenState { Menu, Game, Score }
        ScreenState GameState;

        // --- Main Menu ---
        Sprite Background_MENU;
        Song BGM_Menu;
        //List<Button> MainMenu;

        // --- Game Screen ---
        Sprite Background_GAME;
        Song BGM_Game;
        SoundEffect SFX_hit, SFX_shoot, SFX_time, SFX_spawn;
        Player player;
        Button button; //Game Quit
        List<Enemy> Enemies;

        static float SPAWNTIMER = 3;
        float spawnTimer = SPAWNTIMER;

        // --- Score Screen ---
        Sprite Background_SCORE;
        SoundEffect BGM_Score;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // --- Main Menu ---
            GameState = ScreenState.Game;
            BGM_Menu = this.Content.Load<Song>("Sound/Menu");

            Background_MENU = new Sprite();
            Background_MENU.LoadContent(this.Content, "bgMenu");

            // --- Game Screen ---
            Enemies = new List<Enemy>();
            LoadEnemies();

            player = new Player();
            player.LoadContent(this.Content);

            button = new Button();
            button.LoadContent(this.Content, "Buttons/game_quit", 715, 394);

            SFX_hit = this.Content.Load<SoundEffect>("Sound/hit");
            SFX_shoot = this.Content.Load<SoundEffect>("Sound/shoot");
            SFX_time = this.Content.Load<SoundEffect>("Sound/time");
            SFX_spawn = this.Content.Load<SoundEffect>("Sound/spawn");

            BGM_Game = this.Content.Load<Song>("Sound/Game");

            Background_GAME = new Sprite();
            Background_GAME.LoadContent(this.Content, "bgGame");

            // --- Score Screen ---
            BGM_Score = this.Content.Load<SoundEffect>("Sound/Score");
            Background_SCORE = new Sprite();
            Background_SCORE.LoadContent(this.Content, "bgScore");

            MediaPlayer.Play(BGM_Game);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (GameState) 
            {
                case ScreenState.Menu: 
                {
                    //GameState = ScreenState.Game;
                    break;
                }
                case ScreenState.Game:
                {
                    spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Update(gameTime);
                    }
                    LoadEnemies();
                    button.Update();
                    HandleGame();

                    player.Update(gameTime);
                    break;
                }
                case ScreenState.Score: 
                {
                    GameState = ScreenState.Menu;
                    break;
                }
                default: GameState = ScreenState.Menu; break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (GameState)
            {
                case ScreenState.Menu:
                {
                    Background_MENU.Draw(this.spriteBatch);
                    break;
                }
                case ScreenState.Game:
                {
                    Background_GAME.Draw(this.spriteBatch);
                    button.Draw(this.spriteBatch);
                    foreach (Enemy enemy in Enemies) { enemy.Draw(this.spriteBatch); }
                    player.DrawGUI(this.spriteBatch);
                    player.Draw(this.spriteBatch);
                    break;
                }
                case ScreenState.Score:
                {
                    Background_SCORE.Draw(this.spriteBatch);
                    break;
                }
                default: GameState = ScreenState.Menu; break;
            }

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
                    SFX_spawn.Play();
                }
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].Active)
                {
                    Enemies.RemoveAt(i);
                    i--;
                    SFX_hit.Play();
                }
            }
        }

        public void HandleGame() 
        {
            Rectangle enemyP;
            if (player.MouseClick)
            {
                SFX_shoot.Play();
                for (int i = 0; i < Enemies.Count; i++)
                {
                    enemyP.X = (int)Enemies[i].Position.X;
                    enemyP.Y = (int)Enemies[i].Position.Y;
                    enemyP.Width = (int)Enemies[i].Size.Width;
                    enemyP.Height = (int)Enemies[i].Size.Height;

                    if (enemyP.Contains(new Point(player.x, player.y)))
                    {
                        if (player.misses == 0) player.score += 300;
                        else player.score += (300 / player.misses);
                        Enemies[i].Active = false;
                    }
                    else player.misses++;
                }
            }

            if (button.clicked) 
            {
                SFX_time.Play();
                this.Exit();
            }
        }
    }
}

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
        Sprite Background_MENU, Title_MENU;
        Song BGM_Menu;
        List<Button> MainMenuButtons;

        // --- Game Screen ---
        Sprite Background_GAME;
        Song BGM_Game;
        SoundEffect SFX_hit, SFX_shoot, SFX_time, SFX_spawn;
        Player player;
        List<Button> GameButtons;
        List<Enemy> Enemies;

        bool paused = false;
        static float SPAWNTIMER = 3;
        float spawnTimer = SPAWNTIMER;

        // --- Score Screen ---
        Sprite Background_SCORE, rank;
        SoundEffect BGM_Score;
        SpriteFont font;
        List<Button> ScoreButtons;
        List<Sprite> Ranking;
        Color ratingColor = Color.Gray;

        string scoreScore = "";
        string scoreMisses = "";
        string scoreRating = "";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // --- Sounds ---
            BGM_Menu = this.Content.Load<Song>("Sound/Menu");
            BGM_Game = this.Content.Load<Song>("Sound/GameNormal");
            BGM_Score = this.Content.Load<SoundEffect>("Sound/Score");

            SFX_hit = this.Content.Load<SoundEffect>("Sound/hit");
            SFX_shoot = this.Content.Load<SoundEffect>("Sound/shoot");
            SFX_time = this.Content.Load<SoundEffect>("Sound/time");
            SFX_spawn = this.Content.Load<SoundEffect>("Sound/spawn");

            // --- Main Menu ---
            GameState = ScreenState.Menu;

            MainMenuButtons = new List<Button>();
            MainMenuButtons.Add(new Button());
            MainMenuButtons[0].LoadContent(this.Content, "Buttons/menu_start", -148, 313);
            MainMenuButtons.Add(new Button());
            MainMenuButtons[1].LoadContent(this.Content, "Buttons/menu_quit", 800, 313);

            Background_MENU = new Sprite();
            Background_MENU.LoadContent(this.Content, "bgMenu");
            Title_MENU = new Sprite();
            Title_MENU.LoadContent(this.Content, "title");
            Title_MENU.Position.Y = -120;

            // --- Game Screen ---
            Enemies = new List<Enemy>();

            player = new Player();
            player.LoadContent(this.Content);

            GameButtons = new List<Button>();
            GameButtons.Add(new Button());
            GameButtons[0].LoadContent(this.Content, "Buttons/game_quit", 715, 394);
            GameButtons.Add(new Button());
            GameButtons[1].LoadContent(this.Content, "Buttons/game_retry", 630, 394);
            GameButtons.Add(new Button());
            GameButtons[2].LoadContent(this.Content, "Buttons/game_pause", 545, 394);
            GameButtons.Add(new Button());
            GameButtons[3].LoadContent(this.Content, "Buttons/game_proceed", 204, -45);
            GameButtons[3].Scale = 0;

            Background_GAME = new Sprite();
            Background_GAME.LoadContent(this.Content, "bgGame");

            // --- Score Screen ---
            Background_SCORE = new Sprite();
            Background_SCORE.LoadContent(this.Content, "bgScore");
            font = this.Content.Load<SpriteFont>("scoreFont");

            ScoreButtons = new List<Button>();
            ScoreButtons.Add(new Button());
            ScoreButtons[0].LoadContent(this.Content, "Buttons/score_return", 532, 416);
            ScoreButtons.Add(new Button());
            ScoreButtons[1].LoadContent(this.Content, "Buttons/score_retry", 532, 351);

            Ranking = new List<Sprite>();
            Ranking.Add(new Sprite());
            Ranking[0].LoadContent(this.Content, "score-ss");
            Ranking.Add(new Sprite());
            Ranking[1].LoadContent(this.Content, "score-a");
            Ranking.Add(new Sprite());
            Ranking[2].LoadContent(this.Content, "score-c");
            Ranking.Add(new Sprite());
            Ranking[3].LoadContent(this.Content, "score-d");
            Ranking.Add(new Sprite());
            Ranking[4].LoadContent(this.Content, "score-x");
            rank = new Sprite();
            rank.LoadContent(this.Content, "default");
            rank.Scale = 0;

            // --- Music Player ---
            MediaPlayer.Play(BGM_Menu);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            switch (GameState) 
            {
                case ScreenState.Menu: 
                {
                    this.IsMouseVisible = true;
                    foreach (Button button in MainMenuButtons) { button.Update(); }
                    HandleMenu();
                    break;
                }

                case ScreenState.Game:
                {
                    HandleGameButtons();
                    
                    if (paused) { this.IsMouseVisible = true; }
                    else
                    {
                        HandleGame(gameTime);
                    }
                    break;
                }

                case ScreenState.Score: 
                {
                    this.IsMouseVisible = true;
                    HandleScore();
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
                    Title_MENU.Draw(this.spriteBatch);
                    foreach (Button button in MainMenuButtons) { button.Draw(this.spriteBatch); }
                    break;
                }
                case ScreenState.Game:
                {
                    Background_GAME.Draw(this.spriteBatch);
                    foreach (Enemy enemy in Enemies) { enemy.Draw(this.spriteBatch); }
                    foreach (Button button in GameButtons) { button.Draw(this.spriteBatch); }
                    player.DrawGUI(this.spriteBatch);
                    player.Draw(this.spriteBatch);
                    break;
                }
                case ScreenState.Score:
                {
                    Background_SCORE.Draw(this.spriteBatch);
                    spriteBatch.DrawString(font, scoreScore, new Vector2(30, 20), Color.LightSkyBlue);
                    spriteBatch.DrawString(font, scoreMisses, new Vector2(30, 55), Color.Red);
                    spriteBatch.DrawString(font, scoreRating, new Vector2(30, 320), ratingColor);
                    foreach (Button button in ScoreButtons) { button.Draw(this.spriteBatch); }
                    rank.Draw(this.spriteBatch);
                    break;
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void HandleMenu()
        {
            if (Title_MENU.Position.Y <= -20) { Title_MENU.Position.Y++; }
            if (MainMenuButtons[0].Position.X <= 173) { MainMenuButtons[0].Position.X += 3; }
            if (MainMenuButtons[1].Position.X >= 479) { MainMenuButtons[1].Position.X -= 3; }

            if (MainMenuButtons[0].clicked) //Start Button
            {
                player.Reset();
                GameState = ScreenState.Game;
                MediaPlayer.Stop();
                MediaPlayer.Play(BGM_Game);
            }

            if (MainMenuButtons[1].clicked) //Exit Button
            {
                this.Exit();
            }
        }

        public void HandleGame(GameTime gameTime)
        {
            if (!player.active)
            {
                this.IsMouseVisible = true;
                GameButtons[3].Scale = 1;

                if (GameButtons[3].Position.Y <= 219) GameButtons[3].Position.Y++;
            }
            else
            {
                this.IsMouseVisible = false;
                GameButtons[3].Scale = 0;

                foreach (Enemy enemy in Enemies) { enemy.Update(gameTime); }
                SpawnNewEnemies();
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.Update(gameTime);

                Rectangle enemyP;
                bool hit, enemiesActive;

                if (player.MouseClick)
                {
                    SFX_shoot.Play();
                    hit = false;
                    enemiesActive = false;

                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        enemyP.X = (int)Enemies[i].Position.X;
                        enemyP.Y = (int)Enemies[i].Position.Y;
                        enemyP.Width = (int)Enemies[i].Size.Width;
                        enemyP.Height = (int)Enemies[i].Size.Height;

                        if (enemyP.Contains(new Point(player.x, player.y)))
                        {
                            hit = true;
                            Enemies[i].Active = false;
                        }

                        if (Enemies[i].Active == true)
                        {
                            enemiesActive = true;
                        }
                    }

                    if (hit == true)
                    {
                        if (player.misses == 0) player.score += 300;
                        else player.score += (300 / (player.misses + 1));
                    }
                    else
                    {
                        Rectangle EnemyArea; EnemyArea.X = 0; EnemyArea.Y = 0; EnemyArea.Width = 800; EnemyArea.Height = 350;
                        if ((enemiesActive == true) && (EnemyArea.Contains(new Point(player.x, player.y)))) player.misses++;
                    }
                } //for (int i = 0; i < Enemies.Count; i++)
            } //else
        }

        public void HandleScore()
        {
            if (rank.Scale < 1)
            {
                rank.Scale += 0.015f;
            }

            scoreScore = "Score: " + player.score;
            scoreMisses = "Misses: " + player.misses;
            rank.Position.X = 790 - rank.Size.Width;
            rank.Position.Y = 10;

            if (rank == Ranking[0]) //Perfect Ending
            {
                Random rnd = new Random();
                int theColor = rnd.Next(1, 8);
                switch (theColor)
                {
                    case 1: ratingColor = Color.Red; break;
                    case 2: ratingColor = Color.Orange; break;
                    case 3: ratingColor = Color.Yellow; break;
                    case 4: ratingColor = Color.Green; break;
                    case 5: ratingColor = Color.Blue; break;
                    case 6: ratingColor = Color.Indigo; break;
                    case 7: ratingColor = Color.Violet; break;
                    default: ratingColor = Color.Gray; break;
                }
            }

            //Handle Buttons
            foreach (Button button in ScoreButtons) { button.Update(); }
            if (ScoreButtons[0].clicked) //Return
            {
                foreach (Enemy enemy in Enemies) { enemy.Active = false; }
                paused = false;
                GameButtons[3].Position.Y = -45;
                MediaPlayer.Stop();
                MediaPlayer.Play(BGM_Menu);

                GameState = ScreenState.Menu;

            }
            if (ScoreButtons[1].clicked) //Retry
            {
                SFX_time.Play();
                player.Reset();
                foreach (Enemy enemy in Enemies) { enemy.Active = false; }
                paused = false;
                GameButtons[3].Position.Y = -45;
                MediaPlayer.Stop();
                MediaPlayer.Play(BGM_Game);

                GameState = ScreenState.Game;
            }
        }

        public void HandleGameButtons()
        {
            foreach (Button button in GameButtons) { button.Update(); }

            if (GameButtons[0].clicked) //Return to Menu
            {
                SFX_time.Play();
                foreach (Enemy enemy in Enemies) { enemy.Active = false; }
                paused = false;
                MediaPlayer.Stop();
                MediaPlayer.Play(BGM_Menu);

                GameState = ScreenState.Menu;
            }

            if (GameButtons[1].clicked) //Retry
            {
                SFX_time.Play();
                player.Reset();
                foreach (Enemy enemy in Enemies) { enemy.Active = false; }
                paused = false;
                MediaPlayer.Resume();
            }

            if (GameButtons[2].clicked) //Pause
            {
                if (paused == false)
                {
                    player.Timer.Stop();
                    MediaPlayer.Pause();
                }
                else
                {
                    player.Timer.Start();
                    MediaPlayer.Resume();
                }
                paused = !paused;
            }

            if (GameButtons[3].clicked) //Proceed
            {
                GameState = ScreenState.Score;
                MediaPlayer.Stop();
                BGM_Score.Play();
                CalculateRank();
            }
        }
        
        public void SpawnNewEnemies() 
        {
            if (spawnTimer > SPAWNTIMER)
            {
                if (Enemies.Count() < 10)
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
                    Dir.X = Enemy_DIRECTION_X;
                    Pos.X = Enemy_POSITION_X;
                    Pos.Y = Enemy_POSITION_Y;
                    Dir.Y = Enemy_DIRECTION_Y;

                    Enemies.Add(new Enemy(this.Content, Speed, Pos, Dir));
                    SFX_spawn.Play();
                    spawnTimer = 0;
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

        public void CalculateRank() 
        {
            if ((player.score == 0) && (player.misses == 0))
            {
                ratingColor = Color.Gray;
                scoreRating = "\n\n...";
                rank = Ranking[4]; // x
            }
            else if (player.score <= 1800)
            {
                ratingColor = Color.Red;
                scoreRating = "Worst Ending\n\nYou have been brutally murdered..";
                rank = Ranking[3]; // D
            }
            else
            {
                if (player.misses <= 1)
                {
                    scoreRating = "Best Ending\n\nYou managed to get away safely.";
                    rank = Ranking[0]; // SS
                }
                else
                {
                    if (player.misses <= 3)
                    {
                        ratingColor = Color.Gold;
                        scoreRating = "Good Ending\n\nYou barely got away alive.";
                        rank = Ranking[1]; // A
                    }
                    else
                    {
                        ratingColor = Color.Gray;
                        scoreRating = "Bad Ending\n\nThere are too many of them!";
                        rank = Ranking[2]; // C
                    }
                }
            }
            rank.Scale = 0;
        }
    }
}

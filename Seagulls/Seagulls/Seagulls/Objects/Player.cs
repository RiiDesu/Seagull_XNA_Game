using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Seagulls
{
    class Player : Sprite
    {
        const string Player_ASSETNAME = "player";
        private Texture2D sCoolDownBar, sCoolDownFrame;
        private Rectangle PosCoolDownBar, PosCoolDownFrame;
        private SpriteFont game_font;
        private MouseState aMouse;
        private Stopwatch Timer;
        private TimeSpan time;
        private int ClickCooldown;

        public int x, y, score, misses;
        public bool MouseClick, active;

        public void LoadContent(ContentManager theContentManager) 
        {
            active = true;
            Position = new Vector2(0, 0);
            base.LoadContent(theContentManager, Player_ASSETNAME);
            
            sCoolDownBar = theContentManager.Load<Texture2D>("bar_bg");
            sCoolDownFrame = theContentManager.Load<Texture2D>("bar_Cooldown");
            PosCoolDownBar = new Rectangle(250, 448, 300, 10);
            PosCoolDownFrame = new Rectangle(248, 434, 304, 26);
            
            game_font = theContentManager.Load<SpriteFont>("font");
            MouseClick = false;
            score = 0; misses = 0;
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Update(GameTime theGameTime) 
        {
            if (!active) 
            {
                Position.X = -100;
                Position.Y = -100;
            }
            else
            {
                // Mouse Position
                aMouse = Mouse.GetState();
                x = aMouse.X;
                y = aMouse.Y;
                Position.X = x - (Size.Width / 2);
                Position.Y = y - (Size.Height / 2);
                if (ClickCooldown >= 1) { ClickCooldown--; }
                if ((aMouse.LeftButton == ButtonState.Pressed) && (ClickCooldown <= 0))
                {
                    MouseClick = true;
                    ClickCooldown = 30;
                }
                else { MouseClick = false; }
            }
        }

        public void DrawGUI(SpriteBatch theSpriteBatch) 
        {
            //Cooldown Bar
            PosCoolDownBar.Width = ClickCooldown * 10;
            theSpriteBatch.Draw(sCoolDownFrame, PosCoolDownFrame, Color.White);
            theSpriteBatch.Draw(sCoolDownBar, PosCoolDownBar, Color.Red);

            //Score
            string Score = "Score: ", Misses = "Misses: ";
            Score += score;
            Misses += misses;

            theSpriteBatch.DrawString(game_font, Score, new Vector2(10, 420), Color.White);
            theSpriteBatch.DrawString(game_font, Misses, new Vector2(10, 435), Color.White);

            //Timer
            string Time;
            time = Timer.Elapsed;

            Time = "Time Elapsed: " + String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            if (time.Minutes == 1 && time.Seconds == 30)
            {
                Timer.Stop();
                Time = "Time Elapsed: 01:30 - TIME'S UP!";
                active = false;
            }

            theSpriteBatch.DrawString(game_font, Time, new Vector2(10, 450), Color.White);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Seagulls
{
    class Enemy : Sprite
    {
        const string Enemy_ASSETNAME = "enemy";
        int START_POSITION_X;
        int START_POSITION_Y;
        int Enemy_SPEED;
        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        //public Enemy(int ID) { }
        public Enemy() { }
        
        public void LoadContent(ContentManager theContentManager)
        {
            Random rnd = new Random();

            START_POSITION_X = rnd.Next(30, 750);
            START_POSITION_Y = rnd.Next(300);
            Enemy_SPEED = rnd.Next(150, 300);

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, Enemy_ASSETNAME);
            
            // random start direction
            bool randX = rnd.Next(0, 2) == 0,
                 randY = rnd.Next(0, 2) == 0;
            if (!randX) { mDirection.X = -1; }
            else { mDirection.X = 1; DrawFlipped(false, true); }
            if (!randY) { mDirection.Y = -1; }
            else { mDirection.Y = 1; }
        }

        public void Update(GameTime theGameTime)
        {
            UpdateMovement();

            base.Update(theGameTime, mSpeed, mDirection);
        }

        private void UpdateMovement()
        {
            int MaxX = 800 - Size.Width;
            int MaxY = 350 - Size.Height;
            mSpeed.X = Enemy_SPEED;
            mSpeed.Y = Enemy_SPEED;

            if (Position.X > MaxX) //left
            {
                mDirection.X = -1;
                DrawFlipped(false, true);
            }
            else if (Position.X < 0) //right
            {
                mDirection.X = 1;
                DrawFlipped(false, true);
            }

            if (Position.Y > MaxY) //up
            {
                mDirection.Y = -1;
            }
            else if (Position.Y < 0) //down
            {
                mDirection.Y = 1;
            }
        }
        
    }
}

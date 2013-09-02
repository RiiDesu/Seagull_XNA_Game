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
        string Enemy_ASSETNAME = "EnemyRight";
        private Vector2 mDirection = Vector2.Zero;
        private Vector2 mSpeed = Vector2.Zero;
        public bool Active = true;

        public Enemy(ContentManager theContentManager, Vector2 theSpeed, Vector2 thePosition, Vector2 theDirection) 
        {
            mSpeed = theSpeed;
            Position = thePosition;
            mDirection = theDirection;

            AssetLeft = "EnemyLeft";
            AssetRight = "EnemyRight";
            base.LoadContent(theContentManager, Enemy_ASSETNAME);
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

            if (Position.X > MaxX) //left
            {
                mDirection.X = -1;
                //DrawFlipped(false, true);
            }
            else if (Position.X < 0) //right
            {
                mDirection.X = 1;
                //DrawFlipped(false, true);
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

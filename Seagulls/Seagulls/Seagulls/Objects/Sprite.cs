using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Seagulls
{
    class Sprite
    {
        public Vector2 Position = new Vector2(0, 0);
        private Texture2D mSpriteTexture;
        public string AssetName;
        public Rectangle Size;
        private float mScale = 1.0f;
        
        public float Scale 
        {
            get { return mScale; }
            set 
            {
                mScale = value;
                Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), 
                                     (int)(mSpriteTexture.Height * Scale));
            }
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), 
                                 (int)(mSpriteTexture.Height * Scale));
        }

        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                                new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height),
                                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public Texture2D DrawFlipped(bool vertical, bool horizontal) 
        {
            Texture2D flipped = mSpriteTexture;

            Color[] data = new Color[mSpriteTexture.Width * mSpriteTexture.Height];
            Color[] flippedData = new Color[data.Length];

            mSpriteTexture.GetData(data);

            for (int x = 0; x < mSpriteTexture.Width; x++)
                for (int y = 0; y < mSpriteTexture.Height; y++)
                {
                    int idx = (horizontal ? mSpriteTexture.Width - 1 - x : x) + ((vertical ? mSpriteTexture.Height - 1 - y : y) * mSpriteTexture.Width);
                    flippedData[x + y * mSpriteTexture.Width] = data[idx];
                }

            flipped.SetData(flippedData);

            return flipped;
        }
    }
}

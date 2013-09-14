using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Seagulls
{
    class Sprite
    {
        public Vector2 Position = new Vector2(0, 0);
        private Texture2D mSpriteTexture;
        public Texture2D Sprite_Left, Sprite_Right;
        public string AssetName, AssetLeft, AssetRight;
        public Rectangle Size;
        private float mScale = 1.0f;
        
        public float Scale 
        {
            get { return mScale; }
            set 
            {
                mScale = value;
                Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            }
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            if ((AssetLeft == null) || (AssetRight == null)) { }
            else 
            {
                Sprite_Left = theContentManager.Load<Texture2D>(AssetLeft);
                Sprite_Right = theContentManager.Load<Texture2D>(AssetRight);
            }
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), 
                                 (int)(mSpriteTexture.Height * Scale));
        }

        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            if ((mSpriteTexture.Name == null) || (Sprite_Left.Name == null) || (Sprite_Right.Name == null)){}
            else 
            {
                if (theDirection.X == 1) { mSpriteTexture = Sprite_Right; }
                else { mSpriteTexture = Sprite_Left; }
            }
            
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                                new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height),
                                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}

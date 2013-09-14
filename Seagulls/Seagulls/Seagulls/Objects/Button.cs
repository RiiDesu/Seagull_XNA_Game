using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Seagulls
{
    class Button : Sprite
    {
        private string button_ASSETNAME;
        private MouseState mouse;
        private MouseState oldMouse;
        public Rectangle Area;
        public bool clicked = false;

        public void LoadContent(ContentManager theContentManager, string theAssetName, int x, int y) 
        {
            Position.X = x;
            Position.Y = y;
            button_ASSETNAME = theAssetName;

            base.LoadContent(theContentManager, button_ASSETNAME);
            Area.X = x;
            Area.Y = y;
            Area.Width = Size.Width;
            Area.Height = Size.Height;
        }

        public void Update() 
        {
            mouse = Mouse.GetState();

            Area.X = (int)Position.X;
            Area.Y = (int)Position.Y;
            Area.Width = Size.Width;
            Area.Height = Size.Height;

            clicked = false;

            if (mouse.LeftButton == ButtonState.Released && (oldMouse.LeftButton == ButtonState.Pressed))
            {
                if (Area.Contains(new Point(mouse.X, mouse.Y)))
                {
                    clicked = true;
                }
            }
            oldMouse = mouse;
        }
    }
}

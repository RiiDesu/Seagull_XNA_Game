using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Seagulls
{
    class Button : Sprite
    {
        private string button_ASSETNAME;

        public void LoadContent(ContentManager theContentManager, string theAssetName, int x, int y) 
        {
            Position.X = x;
            Position.Y = y;
            button_ASSETNAME = theAssetName;

            base.LoadContent(theContentManager, button_ASSETNAME);
        }
    }
}

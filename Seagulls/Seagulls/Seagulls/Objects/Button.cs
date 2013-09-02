using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Seagulls
{
    class Button : Sprite
    {
        enum Bstate { IDLE, CLICKED, DISABLED }
        const int BUTTON_COUNT = 1,
                  BTN_GAME_QUIT = 401,
                  BTN_GAME_PAUSE = 402,
                  BTN_MENU_START = 403,
                  BTN_MENU_OPTIONS = 404,
                  BTN_MENU_EXIT = 405,
                  BTN_SCORE_RETRY = 406,
                  BTN_SCORE_RETURN = 407,
                  BTN_OPTIONS_MUSIC_ON = 408,
                  BTN_OPTIONS_MUSIC_OFF = 409;
        
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

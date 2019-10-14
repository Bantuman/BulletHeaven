using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    class GameMeta
    {
        public TextureCache TextureCache { get; }
        public Game1 GameHandle { get; }
        public InputManager InputManager { get; }
        
        public GameMeta(TextureCache someTextureCache, Game1 someGameHandle, InputManager someInputManager)
        {
            TextureCache = someTextureCache;
            GameHandle = someGameHandle;
            InputManager = someInputManager;
        }
    }
}

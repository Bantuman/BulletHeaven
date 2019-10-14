using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    class TextureCache
    {
        private Dictionary<String, Texture2D> textureCache;
        public Texture2D this[string textureIndex]
        {
            get { return textureCache[textureIndex]; }
            set { textureCache[textureIndex] = value; }
        }
        public TextureCache(ContentManager content)
        {
            textureCache = new Dictionary<string, Texture2D>()
            {
                ["PlayerSprite"] = content.Load<Texture2D>("cirno the phantom"),
            };
        }
    }
}

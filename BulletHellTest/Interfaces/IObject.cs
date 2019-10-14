using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest.Interfaces
{
    interface IObject
    {
        Rectangle Rectangle { get; }
        Vector2 Position { get; set; }
        Point Size { get; set; }
        int RenderIndex { get; set; }
        Texture2D RenderTexture { get; set; }
        
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}

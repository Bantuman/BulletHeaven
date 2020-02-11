using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    class Frame : UIObject
    {
        public Frame(UIObject frameParent, Texture2D frameTexture, UDim framePosition, UDim frameSize)
        {
            Parent = frameParent ?? Game1.WINDOW_HANDLE;
            Position = framePosition;
            Size = frameSize;
            Texture = frameTexture;
        }

        public void Draw(SpriteBatch someSpriteBatch)
        {
            someSpriteBatch.Draw(Texture, GetRectangle(), null, Color, 0, Vector2.Zero, SpriteEffects.None, ZIndex);
        }

        public void Update(GameTime someGameTime)
        { }
    }
}

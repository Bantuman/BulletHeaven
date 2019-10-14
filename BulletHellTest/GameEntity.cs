using BulletHellTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHellTest
{
    abstract class GameEntity : IObject
    {
        public virtual Rectangle Rectangle { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual Point Size { get; set; }
        public virtual int RenderIndex { get; set; }
        public virtual Texture2D RenderTexture { get; set; }
        public GameMeta GameMeta { get; set; }

        public GameEntity(GameMeta gameMeta)
        {
            GameMeta = gameMeta;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RenderTexture, Rectangle, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}

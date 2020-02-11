using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    class Projectile : GameEntity
    {
        public override Rectangle Rectangle { get => new Rectangle(Position.ToPoint(), Size); }
        public override Point Size { get => new Point(8, 8); }
        public Vector2 Direction { get; set; }
        public float Velocity { get; set; }

        public Projectile(GameMeta gameMeta) : base(gameMeta)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RenderTexture, Rectangle, null, Color.White, 0, RenderTexture.Bounds.Size.ToVector2() * 0.5f, SpriteEffects.None, 1);
        }

        private void MoveBy(Vector2 deltaVector)
        {
            Vector2 newPosition = Position + deltaVector;
            if ((GameMeta.GameHandle.currentScene as Stage).CleanupBounds.Contains(newPosition))
            {
                Position = newPosition;
                return;
            }

            // destroy projectile??
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MoveBy(Direction * Velocity * deltaTime);
        }
    }
}

using BulletHellTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BulletHellTest
{
    class Player : GameEntity
    {
        public override Rectangle Rectangle { get => new Rectangle(Position.ToPoint(), Size); }
        public override Point Size { get => new Point(32, 64); }

        public Player(GameMeta gameMeta) : base(gameMeta)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RenderTexture, Rectangle, null, Color.White, 0, RenderTexture.Bounds.Size.ToVector2() * 0.5f, SpriteEffects.None, 1);
        }

        private void MoveBy(Vector2 deltaVector)
        {
            Vector2 newPosition = Position + deltaVector;
            if ((GameMeta.GameHandle.currentScene as Stage).SceneBounds.Contains(newPosition))
            {
                Position = newPosition;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 tempVelocity = new Vector2(0, 0);
            if (GameMeta.InputManager[Keys.A])
            {
                tempVelocity += new Vector2(-1, 0);
            }
            if (GameMeta.InputManager[Keys.D])
            {
                tempVelocity += new Vector2(1, 0);
            }
            if (GameMeta.InputManager[Keys.W])
            {
                tempVelocity += new Vector2(0, -1);
            }
            if (GameMeta.InputManager[Keys.S])
            {
                tempVelocity += new Vector2(0, 1);
            }

            if (tempVelocity.Length() > 0)
            {
                tempVelocity = Vector2.Normalize(tempVelocity) * 5;
                MoveBy(new Vector2(tempVelocity.X, 0));
                MoveBy(new Vector2(0, tempVelocity.Y));
            }
        }
    }
}

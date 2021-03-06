﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletHellTest.Interfaces;

namespace BulletHellTest
{
    class Stage : IScene
    {
        public List<IObject> Scenery { get; private set; }
        public Player Player { get; private set; }
        public GameMeta CurrentMeta { get; private set; }
        public Rectangle SceneBounds { get; private set; }
        public Rectangle CleanupBounds {
            get {
                Rectangle bound = SceneBounds;
                bound.Inflate(128, 128);
                return bound;
            }
        }

        public Stage(GameMeta GameMeta)
        {
            CurrentMeta = GameMeta;
            SceneBounds = new Rectangle(0, 0, 512, 764);
            Scenery = new List<IObject>();
            Player = new Player(GameMeta)
            {
                RenderTexture = CurrentMeta.TextureCache["PlayerSprite"],
                Position = new Vector2(SceneBounds.X + SceneBounds.Width / 2, SceneBounds.Height - 128),
            };

            Scenery.Add(Player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = Scenery.Count - 1; i >= 0; --i)
            {
                Scenery[i].Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            int realHeight = SceneBounds.Height;
            int realWidth = (int)(SceneBounds.Width * ((float)realHeight) / SceneBounds.Height);
            spriteBatch.Draw(CurrentMeta.TextureCache["Pixel"], new Rectangle(0, 0, (764 - realWidth) / 2, realHeight), Color.Black);
            spriteBatch.Draw(CurrentMeta.TextureCache["Pixel"], new Rectangle(764 - (764 - realWidth) / 2, 0, (764 - realWidth) / 2, realHeight), Color.Black);
            spriteBatch.End();
        }

        public IScene Update(GameTime gameTime)
        {
            for (int i = Scenery.Count - 1; i >= 0; --i)
            {
                Scenery[i].Update(gameTime);
            }
            return this;
        }
    }
}

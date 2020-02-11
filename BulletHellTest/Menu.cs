using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletHellTest.Interfaces;
using System.Diagnostics;

namespace BulletHellTest
{
    class Menu : IScene
    {
        private float currentButtonIndex;
        private List<Button> menuButtons;
        private IScene currentScene;

        public GameMeta CurrentMeta { get; private set; }
        public Menu(GameMeta GameMeta)
        {
            CurrentMeta = GameMeta;
            menuButtons = new List<Button>();
            currentScene = this;
            menuButtons.Add(new Button(null, CurrentMeta.TextureCache["Button"], new UDim(new Vector2(0.5f, 0.5f), new Vector2(-50, -55)), new UDim(Vector2.Zero, new Vector2(100, 50)), () =>
            {
                currentScene = new Stage(CurrentMeta);
            }));
            menuButtons.Add(new Button(null, CurrentMeta.TextureCache["Button"], new UDim(new Vector2(0.5f, 0.5f), new Vector2(-50, 55)), new UDim(Vector2.Zero, new Vector2(100, 50)), () =>
            {
                currentScene = new LevelEditor(CurrentMeta);
            }));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach(Button button in menuButtons)
            {
                button.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public IScene Update(GameTime gameTime)
        {
            foreach (Button button in menuButtons)
            {
                button.Update(gameTime);
            }
            return currentScene;
        }
    }
}

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
    class LevelEditor : IScene
    {
        public List<IObject> Scenery { get; private set; }
        public Player Player { get; private set; }
        public GameMeta CurrentMeta { get; private set; }
        public Rectangle SceneBounds { get; private set; }
        public Rectangle CleanupBounds
        {
            get
            {
                Rectangle bound = SceneBounds;
                bound.Inflate(128, 128);
                return bound;
            }
        }

        private List<UIObject> UI;
        private List<LevelKeypoint> LevelData;
        private float cursorStep = 0.25f;
        private float levelLength = 100;
        private float levelPosition = 50;
        private bool cursorDragging = false;
        private int cursorWidth = 10;
        private Rectangle GetCursorRectangle(Point offset)
        {
            return new Rectangle(offset + new Point((int)((levelPosition / levelLength) * (664 - cursorWidth)), 0), new Point(cursorWidth, 24));
        }

        public LevelEditor(GameMeta GameMeta)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".dmk"; // Default file extension
            dlg.Filter = "Danmaku (.dmk)|*.dmk"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
            }

            CurrentMeta = GameMeta;
            SceneBounds = new Rectangle(0, 0, 512, 764);
            Scenery = new List<IObject>();
            LevelData = new List<LevelKeypoint>();
            UI = new List<UIObject>();

            BuildUI();
        }

        public void BuildUI()
        {
            Texture2D frameTexture = CurrentMeta.TextureCache["Pixel"];
            UI.Add(new Frame(null, frameTexture, new UDim(0, 0, 0, 0), new UDim(1, 0, 1, 0)) { Color = Color.Black });
            UI.Add(new Frame(null, frameTexture, new UDim(0, 0, 1, -128), new UDim(1, 0, 0, 128)) { Color = new Color(0.8f, 0.8f, 0.8f) });
            UI.Add(new Frame(null, frameTexture, new UDim(0.5f, -256, 0, 0), new UDim(0, 512, 1, -128)) { Color = Color.DarkMagenta });
        }

        public void AddDataRaw(float timePosition, string data)
        {
            foreach(LevelKeypoint keypoint in LevelData)
            {
                if (keypoint.TimePosition == timePosition)
                {
                    keypoint.AddData(data);
                    return;
                }
            }
            LevelData.Add(new LevelKeypoint(timePosition, data));
        }

        public void RemoveDataRaw(float timePosition, string data)
        {
            foreach (LevelKeypoint keypoint in LevelData)
            {
                if (keypoint.TimePosition == timePosition)
                {
                    if (data == "@CLEAN")
                    {
                        LevelData.Remove(keypoint);
                        return;
                    }
                    keypoint.RemoveData(data);
                }
            }
        }

        public Vector2 StringDataToVector2(string vector2)
        {
            string[] vector2data = vector2.Split(',');
            return new Vector2(float.Parse(vector2data[0]), float.Parse(vector2data[1]));
        }

        public void AddBulletSpawn(BulletSpawn bulletSpawn) => AddBulletSpawn(bulletSpawn, levelPosition);
        public void AddBulletSpawn(BulletSpawn bulletSpawn, float timePosition)
        {
            AddDataRaw(timePosition, bulletSpawn.ParsedData);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = Scenery.Count - 1; i >= 0; --i)
            {
                Scenery[i].Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            foreach(UIObject uiObject in UI)
            {
                if (uiObject is Button button)
                {
                    button.Draw(spriteBatch);
                }
                else if(uiObject is Frame frame)
                {
                   frame.Draw(spriteBatch);
                }
            }

            if (CurrentMeta.InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                AddBulletSpawn(new BulletSpawn(CurrentMeta.InputManager.GetMousePosition().ToVector2(), Vector2.UnitY * 10));
            }

            foreach(LevelKeypoint keypoint in LevelData)
            {
                foreach(string keypointData in keypoint.KeypointData)
                {
                    string[] positionData = keypointData.Split('.');
                    Vector2 spawnPosition = StringDataToVector2(positionData[0]);
                    Vector2 spawnVelocity = StringDataToVector2(positionData[1]);

                    float tempAlpha = Math.Abs(levelPosition - keypoint.TimePosition) * 5;
                    Rectangle tempRectangle = new Rectangle(spawnPosition.ToPoint(), new Point(16, 16));
                    if (tempRectangle.Contains(CurrentMeta.InputManager.GetMousePosition()) && CurrentMeta.InputManager.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete))
                    {
                        RemoveDataRaw(keypoint.TimePosition, keypointData);
                    }

                    tempAlpha = 1 - (((float)Math.Round(tempAlpha / 40) * 40) / levelLength);
                    spriteBatch.Draw(CurrentMeta.TextureCache["Pixel"], tempRectangle, Color.Red * tempAlpha);
                    spriteBatch.DrawString(CurrentMeta.GameHandle.Content.Load<SpriteFont>("Default"), keypoint.TimePosition.ToString(), spawnPosition, Color.White * tempAlpha);
                }
            }


            // DRAWING TIMELINE
            Point timelinePosition = new Point(50, 764 - 128 + 10);
            Rectangle cursorRectangle = GetCursorRectangle(timelinePosition);
            spriteBatch.Draw(CurrentMeta.TextureCache["Pixel"], new Rectangle(timelinePosition, new Point(664, 24)), new Color(0.5f, 0.5f, 0.5f));
            spriteBatch.Draw(CurrentMeta.TextureCache["Pixel"], cursorRectangle, new Color(1f, 0.2f, 0.2f));

            // DRAWING CURRENT POS
            spriteBatch.DrawString(CurrentMeta.GameHandle.Content.Load<SpriteFont>("Default"), levelPosition.ToString(), new Vector2(50, 50), Color.White);
            spriteBatch.End();
        }

        public IScene Update(GameTime gameTime)
        {
            for (int i = Scenery.Count - 1; i >= 0; --i)
            {
                Scenery[i].Update(gameTime);
            }

            Point mousePosition = CurrentMeta.InputManager.GetMousePosition();
            Point timelinePosition = new Point(50, 764 - 128 + 10);
            Rectangle cursorRectangle = GetCursorRectangle(timelinePosition);

            if (cursorRectangle.Contains(mousePosition) && CurrentMeta.InputManager.IsMouseLeftDown())
            {
                cursorDragging = true;
            }

            if (!CurrentMeta.InputManager.IsMouseLeftDown())
            {
                cursorDragging = false;
            }

            if (cursorDragging)
            {
                levelPosition = (float)Math.Round((double)MathHelper.Clamp(((mousePosition.X - 50f) / (664f - cursorWidth)) * levelLength, 0, levelLength) / cursorStep) * cursorStep;
            }

            if (CurrentMeta.InputManager[Microsoft.Xna.Framework.Input.Keys.E])
            {
                levelPosition = MathHelper.Clamp(levelPosition + cursorStep, 0, levelLength);
            }
            else if(CurrentMeta.InputManager[Microsoft.Xna.Framework.Input.Keys.Q])
            {
                levelPosition = MathHelper.Clamp(levelPosition - cursorStep, 0, levelLength);
            }

            return this;
        }
    }
}

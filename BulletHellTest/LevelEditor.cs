using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletHellTest.Interfaces;
using System.Diagnostics;
using NAudio;
using NAudio.Wave;
using System.IO;

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
        private Frame cursorObject;
        private float cursorStep = 0.25f;
        private float levelLength = 100;
        private float levelPosition = 50;
        private bool cursorDragging = false;
        private int cursorWidth = 10;

        public LevelEditor(GameMeta GameMeta)
        {

            CurrentMeta = GameMeta;
            SceneBounds = new Rectangle(0, 0, 512, 764);
            Scenery = new List<IObject>();
            LevelData = new List<LevelKeypoint>();
            UI = new List<UIObject>();

            BuildUI();
        }

        public void SaveLevel()
        {
            string levelData = EncryptLevel();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".dmk"; // Default file extension
            dlg.Filter = "Danmaku (.dmk)|*.dmk"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (dlg.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                File.WriteAllText(dlg.FileName, levelData);
            }
        }

        public void LoadLevel()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".dmk"; // Default file extension
            dlg.Filter = "Danmaku (.dmk)|*.dmk"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                StreamReader reader = new StreamReader(dlg.OpenFile());
                ParseLevel(reader.ReadToEnd());
            }
        }

        public void LoadSong()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
          //  dlg.DefaultExt = ".mp3"; // Default file extension
          //  dlg.Filter = "Mp3 |*.dmk"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                Mp3FileReader reader = new Mp3FileReader(dlg.OpenFile());
                SetLength((float)reader.TotalTime.TotalSeconds);
                string filename = dlg.FileName;
               // levelPlayer = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
               // levelPlayer.Load(dlg.OpenFile());
       
               // levelPlayer.Play();
            }
        }

        public void BuildUI()
        {
            Texture2D frameTexture = CurrentMeta.TextureCache["Pixel"];
            Texture2D buttonTexture = CurrentMeta.TextureCache["Button"];
            UI.Add(new Frame(null, frameTexture, new UDim(0, 0, 0, 0), new UDim(1, 0, 1, 0)) { Color = Color.Black });
            UI.Add(new Frame(null, frameTexture, new UDim(0, 0, 1, -128), new UDim(1, 0, 0, 128)) { Color = new Color(0.8f, 0.8f, 0.8f) });
            Frame tempGameBorder = new Frame(null, frameTexture, new UDim(0.5f, -256 - 64, 0, 0), new UDim(0, 512 - 128, 1, -128)) { Color = Color.DarkMagenta };

            // timeline
            Frame cursorBar = new Frame(null, frameTexture, new UDim(0, 8, 1, -128 + 8), new UDim(1, -16 - cursorWidth, 0, 24)) { Color = new Color(0.5f, 0.5f, 0.5f) };
            cursorObject = new Frame(cursorBar, frameTexture, new UDim(0, 0, 0, 0), new UDim(0, cursorWidth, 1, 0)) { Color = Color.Red };

            UI.Add(new Button(tempGameBorder, buttonTexture, new UDim(1, 100, 0.5f, -100), new UDim(0, 150, 0, 50), () =>
            {
                LoadSong();
            })
            {Color = Color.Green});
            UI.Add(new Button(tempGameBorder, buttonTexture, new UDim(1, 100, 0.5f, -160), new UDim(0, 150, 0, 50), () => {
                LoadLevel();
            }));
            UI.Add(new Button(tempGameBorder, buttonTexture, new UDim(1, 100, 0.5f, -40), new UDim(0, 150, 0, 50), () => {
                SaveLevel();
            }));

            UI.Add(tempGameBorder);
            UI.Add(new Frame(cursorBar, frameTexture, new UDim(1, 0, 0, 0), new UDim(0, cursorWidth, 1, 0)) { Color = cursorBar.Color });
            UI.Add(cursorBar);
            UI.Add(cursorObject);
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

        public void SetLength(float someLength)
        {
            levelLength = someLength;
            levelPosition = 0;
        }

        // WILL OVERWRITE THE CURRENT LEVEL
        public void ParseLevel(string levelData)
        {
            string[] initialDataSplit = levelData.Split(']');
            string[] levelDataStr = initialDataSplit[1].Split('@');

            SetLength(float.Parse(initialDataSplit[0]));
            LevelData = new List<LevelKeypoint>();
            foreach (string keypointDataStr in levelDataStr)
            {
                float timePosition = 0;
                foreach (string bulletDataStr in keypointDataStr.Split('['))
                {
                    string[] bulletDataSplit = bulletDataStr.Split('!');
                    if (bulletDataSplit.Length >= 2)
                    {
                        timePosition = float.Parse(bulletDataSplit[0]);
                        AddDataRaw(timePosition, bulletDataSplit[1]);
                    }
                    else if(bulletDataStr.Length > 0)
                    {
                        AddDataRaw(timePosition, bulletDataStr);
                    }
                    //    Vector2 spawnPosition = StringDataToVector2(spawnPositionSplit[0]);
                    //    Vector2 spawnVelocity = StringDataToVector2(spawnPositionSplit[1]);
                }
            }
        }

        public string EncryptLevel()
        {
            string levelStr = string.Format("{0}]", levelLength);
            foreach (LevelKeypoint keypoint in LevelData)
            {
                string keypointStr = "";
                foreach (string keypointData in keypoint.KeypointData)
                {
                    keypointStr += keypointData + "[";
                }
                levelStr += keypoint.TimePosition.ToString() + "!" + keypointStr + "@";
            }
            return levelStr;
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

            int totalNumber = 0;
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

                    totalNumber++;
                    tempAlpha = 1 - (((float)Math.Round(tempAlpha / 40) * 40) / levelLength);
                    spriteBatch.Draw(CurrentMeta.TextureCache["Pixel"], tempRectangle, Color.Red * tempAlpha);
                    spriteBatch.DrawString(CurrentMeta.GameHandle.Content.Load<SpriteFont>("Default"), keypoint.TimePosition.ToString(), spawnPosition, Color.White * tempAlpha);
                }
            }

            spriteBatch.DrawString(CurrentMeta.GameHandle.Content.Load<SpriteFont>("Default"), totalNumber.ToString(), new Vector2(150, 50), Color.White);
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
            foreach (UIObject uiObject in UI)
            {
                if (uiObject is Button button)
                {
                    button.Update(gameTime);
                }
            }

            Point mousePosition = CurrentMeta.InputManager.GetMousePosition();
            Rectangle cursorRectangle = cursorObject.GetRectangle();
            Rectangle timelineRectangle = cursorObject.Parent.GetRectangle();

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
                levelPosition = (float)Math.Round((double)MathHelper.Clamp((((float)mousePosition.X - timelineRectangle.Location.X) / ((float)timelineRectangle.Width)) * levelLength, 0, levelLength) / cursorStep) * cursorStep;
            }

            if (CurrentMeta.InputManager[Microsoft.Xna.Framework.Input.Keys.E])
            {
                levelPosition = MathHelper.Clamp(levelPosition + cursorStep, 0, levelLength);
            }
            else if(CurrentMeta.InputManager[Microsoft.Xna.Framework.Input.Keys.Q])
            {
                levelPosition = MathHelper.Clamp(levelPosition - cursorStep, 0, levelLength);
            }

            cursorObject.Position = new UDim(levelPosition / levelLength, 0, 0, 0);

            return this;
        }
    }
}

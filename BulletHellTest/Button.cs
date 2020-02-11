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
    class Button : UIObject
    {
        public bool IsSelected { get; set; }
        public Action OnClick { get; set; }

        private bool wasMouseSelected;
        private float lastClick;
        private float clickDebounce;

        public Button(UIObject buttonParent, Texture2D buttonTexture, UDim buttonPosition, UDim buttonSize, Action buttonOnClick) : this(buttonParent, buttonTexture, buttonPosition, buttonSize, buttonOnClick, 1.0f) { }
        public Button(UIObject buttonParent, Texture2D buttonTexture, UDim buttonPosition, UDim buttonSize, Action buttonOnClick, float buttonDebounce)
        {
            Parent = buttonParent ?? Game1.WINDOW_HANDLE;
            Position = buttonPosition;
            Size = buttonSize;
            OnClick = buttonOnClick;

            Texture = buttonTexture;
            clickDebounce = buttonDebounce;
        }

        public void Draw(SpriteBatch someSpriteBatch)
        {
            someSpriteBatch.Draw(Texture, GetRectangle(), null, Color, 0, Vector2.Zero, SpriteEffects.None, 1);
            if (IsSelected)
            {

            }
        }

        public void Update(GameTime someGameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            Rectangle tempCurrentRectangle = GetRectangle();

            if (!IsSelected && tempCurrentRectangle.Contains(mouseState.Position))
            {
                IsSelected = true;
                wasMouseSelected = true;
            }
            else if (wasMouseSelected && !tempCurrentRectangle.Contains(mouseState.Position))
            {
                IsSelected = false;
                wasMouseSelected = false;
            }
            lastClick += (float)someGameTime.ElapsedGameTime.TotalSeconds;
            if (lastClick > clickDebounce && ((IsSelected && !wasMouseSelected && keyState.IsKeyDown(Keys.Enter))
                || (IsSelected && wasMouseSelected && mouseState.LeftButton == ButtonState.Pressed)))
            {
                lastClick = 0;
                OnClick.Invoke();
            }
        }
    }
}

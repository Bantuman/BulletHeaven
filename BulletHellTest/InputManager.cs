using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BulletHellTest
{
    class InputManager
    {
        private Dictionary<Keys, bool> currentInput;
        private Dictionary<Keys, bool> previousInput;

        public bool this[Keys key]
        {
            get { return currentInput.ContainsKey(key); }
            set { currentInput[key] = value; }
        }

        private bool checkKey(Dictionary<Keys, bool> dictionary, Keys key)
        {
            bool down = false;
            if (dictionary.ContainsKey(key))
            {
                down = dictionary[key];
            }
            return down;
        }

        public bool IsKeyDown(Keys key)
        {
            return checkKey(currentInput, key) && !checkKey(previousInput, key);
        }

        public Point GetMousePosition()
        {
            return Mouse.GetState().Position;
        }

        public bool IsMouseLeftDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public InputManager()
        {
            previousInput = currentInput = new Dictionary<Keys, bool>();
        }

        public void Update()
        {
            previousInput = currentInput;
            currentInput = new Dictionary<Keys, bool>();
            foreach(Keys key in Keyboard.GetState().GetPressedKeys())
            {
                currentInput[key] = true;
            }
        }
    }
}

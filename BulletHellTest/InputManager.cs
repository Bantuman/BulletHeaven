using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool IsKeyDown(Keys key)
        {
            return currentInput[key] && !previousInput[key];
        }

        public InputManager()
        {
            currentInput = previousInput = new Dictionary<Keys, bool>();
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

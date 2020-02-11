using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    public class UIObject
    {
        public UDim Position { get; set; }
        public UDim Size { get; set; }
        public UIObject Parent { get; set; }
        public Texture2D Texture { get; set; }
        public Color Color { get; set; } = Color.White;
        public float ZIndex { get; set; } = 0.5f;

        public Rectangle GetRectangle()
        {
            Vector2 size,
                    position;

            if (Parent == null)
            {
                size = Size.Offset;
                position = Position.Offset;
                return new Rectangle(position.ToPoint(), size.ToPoint());
            }

            Rectangle rectangle = Parent.GetRectangle();
            size = (rectangle.Size.ToVector2() * Size.Scale) + Size.Offset;
            position = rectangle.Location.ToVector2() + (rectangle.Size.ToVector2() * Position.Scale) + Position.Offset;
            return new Rectangle(position.ToPoint(), size.ToPoint());
        }
    }
}

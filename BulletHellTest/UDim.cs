using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellTest
{
    public class UDim
    {
        private Vector2 myScale;
        private Vector2 myOffset;

        public Vector2 Scale { get => myScale; set => myScale = value; }
        public Vector2 Offset { get => myOffset; set => myOffset = value; }

        public UDim(float scaleX, float offsetX, float scaleY, float offsetY) : this(new Vector2(scaleX, scaleY), new Vector2(offsetX, offsetY)) { }
        public UDim(Vector2 someScale, Vector2 someOffset)
        {
            myScale = someScale;
            myOffset = someOffset;
        }

        public static UDim operator +(UDim first, UDim second)
        {
            return new UDim(first.Scale + second.Scale, first.Offset + second.Offset);
        }

        public static UDim operator *(UDim first, UDim second)
        {
            return new UDim(first.Scale * second.Scale, first.Offset * second.Offset);
        }

        public static UDim operator /(UDim first, UDim second)
        {
            return new UDim(first.Scale / second.Scale, first.Offset / second.Offset);
        }

        public static UDim operator -(UDim first, UDim second)
        {
            return new UDim(first.Scale - second.Scale, first.Offset - second.Offset);
        }
    }
}

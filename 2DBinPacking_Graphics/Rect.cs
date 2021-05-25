using System;
using System.Collections.Generic;
using System.Text;

namespace _2DBinPacking_Graphics
{
    class Rect
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rect(int ID, int X, int Y, int Width, int Height)
        {
            this.ID = ID;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }
        public int getArea()
        {
            return Width * Height;
        }
    }
}

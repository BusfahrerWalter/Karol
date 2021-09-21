namespace Karol.Core
{
    public struct Bounds
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public int Horizontal => Left + Right;
        public int Vertical => Top + Bottom;

        public Bounds(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}

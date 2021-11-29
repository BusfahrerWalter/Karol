using System.Drawing;

namespace Karol.Core.Rendering
{
    internal interface ICustomRenderBehavior2D
    {
        internal Color? Render(Rectangle rect, int stackSize, Graphics g);
    }
}

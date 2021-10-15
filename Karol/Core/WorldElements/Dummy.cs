using Karol.Core.Annotations;
using Karol.Extensions;

namespace Karol.Core.WorldElements
{
    [WorldElementInfo('D')]
    internal class Dummy : WorldElement
    {
        public Dummy(bool canPickUp, bool canStackOnTop) : base(ImageExtension.EmptyBitmap) 
        {
            CanPickUp = canPickUp;
            CanStackOnTop = canStackOnTop;
        }
    }
}

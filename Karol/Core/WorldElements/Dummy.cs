using Karol.Core.Annotations;
using Karol.Extensions;
using System;

namespace Karol.Core.WorldElements
{
    [WorldElementInfo('D', false)]
    internal class Dummy : WorldElement
    {
        private Action onDestroy;

        public Dummy(bool canPickUp, bool canStackOnTop) : base(ImageExtension.EmptyBitmap) 
        {
            CanPickUp = canPickUp;
            CanStackOnTop = canStackOnTop;
        }

        public Dummy(bool canPickUp, bool canStackOnTop, Action onDestroyAction) : this(canPickUp, canStackOnTop)
        {
            onDestroy = onDestroyAction;
        }

        internal override void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}

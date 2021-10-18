using System;

namespace Karol.Core.Rendering
{
    internal class RendererInfoAttribute : Attribute
    {
        public WorldRenderingMode Mode { get; private set; }

        public RendererInfoAttribute(WorldRenderingMode mode)
        {
            Mode = mode;
        }
    }
}

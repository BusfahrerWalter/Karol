using System;

namespace Karol.Core.Rendering
{
    internal class RendererInfoAttribute : Attribute
    {
        private WorldRenderingMode _mode;

        public WorldRenderingMode Mode
        {
            get => _mode;
            set
            {
                ModeIndex = (int)value;
                _mode = value;
            }
        }

        public int ModeIndex { get; private set; }

        public RendererInfoAttribute(WorldRenderingMode mode)
        {
            Mode = mode;
        }

        public RendererInfoAttribute(int modeIndex)
        {
            ModeIndex = modeIndex;
        }
    }
}

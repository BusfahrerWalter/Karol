using Karol.Properties;
using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Karol.Core.Annotations;

namespace Karol.Core.WorldElements
{
    [WorldElementInfo('M', IsCasheable = false)]
    internal class Marker : WorldElement, IContainer
    {
        private event EventHandler onWorldSet;

        /// <summary>
        /// Roboter der auf der Marke steht
        /// </summary>
        public WorldElement Content { get; set; }

        public Point ContentOffset => new Point(0, 33);

        public bool IsEmpty => Content == null;

        internal override string Metadata
        {
            get
            {
                if (IsEmpty) 
                    return string.Empty;

                return Content is Robot r ? $"R{r.FaceDirection.Offset}" : Content.ID.ToString();
            }
            set
            {
                if (!value.StartsWith("R"))
                    return;

                int offset = int.Parse(value[1].ToString());
                onWorldSet += (s, args) =>
                {
                    Robot r = new Robot(Position.X, Position.Z, World, Direction.FromOffset(offset), false, false);
                    r.Mark = this;
                    Content = r;
                };
            }
        }

        public Marker() : base(new Bitmap(Resources.Marke))
        {
            CanStackOnTop = false;
            CanPickUp = false;
            Info2D.FillColor = Color.Yellow;
        }

        public Marker(Robot robot) : this()
        {
            Content = robot;
        }

        public void Reset(bool redraw = true)
        {
            Content = null;
            if(redraw)
                World.Update(Position.X, Position.Z, this);
        }

        internal override void OnWorldSet()
        {
            onWorldSet?.Invoke(this, EventArgs.Empty);
        }

        internal override void OnDestroy()
        {
            Destroy(Content, false);
        }
    }
}

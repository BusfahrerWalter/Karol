using Karol.Core.Annotations;
using Karol.Core.Rendering;
using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Karol.Core
{
    public partial class EditorForm : Form
    {
        private class WorldElementItem
        {
            public string Name { get; set; }
            public char ID { get; set; }

            public WorldElementItem(string name, char iD)
            {
                Name = name;
                ID = iD;
            }

            public WorldElement Get()
            {
                return WorldElement.ForID(ID);
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private World World { get; set; }
        private bool Remove { get; set; }

        public EditorForm(World targetWorld)
        {
            World = targetWorld;
            World.WorldForm.BlockMap.Click += BlockMap_Click;

            InitializeComponent();
            InitializeListBox();
        }

        private void InitializeListBox()
        {
            var list = typeof(WorldElement).Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(WorldElement)) && t.GetCustomAttribute<WorldElementInfoAttribute>().IncludeInEditor)
                .Select(t => new WorldElementItem(t.Name, t.GetCustomAttribute<WorldElementInfoAttribute>().ID))
                .ToArray();

            ActionListBox.Items.AddRange(list);
        }

        private Point PixelToCellPos(int x, int y)
        {
            return new Point((int)Math.Ceiling((double)(x / WorldRenderer2D.EdgeLength)), 
                (int)Math.Ceiling((double)((World.WorldForm.BlockMap.Height - y) / WorldRenderer2D.EdgeLength)));
        }

        private void PlaceBlock(Point pos)
        {
            if (!World.IsPositionValid(pos.X, 0, pos.Y))
                return;

            int stackSize = World.GetStackSize(pos.X, pos.Y);
            if (!World.IsPositionValid(pos.X, stackSize, pos.Y))
                return;

            if (Remove)
            {
                World.SetCell(pos.X, Math.Max(stackSize - 1, 0), pos.Y, null, true);
            }
            else
            {
                var item = ActionListBox.SelectedItem as WorldElementItem;
                var newCell = World.AddToStack(pos.X, pos.Y, item.Get());
                World.Update(pos.X, pos.Y, newCell);
            }
        }

        private void BlockMap_Click(object sender, EventArgs e)
        {
            if (!Visible)
                return;

            var args = e as MouseEventArgs;
            var point = PixelToCellPos(args.X, args.Y);
            PlaceBlock(point);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ActionListBox.ClearSelected();
            Remove = true;
        }

        private void ActionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Remove = false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            World.WorldForm.BlockMap.Click -= BlockMap_Click;
        }
    }
}

using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Karol.Core
{
    internal partial class ControllerForm : Form
    {
        private static Size CollapsedSize = new Size(382, 458);
        private static Size ExpandedSize = new Size(590, 458);

        public event EventHandler onColorChanged;

        public ControllerForm()
        {
            InitializeComponent();
            Size = CollapsedSize;
        }

        private void SelectColorButton_Click(object sender, EventArgs e)
        {
            var erg = ColorDialog.ShowDialog();
            if(erg == DialogResult.OK || erg == DialogResult.Yes)
            {
                var btn = sender as Button;
                btn.BackColor = ColorDialog.Color;
                btn.ForeColor = ColorDialog.Color.Invert();
                OnColorChanged();
            }
        }

        private void OnColorChanged()
        {
            onColorChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MoreInfoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!MoreInfoCheckBox.Checked)
            {
                Size = CollapsedSize;
                MoreInfoCheckBox.Text = "More Info";
            }
            else
            {
                Size = ExpandedSize;
                MoreInfoCheckBox.Text = "Less Info";
            }
        }

        private void LogListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolTip.SetToolTip(LogListBox, LogListBox.SelectedItem.ToString());
        }

        private void ControllerForm_Load(object sender, EventArgs e)
        {
            SetChildToolTips(this);
        }

        private void SetChildToolTips(Control parent)
        {
            foreach(Control ctrl in parent.Controls)
            {
                if(ctrl.Tag is string str)
                {
                    ctrl.MouseHover += (s, args) =>
                    {
                        ToolTip.SetToolTip(s as Control, str);
                    };
                }

                if(ctrl.Controls.Count > 0)
                    SetChildToolTips(ctrl);
            }
        }
    }
}

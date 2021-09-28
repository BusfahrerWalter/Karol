using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Karol.Core
{
    public partial class ControllerForm : Form
    {
        public event EventHandler onColorChanged;

        public ControllerForm()
        {
            InitializeComponent();
        }

        private void SelectColorButton_Click(object sender, EventArgs e)
        {
            var erg = ColorDialog.ShowDialog();
            if(erg == DialogResult.OK || erg == DialogResult.Yes)
            {
                var btn = sender as Button;
                btn.BackColor = ColorDialog.Color;
                OnColorChanged();
            }
        }

        private void OnColorChanged()
        {
            onColorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

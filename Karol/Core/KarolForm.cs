using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Karol
{
    internal class KarolForm : Form
    {
        public PictureBox GridPicture;
        public PictureBox BlockMap;

        public void SetUp(Image image)
        {
            InitializeComponent();
            GridPicture.Size = image.Size;
            GridPicture.Image = image;

            BlockMap = new PictureBox();
            GridPicture.Controls.Add(BlockMap);
            GridPicture.BackColor = Color.Transparent;

            BlockMap.Size = image.Size;
            BlockMap.Location = Point.Empty;
            BlockMap.BackColor = Color.Transparent;
            BlockMap.Image = new Bitmap(image.Size.Width, image.Size.Height);

            Size = new Size(image.Width + 40, image.Height + 60);
        }

        private void InitializeComponent()
        {
            this.GridPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GridPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // GridPicture
            // 
            this.GridPicture.BackColor = System.Drawing.Color.Transparent;
            this.GridPicture.Location = new System.Drawing.Point(9, 9);
            this.GridPicture.Margin = new System.Windows.Forms.Padding(0, 0, 10, 10);
            this.GridPicture.Name = "GridPicture";
            this.GridPicture.Size = new System.Drawing.Size(108, 79);
            this.GridPicture.TabIndex = 0;
            this.GridPicture.TabStop = false;
            // 
            // KarolForm
            // 
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.ClientSize = new System.Drawing.Size(133, 103);
            this.Controls.Add(this.GridPicture);
            this.HelpButton = true;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.Name = "KarolForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Karol World";
            ((System.ComponentModel.ISupportInitialize)(this.GridPicture)).EndInit();
            this.ResumeLayout(false);

        }
    }
}

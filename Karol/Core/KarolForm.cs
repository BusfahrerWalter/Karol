using Karol.Core;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Karol
{
    internal class KarolForm : Form
    {      
        public World World { get; set; }

        public PictureBox GridPicture;
        public PictureBox BlockMap;
        public ToolStripMenuItem RobotsMenuItem;
        
        private ContextMenuStrip contextMenuStrip;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem SaveImageButton;
        private ToolStripMenuItem SaveButton;
        private ToolStripMenuItem SaveScreenshotButton;

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
            BlockMap.Click += Map_Click;

            Size = new Size(image.Width + 40, image.Height + 60);

            World.onRobotAdded += World_onRobotAdded;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GridPicture = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveImageButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveScreenshotButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RobotsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.GridPicture)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
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
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveImageButton,
            this.SaveButton,
            this.SaveScreenshotButton,
            this.RobotsMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(160, 92);
            // 
            // SaveImageButton
            // 
            this.SaveImageButton.Name = "SaveImageButton";
            this.SaveImageButton.Size = new System.Drawing.Size(159, 22);
            this.SaveImageButton.Text = "Save as .png";
            this.SaveImageButton.Click += new System.EventHandler(this.SaveImageButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(159, 22);
            this.SaveButton.Text = "Save as .cskw";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveScreenshotButton
            // 
            this.SaveScreenshotButton.Name = "SaveScreenshotButton";
            this.SaveScreenshotButton.Size = new System.Drawing.Size(159, 22);
            this.SaveScreenshotButton.Text = "Save Screenshot";
            this.SaveScreenshotButton.Click += new System.EventHandler(this.SaveScreenshotButton_Click);
            // 
            // RobotsMenuItem
            // 
            this.RobotsMenuItem.Name = "RobotsMenuItem";
            this.RobotsMenuItem.Size = new System.Drawing.Size(159, 22);
            this.RobotsMenuItem.Text = "Take Control";
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
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private string DesktopPath(string fileName)
        {
            int i = 0;
            string ext = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);

            while (true)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{name} - {i}{ext}");
                if (!File.Exists(path))
                    return path;

                i++;
            }
        }

        #region Events
        private void World_onRobotAdded(object sender, WorldChangedEventArgs args)
        {
            var robo = args.NewElement as Robot;
            var item = RobotsMenuItem.DropDownItems.Add($"Robot {World.RoboterCount}", World.Robots[World.Robots.Count - 1].BitMap);

            item.Click += (s, args) =>
            {
                Controller.Create(robo);
            };
        }

        private void Map_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if(me.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(BlockMap, me.Location);
            }
        }

        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            string path = DesktopPath("KarolMap.png");
            try
            {
                World.SaveImage(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Datei {path} konnte nicht gespeichert werden.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(this, $"Datei wurde unter {path} gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string path = DesktopPath("KarolMap.cskw");
            try
            {
                World.Save(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Datei {path} konnte nicht gespeichert werden.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(this, $"Datei wurde unter {path} gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveScreenshotButton_Click(object sender, EventArgs e)
        {
            string path = DesktopPath("WorldScreenshot.png");
            try
            {
                World.SaveScreenshot(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Datei {path} konnte nicht gespeichert werden.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(this, $"Datei wurde unter {path} gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}

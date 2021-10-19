using Karol.Core;
using Karol.Core.Rendering;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karol
{
    internal class KarolForm : Form
    {
        internal class SaveDialogResult
        {
            public bool IsValid { get; set; }
            public string FileName { get; set; }

            public SaveDialogResult(bool isValid, string fileName)
            {
                IsValid = isValid;
                FileName = fileName;
            }

            public SaveDialogResult()
            {
            }
        }

        public World World { get; set; }
        private EditorForm WorldEditor { get; set; }

        public PictureBox GridPicture;
        public PictureBox BlockMap;
        public ToolStripMenuItem RobotsMenuItem;
        
        private ContextMenuStrip contextMenuStrip;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem SaveImageButton;
        private ToolStripMenuItem SaveButton;
        public ProgressBar ProgressBar;
        private ToolStripSeparator toolStripSeparator2;
        public ToolStripMenuItem View2DButton;
        private ToolStripSeparator toolStripSeparator1;
        public ToolStripMenuItem EditorButton;
        private ToolStripMenuItem SaveScreenshotButton;

        public KarolForm(World world, string title)
        {
            World = world;
            Text = title;        
        }

        public void SetUp(Image image, bool init = true)
        {
            if (init)
            {
                InitializeComponent();

                BlockMap = new PictureBox();
                GridPicture.Controls.Add(BlockMap);
                GridPicture.BackColor = Color.Transparent;

                BlockMap.Location = Point.Empty;
                BlockMap.BackColor = Color.Transparent;

                BlockMap.Click += Map_Click;
                World.onRenderingModeChanged += World_onRenderingModeChanged;
                Focus();
            }

            GridPicture.Size = image.Size;
            GridPicture.Image = image;
            BlockMap.Size = image.Size;
            BlockMap.Image = new Bitmap(image.Size.Width, image.Size.Height);
            Size = new Size(image.Width + 40, image.Height + 60);
            
            World.WorldRenderer.GridMap = GridPicture;
            World.WorldRenderer.BlockMap = BlockMap;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GridPicture = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveImageButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveScreenshotButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.View2DButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RobotsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
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
            this.toolStripSeparator2,
            this.View2DButton,
            this.EditorButton,
            this.toolStripSeparator1,
            this.RobotsMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(160, 148);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // View2DButton
            // 
            this.View2DButton.CheckOnClick = true;
            this.View2DButton.Name = "View2DButton";
            this.View2DButton.Size = new System.Drawing.Size(159, 22);
            this.View2DButton.Text = "2D View";
            this.View2DButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.View2DButton.Click += new System.EventHandler(this.View2DButton_Click);
            // 
            // EditorButton
            // 
            this.EditorButton.Enabled = false;
            this.EditorButton.Name = "EditorButton";
            this.EditorButton.Size = new System.Drawing.Size(159, 22);
            this.EditorButton.Text = "Open Editor";
            this.EditorButton.Click += new System.EventHandler(this.EditorButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // RobotsMenuItem
            // 
            this.RobotsMenuItem.Name = "RobotsMenuItem";
            this.RobotsMenuItem.Size = new System.Drawing.Size(159, 22);
            this.RobotsMenuItem.Text = "Take Control";
            // 
            // ProgressBar
            // 
            this.ProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.ProgressBar.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProgressBar.Location = new System.Drawing.Point(0, 0);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(133, 5);
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressBar.TabIndex = 1;
            this.ProgressBar.UseWaitCursor = true;
            this.ProgressBar.Value = 1;
            // 
            // KarolForm
            // 
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.ClientSize = new System.Drawing.Size(133, 103);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.GridPicture);
            this.HelpButton = true;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.Name = "KarolForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.GridPicture)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ShowSaveDialog(string filter, string defaultName, Action<SaveDialogResult> callback)
        {
            Thread t = new Thread(() =>
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Filter = filter,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FileName = defaultName
                };

                var erg = dialog.ShowDialog();
                try
                {
                    callback.Invoke(new SaveDialogResult()
                    {
                        IsValid = erg == DialogResult.OK || erg == DialogResult.Yes,
                        FileName = dialog.FileName
                    });
                }
                catch (Exception)
                {
                    MessageBox.Show("Beim speichern der Datei ist ein Fehler aufgetreten", "Oh NEIN!!!!");
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public void AddRobotToList(Robot robo)
        {
            Invoke((MethodInvoker)delegate
            {
                var item = RobotsMenuItem.DropDownItems.Add($"Robot {World.RoboterCount}", World.RobotCollection.Last.BitMap);
                item.Tag = robo;

                item.Click += (s, args) =>
                {
                    Controller.Create(robo);
                };
            });
        }

        public void RemoveRobotFromList(Robot robo)
        {
            Invoke((MethodInvoker)delegate
            {
                ToolStripItem item = null;
                foreach(ToolStripMenuItem i in RobotsMenuItem.DropDownItems)
                {
                    if(i.Tag == robo)
                    {
                        item = i;
                        break;
                    }
                }

                if (item == null)
                    return;

                RobotsMenuItem.DropDownItems.Remove(item);
            });
        }

        #region Events
        private void World_onRenderingModeChanged(object sender, WorldRenderingMode e)
        {
            Invoke((MethodInvoker)delegate
            {
                View2DButton.Checked = e == WorldRenderingMode.Render2D;
                EditorButton.Enabled = e == WorldRenderingMode.Render2D;
            });
        }

        private void Map_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if(me.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(BlockMap, me.Location);
            }
        }

        private void View2DButton_Click(object sender, EventArgs e)
        {
            World.RenderingMode = View2DButton.Checked ? WorldRenderingMode.Render2D : WorldRenderingMode.Render3D;
            if (!View2DButton.Checked && WorldEditor != null)
            {
                WorldEditor.Close();
            }
        }

        private void EditorButton_Click(object sender, EventArgs e)
        {
            if (WorldEditor != null && WorldEditor.Visible)
                return;

            WorldEditor = new EditorForm(World);
            WorldEditor.Show();
        }

        private void SaveImageButton_Click(object sender, EventArgs e)
        {
            ShowSaveDialog("Bilder (*.png)|*.png", "KarolImage.png", e =>
            {
                if (e.IsValid)
                {
                    World.SaveImage(e.FileName);
                    MessageBox.Show($"Datei wurde unter {e.FileName} gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });        
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ShowSaveDialog("C_Gartenzaun_Karol_World (*.cskw)|*.cskw", "KarolWorld.cskw", e =>
            {
                if (e.IsValid)
                {
                    World.Save(e.FileName);
                    MessageBox.Show($"Datei wurde unter {e.FileName} gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }

        private void SaveScreenshotButton_Click(object sender, EventArgs e)
        {
            ShowSaveDialog("Bilder (*.png)|*.png", "Screenshot.png", e =>
            {
                if (e.IsValid)
                {
                    World.SaveScreenshot(e.FileName);
                    MessageBox.Show($"Datei wurde unter {e.FileName} gespeichert!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }
        #endregion
    }
}


namespace Karol.Core
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.OkButton = new System.Windows.Forms.Button();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.ActionListBox = new System.Windows.Forms.ListBox();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkButton.Image = global::Karol.Properties.Resources.OkButton;
            this.OkButton.Location = new System.Drawing.Point(65, 106);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(44, 34);
            this.OkButton.TabIndex = 5;
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ImageList
            // 
            this.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "Marke.gif");
            this.ImageList.Images.SetKeyName(1, "Quader.gif");
            this.ImageList.Images.SetKeyName(2, "robot0.gif");
            this.ImageList.Images.SetKeyName(3, "ZiegelRed.gif");
            // 
            // ActionListBox
            // 
            this.ActionListBox.FormattingEnabled = true;
            this.ActionListBox.ItemHeight = 15;
            this.ActionListBox.Location = new System.Drawing.Point(13, 6);
            this.ActionListBox.Name = "ActionListBox";
            this.ActionListBox.Size = new System.Drawing.Size(96, 94);
            this.ActionListBox.TabIndex = 7;
            this.ActionListBox.SelectedIndexChanged += new System.EventHandler(this.ActionListBox_SelectedIndexChanged);
            // 
            // RemoveButton
            // 
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.Image = global::Karol.Properties.Resources.CancelButton;
            this.RemoveButton.Location = new System.Drawing.Point(13, 106);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(44, 34);
            this.RemoveButton.TabIndex = 9;
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // EditorForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(121, 153);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.ActionListBox);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditorForm";
            this.Text = "Editor";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditorForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.ImageList ImageList;
        public System.Windows.Forms.ListBox ActionListBox;
        public System.Windows.Forms.Button RemoveButton;
    }
}
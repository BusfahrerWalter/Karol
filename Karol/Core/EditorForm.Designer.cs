
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
            this.PlaceRobotButton = new System.Windows.Forms.RadioButton();
            this.PlaceBrickButton = new System.Windows.Forms.RadioButton();
            this.RemoveButton = new System.Windows.Forms.RadioButton();
            this.PlaceCubeButton = new System.Windows.Forms.RadioButton();
            this.PlaceMarkerButton = new System.Windows.Forms.RadioButton();
            this.OkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PlaceRobotButton
            // 
            this.PlaceRobotButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PlaceRobotButton.Image = global::Karol.Properties.Resources.robot0;
            this.PlaceRobotButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.PlaceRobotButton.Location = new System.Drawing.Point(78, 77);
            this.PlaceRobotButton.Name = "PlaceRobotButton";
            this.PlaceRobotButton.Size = new System.Drawing.Size(60, 60);
            this.PlaceRobotButton.TabIndex = 3;
            this.PlaceRobotButton.UseVisualStyleBackColor = true;
            // 
            // PlaceBrickButton
            // 
            this.PlaceBrickButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PlaceBrickButton.Image = global::Karol.Properties.Resources.ZiegelRed;
            this.PlaceBrickButton.Location = new System.Drawing.Point(12, 11);
            this.PlaceBrickButton.Name = "PlaceBrickButton";
            this.PlaceBrickButton.Size = new System.Drawing.Size(60, 60);
            this.PlaceBrickButton.TabIndex = 0;
            this.PlaceBrickButton.UseVisualStyleBackColor = true;
            // 
            // RemoveButton
            // 
            this.RemoveButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.Image = global::Karol.Properties.Resources.CancelButton;
            this.RemoveButton.Location = new System.Drawing.Point(12, 143);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(60, 60);
            this.RemoveButton.TabIndex = 4;
            this.RemoveButton.UseVisualStyleBackColor = true;
            // 
            // PlaceCubeButton
            // 
            this.PlaceCubeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PlaceCubeButton.Image = global::Karol.Properties.Resources.Quader;
            this.PlaceCubeButton.Location = new System.Drawing.Point(78, 11);
            this.PlaceCubeButton.Name = "PlaceCubeButton";
            this.PlaceCubeButton.Size = new System.Drawing.Size(60, 60);
            this.PlaceCubeButton.TabIndex = 1;
            this.PlaceCubeButton.UseVisualStyleBackColor = true;
            // 
            // PlaceMarkerButton
            // 
            this.PlaceMarkerButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PlaceMarkerButton.Image = global::Karol.Properties.Resources.Marke;
            this.PlaceMarkerButton.Location = new System.Drawing.Point(12, 77);
            this.PlaceMarkerButton.Name = "PlaceMarkerButton";
            this.PlaceMarkerButton.Size = new System.Drawing.Size(60, 60);
            this.PlaceMarkerButton.TabIndex = 2;
            this.PlaceMarkerButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkButton.Image = global::Karol.Properties.Resources.OkButton;
            this.OkButton.Location = new System.Drawing.Point(78, 143);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(60, 60);
            this.OkButton.TabIndex = 5;
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // EditorForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(151, 215);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.PlaceCubeButton);
            this.Controls.Add(this.PlaceRobotButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.PlaceBrickButton);
            this.Controls.Add(this.PlaceMarkerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditorForm";
            this.Text = "Editor";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RadioButton PlaceBrickButton;
        public System.Windows.Forms.RadioButton PlaceCubeButton;
        public System.Windows.Forms.RadioButton PlaceMarkerButton;
        public System.Windows.Forms.RadioButton PlaceRobotButton;
        public System.Windows.Forms.RadioButton RemoveButton;
        public System.Windows.Forms.Button OkButton;
    }
}
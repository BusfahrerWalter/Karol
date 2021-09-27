
namespace Karol.Core
{
    partial class ControllerForm
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
            this.ButtonRight = new System.Windows.Forms.Button();
            this.ButtonLeft = new System.Windows.Forms.Button();
            this.ButtonDown = new System.Windows.Forms.Button();
            this.ButtonUp = new System.Windows.Forms.Button();
            this.PlaceBrickButton = new System.Windows.Forms.Button();
            this.PlaceMarkButton = new System.Windows.Forms.Button();
            this.TurnLeftButton = new System.Windows.Forms.Button();
            this.TurnRightButton = new System.Windows.Forms.Button();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ButtonRight
            // 
            this.ButtonRight.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonRight.Location = new System.Drawing.Point(100, 55);
            this.ButtonRight.Name = "ButtonRight";
            this.ButtonRight.Size = new System.Drawing.Size(40, 40);
            this.ButtonRight.TabIndex = 0;
            this.ButtonRight.Text = "→";
            this.ButtonRight.UseVisualStyleBackColor = true;
            // 
            // ButtonLeft
            // 
            this.ButtonLeft.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonLeft.Location = new System.Drawing.Point(10, 55);
            this.ButtonLeft.Name = "ButtonLeft";
            this.ButtonLeft.Size = new System.Drawing.Size(40, 40);
            this.ButtonLeft.TabIndex = 1;
            this.ButtonLeft.Text = "←";
            this.ButtonLeft.UseVisualStyleBackColor = true;
            // 
            // ButtonDown
            // 
            this.ButtonDown.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonDown.Location = new System.Drawing.Point(55, 55);
            this.ButtonDown.Name = "ButtonDown";
            this.ButtonDown.Size = new System.Drawing.Size(40, 40);
            this.ButtonDown.TabIndex = 2;
            this.ButtonDown.Text = "↓";
            this.ButtonDown.UseVisualStyleBackColor = true;
            // 
            // ButtonUp
            // 
            this.ButtonUp.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonUp.Location = new System.Drawing.Point(55, 10);
            this.ButtonUp.Name = "ButtonUp";
            this.ButtonUp.Size = new System.Drawing.Size(40, 40);
            this.ButtonUp.TabIndex = 3;
            this.ButtonUp.Text = "↑";
            this.ButtonUp.UseVisualStyleBackColor = true;
            // 
            // PlaceBrickButton
            // 
            this.PlaceBrickButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlaceBrickButton.Image = global::Karol.Properties.Resources.ZiegelRed;
            this.PlaceBrickButton.Location = new System.Drawing.Point(9, 101);
            this.PlaceBrickButton.Name = "PlaceBrickButton";
            this.PlaceBrickButton.Size = new System.Drawing.Size(65, 40);
            this.PlaceBrickButton.TabIndex = 4;
            this.PlaceBrickButton.UseVisualStyleBackColor = true;
            // 
            // PlaceMarkButton
            // 
            this.PlaceMarkButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlaceMarkButton.Image = global::Karol.Properties.Resources.Marke;
            this.PlaceMarkButton.Location = new System.Drawing.Point(74, 101);
            this.PlaceMarkButton.Name = "PlaceMarkButton";
            this.PlaceMarkButton.Size = new System.Drawing.Size(65, 40);
            this.PlaceMarkButton.TabIndex = 5;
            this.PlaceMarkButton.UseVisualStyleBackColor = true;
            // 
            // TurnLeftButton
            // 
            this.TurnLeftButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TurnLeftButton.Location = new System.Drawing.Point(10, 10);
            this.TurnLeftButton.Name = "TurnLeftButton";
            this.TurnLeftButton.Size = new System.Drawing.Size(40, 40);
            this.TurnLeftButton.TabIndex = 6;
            this.TurnLeftButton.Text = "↶";
            this.TurnLeftButton.UseVisualStyleBackColor = true;
            // 
            // TurnRightButton
            // 
            this.TurnRightButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TurnRightButton.Location = new System.Drawing.Point(100, 10);
            this.TurnRightButton.Name = "TurnRightButton";
            this.TurnRightButton.Size = new System.Drawing.Size(40, 40);
            this.TurnRightButton.TabIndex = 7;
            this.TurnRightButton.Text = "↷";
            this.TurnRightButton.UseVisualStyleBackColor = true;
            // 
            // LogListBox
            // 
            this.LogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.ItemHeight = 15;
            this.LogListBox.Location = new System.Drawing.Point(9, 147);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(130, 124);
            this.LogListBox.TabIndex = 8;
            // 
            // ControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(151, 283);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.TurnRightButton);
            this.Controls.Add(this.TurnLeftButton);
            this.Controls.Add(this.PlaceMarkButton);
            this.Controls.Add(this.PlaceBrickButton);
            this.Controls.Add(this.ButtonUp);
            this.Controls.Add(this.ButtonDown);
            this.Controls.Add(this.ButtonLeft);
            this.Controls.Add(this.ButtonRight);
            this.Name = "ControllerForm";
            this.Text = "Karol Controller";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button ButtonRight;
        public System.Windows.Forms.Button ButtonLeft;
        public System.Windows.Forms.Button ButtonDown;
        public System.Windows.Forms.Button ButtonUp;
        public System.Windows.Forms.Button PlaceBrickButton;
        public System.Windows.Forms.Button PlaceMarkButton;
        public System.Windows.Forms.Button TurnLeftButton;
        public System.Windows.Forms.Button TurnRightButton;
        public System.Windows.Forms.ListBox LogListBox;
    }
}
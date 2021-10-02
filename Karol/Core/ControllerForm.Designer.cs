
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
            this.PickUpMarkButton = new System.Windows.Forms.Button();
            this.PickUpBrickButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BrickCountInput = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.MaxBackpackSizeInput = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectColorButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.JumpHeightInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.DelayInput = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.GetPaintButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrickCountInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxBackpackSizeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JumpHeightInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayInput)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonRight
            // 
            this.ButtonRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonRight.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonRight.Image = global::Karol.Properties.Resources.MoveRight;
            this.ButtonRight.Location = new System.Drawing.Point(100, 66);
            this.ButtonRight.Name = "ButtonRight";
            this.ButtonRight.Size = new System.Drawing.Size(40, 40);
            this.ButtonRight.TabIndex = 0;
            this.ButtonRight.UseVisualStyleBackColor = true;
            // 
            // ButtonLeft
            // 
            this.ButtonLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonLeft.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonLeft.Image = global::Karol.Properties.Resources.MoveLeft;
            this.ButtonLeft.Location = new System.Drawing.Point(10, 66);
            this.ButtonLeft.Name = "ButtonLeft";
            this.ButtonLeft.Size = new System.Drawing.Size(40, 40);
            this.ButtonLeft.TabIndex = 1;
            this.ButtonLeft.UseVisualStyleBackColor = true;
            // 
            // ButtonDown
            // 
            this.ButtonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonDown.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonDown.Image = global::Karol.Properties.Resources.MoveDown;
            this.ButtonDown.Location = new System.Drawing.Point(55, 66);
            this.ButtonDown.Name = "ButtonDown";
            this.ButtonDown.Size = new System.Drawing.Size(40, 40);
            this.ButtonDown.TabIndex = 2;
            this.ButtonDown.UseVisualStyleBackColor = true;
            // 
            // ButtonUp
            // 
            this.ButtonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonUp.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonUp.Image = global::Karol.Properties.Resources.MoveUp;
            this.ButtonUp.Location = new System.Drawing.Point(55, 21);
            this.ButtonUp.Name = "ButtonUp";
            this.ButtonUp.Size = new System.Drawing.Size(40, 40);
            this.ButtonUp.TabIndex = 3;
            this.ButtonUp.UseVisualStyleBackColor = true;
            // 
            // PlaceBrickButton
            // 
            this.PlaceBrickButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlaceBrickButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlaceBrickButton.Image = global::Karol.Properties.Resources.BrickDown;
            this.PlaceBrickButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PlaceBrickButton.Location = new System.Drawing.Point(10, 111);
            this.PlaceBrickButton.Name = "PlaceBrickButton";
            this.PlaceBrickButton.Size = new System.Drawing.Size(62, 40);
            this.PlaceBrickButton.TabIndex = 4;
            this.PlaceBrickButton.UseVisualStyleBackColor = true;
            // 
            // PlaceMarkButton
            // 
            this.PlaceMarkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlaceMarkButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlaceMarkButton.Image = global::Karol.Properties.Resources.MarkDown;
            this.PlaceMarkButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PlaceMarkButton.Location = new System.Drawing.Point(77, 111);
            this.PlaceMarkButton.Name = "PlaceMarkButton";
            this.PlaceMarkButton.Size = new System.Drawing.Size(63, 40);
            this.PlaceMarkButton.TabIndex = 5;
            this.PlaceMarkButton.UseVisualStyleBackColor = true;
            // 
            // TurnLeftButton
            // 
            this.TurnLeftButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TurnLeftButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TurnLeftButton.Image = global::Karol.Properties.Resources.RotateLeft;
            this.TurnLeftButton.Location = new System.Drawing.Point(10, 21);
            this.TurnLeftButton.Name = "TurnLeftButton";
            this.TurnLeftButton.Size = new System.Drawing.Size(40, 40);
            this.TurnLeftButton.TabIndex = 6;
            this.TurnLeftButton.UseVisualStyleBackColor = true;
            // 
            // TurnRightButton
            // 
            this.TurnRightButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TurnRightButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TurnRightButton.Image = global::Karol.Properties.Resources.RotateRight;
            this.TurnRightButton.Location = new System.Drawing.Point(100, 21);
            this.TurnRightButton.Name = "TurnRightButton";
            this.TurnRightButton.Size = new System.Drawing.Size(40, 40);
            this.TurnRightButton.TabIndex = 7;
            this.TurnRightButton.UseVisualStyleBackColor = true;
            // 
            // LogListBox
            // 
            this.LogListBox.AccessibleDescription = "";
            this.LogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.ItemHeight = 15;
            this.LogListBox.Location = new System.Drawing.Point(9, 230);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(348, 139);
            this.LogListBox.TabIndex = 8;
            // 
            // PickUpMarkButton
            // 
            this.PickUpMarkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PickUpMarkButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PickUpMarkButton.Image = global::Karol.Properties.Resources.MarkUp;
            this.PickUpMarkButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PickUpMarkButton.Location = new System.Drawing.Point(77, 157);
            this.PickUpMarkButton.Name = "PickUpMarkButton";
            this.PickUpMarkButton.Size = new System.Drawing.Size(63, 40);
            this.PickUpMarkButton.TabIndex = 10;
            this.PickUpMarkButton.UseVisualStyleBackColor = true;
            // 
            // PickUpBrickButton
            // 
            this.PickUpBrickButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PickUpBrickButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PickUpBrickButton.Image = global::Karol.Properties.Resources.BrickUp;
            this.PickUpBrickButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PickUpBrickButton.Location = new System.Drawing.Point(10, 157);
            this.PickUpBrickButton.Name = "PickUpBrickButton";
            this.PickUpBrickButton.Size = new System.Drawing.Size(62, 40);
            this.PickUpBrickButton.TabIndex = 9;
            this.PickUpBrickButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ButtonRight);
            this.groupBox1.Controls.Add(this.PickUpMarkButton);
            this.groupBox1.Controls.Add(this.ButtonLeft);
            this.groupBox1.Controls.Add(this.PickUpBrickButton);
            this.groupBox1.Controls.Add(this.ButtonDown);
            this.groupBox1.Controls.Add(this.ButtonUp);
            this.groupBox1.Controls.Add(this.TurnRightButton);
            this.groupBox1.Controls.Add(this.PlaceBrickButton);
            this.groupBox1.Controls.Add(this.TurnLeftButton);
            this.groupBox1.Controls.Add(this.PlaceMarkButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 212);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.GetPaintButton);
            this.groupBox2.Controls.Add(this.BrickCountInput);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.MaxBackpackSizeInput);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.SelectColorButton);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.JumpHeightInput);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.DelayInput);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(165, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 212);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // BrickCountInput
            // 
            this.BrickCountInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BrickCountInput.Location = new System.Drawing.Point(96, 177);
            this.BrickCountInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.BrickCountInput.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.BrickCountInput.Name = "BrickCountInput";
            this.BrickCountInput.Size = new System.Drawing.Size(84, 23);
            this.BrickCountInput.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Brick Count";
            // 
            // MaxBackpackSizeInput
            // 
            this.MaxBackpackSizeInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MaxBackpackSizeInput.Location = new System.Drawing.Point(96, 148);
            this.MaxBackpackSizeInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.MaxBackpackSizeInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.MaxBackpackSizeInput.Name = "MaxBackpackSizeInput";
            this.MaxBackpackSizeInput.Size = new System.Drawing.Size(84, 23);
            this.MaxBackpackSizeInput.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Max Back. Size";
            // 
            // SelectColorButton
            // 
            this.SelectColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectColorButton.Location = new System.Drawing.Point(96, 80);
            this.SelectColorButton.Name = "SelectColorButton";
            this.SelectColorButton.Size = new System.Drawing.Size(84, 23);
            this.SelectColorButton.TabIndex = 6;
            this.SelectColorButton.Text = "Select";
            this.SelectColorButton.UseVisualStyleBackColor = true;
            this.SelectColorButton.Click += new System.EventHandler(this.SelectColorButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Paint";
            // 
            // JumpHeightInput
            // 
            this.JumpHeightInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.JumpHeightInput.Location = new System.Drawing.Point(96, 51);
            this.JumpHeightInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.JumpHeightInput.Name = "JumpHeightInput";
            this.JumpHeightInput.Size = new System.Drawing.Size(84, 23);
            this.JumpHeightInput.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Jump Height";
            // 
            // DelayInput
            // 
            this.DelayInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DelayInput.Location = new System.Drawing.Point(96, 22);
            this.DelayInput.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.DelayInput.Name = "DelayInput";
            this.DelayInput.Size = new System.Drawing.Size(84, 23);
            this.DelayInput.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Delay";
            // 
            // GetPaintButton
            // 
            this.GetPaintButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetPaintButton.Location = new System.Drawing.Point(96, 109);
            this.GetPaintButton.Name = "GetPaintButton";
            this.GetPaintButton.Size = new System.Drawing.Size(84, 23);
            this.GetPaintButton.TabIndex = 11;
            this.GetPaintButton.Text = "Get Paint";
            this.GetPaintButton.UseVisualStyleBackColor = true;
            // 
            // ControllerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 381);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LogListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControllerForm";
            this.Text = "Karol Controller";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrickCountInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxBackpackSizeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JumpHeightInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayInput)).EndInit();
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
        public System.Windows.Forms.Button PickUpMarkButton;
        public System.Windows.Forms.Button PickUpBrickButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown JumpHeightInput;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown DelayInput;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ColorDialog ColorDialog;
        public System.Windows.Forms.NumericUpDown BrickCountInput;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown MaxBackpackSizeInput;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button GetPaintButton;
        public System.Windows.Forms.Button SelectColorButton;
    }
}
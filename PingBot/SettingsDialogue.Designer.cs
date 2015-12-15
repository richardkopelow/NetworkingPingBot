namespace PingBot
{
    partial class SettingsDialogue
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
            this.label1 = new System.Windows.Forms.Label();
            this.minBufferBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.maxBufferBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pingTargetsBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Low Load Buffer Size:";
            // 
            // minBufferBox
            // 
            this.minBufferBox.Location = new System.Drawing.Point(12, 30);
            this.minBufferBox.Name = "minBufferBox";
            this.minBufferBox.Size = new System.Drawing.Size(112, 20);
            this.minBufferBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "High Load Buffer Size:";
            // 
            // maxBufferBox
            // 
            this.maxBufferBox.Location = new System.Drawing.Point(12, 70);
            this.maxBufferBox.Name = "maxBufferBox";
            this.maxBufferBox.Size = new System.Drawing.Size(112, 20);
            this.maxBufferBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Target Nodes (one per line):";
            // 
            // pingTargetsBox
            // 
            this.pingTargetsBox.Location = new System.Drawing.Point(12, 132);
            this.pingTargetsBox.Multiline = true;
            this.pingTargetsBox.Name = "pingTargetsBox";
            this.pingTargetsBox.Size = new System.Drawing.Size(260, 137);
            this.pingTargetsBox.TabIndex = 5;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(197, 275);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // SettingsDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 310);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.pingTargetsBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maxBufferBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.minBufferBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsDialogue";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsDialogue_FormClosed);
            this.Load += new System.EventHandler(this.SettingsDialogue_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox minBufferBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox maxBufferBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pingTargetsBox;
        private System.Windows.Forms.Button saveButton;
    }
}
namespace Axxes.AkkaNetDemo.TestClient
{
    partial class Form1
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
            this.btnStart = new System.Windows.Forms.Button();
            this.lstProgress = new System.Windows.Forms.ListBox();
            this.lblNoDevices = new System.Windows.Forms.Label();
            this.txtNoDevices = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoDevices)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(303, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lstProgress
            // 
            this.lstProgress.FormattingEnabled = true;
            this.lstProgress.Location = new System.Drawing.Point(12, 44);
            this.lstProgress.Name = "lstProgress";
            this.lstProgress.Size = new System.Drawing.Size(366, 342);
            this.lstProgress.TabIndex = 1;
            // 
            // lblNoDevices
            // 
            this.lblNoDevices.AutoSize = true;
            this.lblNoDevices.Location = new System.Drawing.Point(14, 17);
            this.lblNoDevices.Name = "lblNoDevices";
            this.lblNoDevices.Size = new System.Drawing.Size(149, 13);
            this.lblNoDevices.TabIndex = 2;
            this.lblNoDevices.Text = "Number of devices to simulate";
            // 
            // txtNoDevices
            // 
            this.txtNoDevices.Location = new System.Drawing.Point(169, 15);
            this.txtNoDevices.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.txtNoDevices.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtNoDevices.Name = "txtNoDevices";
            this.txtNoDevices.Size = new System.Drawing.Size(120, 20);
            this.txtNoDevices.TabIndex = 3;
            this.txtNoDevices.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 393);
            this.Controls.Add(this.txtNoDevices);
            this.Controls.Add(this.lblNoDevices);
            this.Controls.Add(this.lstProgress);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Axxes Meter Reading Load Generator";
            ((System.ComponentModel.ISupportInitialize)(this.txtNoDevices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lstProgress;
        private System.Windows.Forms.Label lblNoDevices;
        private System.Windows.Forms.NumericUpDown txtNoDevices;
    }
}


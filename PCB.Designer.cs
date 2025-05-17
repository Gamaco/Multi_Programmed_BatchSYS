namespace Projecto_2_19_2025
{
    partial class PCB
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
            this.lstProcessControlBlock = new System.Windows.Forms.ListBox();
            this.btnBackToOs = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstProcessControlBlock
            // 
            this.lstProcessControlBlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lstProcessControlBlock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstProcessControlBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lstProcessControlBlock.ForeColor = System.Drawing.Color.White;
            this.lstProcessControlBlock.FormattingEnabled = true;
            this.lstProcessControlBlock.ItemHeight = 20;
            this.lstProcessControlBlock.Location = new System.Drawing.Point(11, 11);
            this.lstProcessControlBlock.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstProcessControlBlock.Name = "lstProcessControlBlock";
            this.lstProcessControlBlock.Size = new System.Drawing.Size(614, 320);
            this.lstProcessControlBlock.TabIndex = 0;
            // 
            // btnBackToOs
            // 
            this.btnBackToOs.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnBackToOs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBackToOs.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnBackToOs.Location = new System.Drawing.Point(225, 10);
            this.btnBackToOs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBackToOs.Name = "btnBackToOs";
            this.btnBackToOs.Size = new System.Drawing.Size(148, 35);
            this.btnBackToOs.TabIndex = 10;
            this.btnBackToOs.Text = "Close";
            this.btnBackToOs.UseVisualStyleBackColor = false;
            this.btnBackToOs.Click += new System.EventHandler(this.btnBackToOs_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.panel1.Controls.Add(this.btnBackToOs);
            this.panel1.Location = new System.Drawing.Point(12, 336);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(613, 54);
            this.panel1.TabIndex = 11;
            // 
            // PCB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(634, 402);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstProcessControlBlock);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PCB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PCB";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PCB_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstProcessControlBlock;
        private System.Windows.Forms.Button btnBackToOs;
        private System.Windows.Forms.Panel panel1;
    }
}
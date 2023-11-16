namespace wComTick
{
    partial class frmMain
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
            this.tLOG = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tLOG
            // 
            this.tLOG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLOG.Location = new System.Drawing.Point(0, 0);
            this.tLOG.Name = "tLOG";
            this.tLOG.ReadOnly = true;
            this.tLOG.Size = new System.Drawing.Size(284, 262);
            this.tLOG.TabIndex = 0;
            this.tLOG.Text = "Wait for response...";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tLOG);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tLOG;
    }
}


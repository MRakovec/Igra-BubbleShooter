namespace BubbleShooter
{
    partial class OknoLestvica
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
            this.SuspendLayout();
            // 
            // OknoLestvica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "OknoLestvica";
            this.Text = "Lestvica";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OknoLestvica_FormClosing);
            this.Load += new System.EventHandler(this.OknoLestvica_Load);
            this.ResizeEnd += new System.EventHandler(this.OknoLestvica_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
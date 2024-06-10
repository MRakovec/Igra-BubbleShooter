namespace BubbleShooter
{
    partial class OknoVpis
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
            // OknoVpis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "OknoVpis";
            this.Text = "Vpis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OknoVpis_FormClosing);
            this.Load += new System.EventHandler(this.OknoVpis_Load);
            this.ResizeEnd += new System.EventHandler(this.OknoVpis_ResizeEnd);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
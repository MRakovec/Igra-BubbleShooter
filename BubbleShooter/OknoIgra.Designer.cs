namespace BubbleShooter
{
    partial class OknoIgra
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
            // OknoIgra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 561);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 400);
            this.Name = "OknoIgra";
            this.Text = "Igra Bubble Shooter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OknoIgra_FormClosing);
            this.Load += new System.EventHandler(this.IgraForm_Load);
            this.ResizeEnd += new System.EventHandler(this.IgraForm_ResizeEnd);
            this.Click += new System.EventHandler(this.IgraForm_Click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OknoIgra_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}


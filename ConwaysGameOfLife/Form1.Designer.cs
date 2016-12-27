namespace ConwaysGameOfLife
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
            this.GraphicsBox = new System.Windows.Forms.PictureBox();
            this.lbl_FPS = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).BeginInit();
            this.SuspendLayout();
            // 
            // GraphicsBox
            // 
            this.GraphicsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphicsBox.Location = new System.Drawing.Point(0, 0);
            this.GraphicsBox.Name = "GraphicsBox";
            this.GraphicsBox.Size = new System.Drawing.Size(480, 457);
            this.GraphicsBox.TabIndex = 0;
            this.GraphicsBox.TabStop = false;
            this.GraphicsBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GraphicsBox_MouseClick);
            this.GraphicsBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphicsBox_MouseMove);
            // 
            // lbl_FPS
            // 
            this.lbl_FPS.BackColor = System.Drawing.Color.Red;
            this.lbl_FPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_FPS.ForeColor = System.Drawing.Color.Black;
            this.lbl_FPS.Location = new System.Drawing.Point(418, 9);
            this.lbl_FPS.Name = "lbl_FPS";
            this.lbl_FPS.Size = new System.Drawing.Size(50, 20);
            this.lbl_FPS.TabIndex = 1;
            this.lbl_FPS.Text = "FPS";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 457);
            this.Controls.Add(this.lbl_FPS);
            this.Controls.Add(this.GraphicsBox);
            this.Font = new System.Drawing.Font("Candara", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Conway\'s Game of Life";
            ((System.ComponentModel.ISupportInitialize)(this.GraphicsBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox GraphicsBox;
        private System.Windows.Forms.Label lbl_FPS;
    }
}


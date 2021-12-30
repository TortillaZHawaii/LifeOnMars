namespace LifeOnMars
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._mainPictureBox = new System.Windows.Forms.PictureBox();
            this._renderTimeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _mainPictureBox
            // 
            this._mainPictureBox.Location = new System.Drawing.Point(12, 12);
            this._mainPictureBox.Name = "_mainPictureBox";
            this._mainPictureBox.Size = new System.Drawing.Size(865, 663);
            this._mainPictureBox.TabIndex = 0;
            this._mainPictureBox.TabStop = false;
            this._mainPictureBox.Click += new System.EventHandler(this._mainPictureBox_Click);
            // 
            // _renderTimeLabel
            // 
            this._renderTimeLabel.AutoSize = true;
            this._renderTimeLabel.Location = new System.Drawing.Point(12, 689);
            this._renderTimeLabel.Name = "_renderTimeLabel";
            this._renderTimeLabel.Size = new System.Drawing.Size(73, 15);
            this._renderTimeLabel.TabIndex = 1;
            this._renderTimeLabel.Text = "RenderTime:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 713);
            this.Controls.Add(this._renderTimeLabel);
            this.Controls.Add(this._mainPictureBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox _mainPictureBox;
        private Label _renderTimeLabel;
    }
}
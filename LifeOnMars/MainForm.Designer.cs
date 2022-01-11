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
            this._lightLabel = new System.Windows.Forms.Label();
            this._ballLabel = new System.Windows.Forms.Label();
            this._cameraLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _mainPictureBox
            // 
            this._mainPictureBox.Location = new System.Drawing.Point(12, 12);
            this._mainPictureBox.Name = "_mainPictureBox";
            this._mainPictureBox.Size = new System.Drawing.Size(679, 679);
            this._mainPictureBox.TabIndex = 0;
            this._mainPictureBox.TabStop = false;
            this._mainPictureBox.Click += new System.EventHandler(this._mainPictureBox_Click);
            // 
            // _renderTimeLabel
            // 
            this._renderTimeLabel.AutoSize = true;
            this._renderTimeLabel.Location = new System.Drawing.Point(12, 694);
            this._renderTimeLabel.Name = "_renderTimeLabel";
            this._renderTimeLabel.Size = new System.Drawing.Size(73, 15);
            this._renderTimeLabel.TabIndex = 1;
            this._renderTimeLabel.Text = "RenderTime:";
            // 
            // _lightLabel
            // 
            this._lightLabel.AutoSize = true;
            this._lightLabel.Location = new System.Drawing.Point(697, 12);
            this._lightLabel.Name = "_lightLabel";
            this._lightLabel.Size = new System.Drawing.Size(38, 15);
            this._lightLabel.TabIndex = 2;
            this._lightLabel.Text = "label1";
            // 
            // _ballLabel
            // 
            this._ballLabel.AutoSize = true;
            this._ballLabel.Location = new System.Drawing.Point(697, 36);
            this._ballLabel.Name = "_ballLabel";
            this._ballLabel.Size = new System.Drawing.Size(38, 15);
            this._ballLabel.TabIndex = 3;
            this._ballLabel.Text = "label2";
            // 
            // _cameraLabel
            // 
            this._cameraLabel.AutoSize = true;
            this._cameraLabel.Location = new System.Drawing.Point(697, 62);
            this._cameraLabel.Name = "_cameraLabel";
            this._cameraLabel.Size = new System.Drawing.Size(38, 15);
            this._cameraLabel.TabIndex = 4;
            this._cameraLabel.Text = "label3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 718);
            this.Controls.Add(this._cameraLabel);
            this.Controls.Add(this._ballLabel);
            this.Controls.Add(this._lightLabel);
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
        private Label _lightLabel;
        private Label _ballLabel;
        private Label _cameraLabel;
    }
}
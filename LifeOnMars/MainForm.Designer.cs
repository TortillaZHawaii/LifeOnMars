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
            this._shaderGroupBox = new System.Windows.Forms.GroupBox();
            this._flatShaderRadio = new System.Windows.Forms.RadioButton();
            this._gouroudShaderRadio = new System.Windows.Forms.RadioButton();
            this._phongShaderRadio = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).BeginInit();
            this._shaderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainPictureBox
            // 
            this._mainPictureBox.Location = new System.Drawing.Point(12, 12);
            this._mainPictureBox.Name = "_mainPictureBox";
            this._mainPictureBox.Size = new System.Drawing.Size(766, 679);
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
            // _shaderGroupBox
            // 
            this._shaderGroupBox.Controls.Add(this._phongShaderRadio);
            this._shaderGroupBox.Controls.Add(this._gouroudShaderRadio);
            this._shaderGroupBox.Controls.Add(this._flatShaderRadio);
            this._shaderGroupBox.Location = new System.Drawing.Point(784, 12);
            this._shaderGroupBox.Name = "_shaderGroupBox";
            this._shaderGroupBox.Size = new System.Drawing.Size(201, 97);
            this._shaderGroupBox.TabIndex = 2;
            this._shaderGroupBox.TabStop = false;
            this._shaderGroupBox.Text = "Shader";
            // 
            // _flatShaderRadio
            // 
            this._flatShaderRadio.AutoSize = true;
            this._flatShaderRadio.Location = new System.Drawing.Point(6, 22);
            this._flatShaderRadio.Name = "_flatShaderRadio";
            this._flatShaderRadio.Size = new System.Drawing.Size(44, 19);
            this._flatShaderRadio.TabIndex = 0;
            this._flatShaderRadio.TabStop = true;
            this._flatShaderRadio.Text = "Flat";
            this._flatShaderRadio.UseVisualStyleBackColor = true;
            // 
            // _gouroudShaderRadio
            // 
            this._gouroudShaderRadio.AutoSize = true;
            this._gouroudShaderRadio.Location = new System.Drawing.Point(6, 47);
            this._gouroudShaderRadio.Name = "_gouroudShaderRadio";
            this._gouroudShaderRadio.Size = new System.Drawing.Size(72, 19);
            this._gouroudShaderRadio.TabIndex = 1;
            this._gouroudShaderRadio.TabStop = true;
            this._gouroudShaderRadio.Text = "Gouroud";
            this._gouroudShaderRadio.UseVisualStyleBackColor = true;
            // 
            // _phongShaderRadio
            // 
            this._phongShaderRadio.AutoSize = true;
            this._phongShaderRadio.Location = new System.Drawing.Point(6, 72);
            this._phongShaderRadio.Name = "_phongShaderRadio";
            this._phongShaderRadio.Size = new System.Drawing.Size(60, 19);
            this._phongShaderRadio.TabIndex = 2;
            this._phongShaderRadio.TabStop = true;
            this._phongShaderRadio.Text = "Phong";
            this._phongShaderRadio.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 718);
            this.Controls.Add(this._shaderGroupBox);
            this.Controls.Add(this._renderTimeLabel);
            this.Controls.Add(this._mainPictureBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).EndInit();
            this._shaderGroupBox.ResumeLayout(false);
            this._shaderGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox _mainPictureBox;
        private Label _renderTimeLabel;
        private GroupBox _shaderGroupBox;
        private RadioButton _phongShaderRadio;
        private RadioButton _gouroudShaderRadio;
        private RadioButton _flatShaderRadio;
    }
}
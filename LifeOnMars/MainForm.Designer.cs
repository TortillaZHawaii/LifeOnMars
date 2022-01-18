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
            this._phongShaderRadio = new System.Windows.Forms.RadioButton();
            this._gouroudShaderRadio = new System.Windows.Forms.RadioButton();
            this._flatShaderRadio = new System.Windows.Forms.RadioButton();
            this._cameraGroupBox = new System.Windows.Forms.GroupBox();
            this._onObjectCameraRadio = new System.Windows.Forms.RadioButton();
            this._followingCameraRadio = new System.Windows.Forms.RadioButton();
            this._fixedCameraRadio = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).BeginInit();
            this._shaderGroupBox.SuspendLayout();
            this._cameraGroupBox.SuspendLayout();
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
            this._phongShaderRadio.CheckedChanged += new System.EventHandler(this._phongShaderRadio_CheckedChanged);
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
            this._gouroudShaderRadio.CheckedChanged += new System.EventHandler(this._gouroudShaderRadio_CheckedChanged);
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
            this._flatShaderRadio.CheckedChanged += new System.EventHandler(this._flatShaderRadio_CheckedChanged);
            // 
            // _cameraGroupBox
            // 
            this._cameraGroupBox.Controls.Add(this._onObjectCameraRadio);
            this._cameraGroupBox.Controls.Add(this._followingCameraRadio);
            this._cameraGroupBox.Controls.Add(this._fixedCameraRadio);
            this._cameraGroupBox.Location = new System.Drawing.Point(784, 115);
            this._cameraGroupBox.Name = "_cameraGroupBox";
            this._cameraGroupBox.Size = new System.Drawing.Size(201, 97);
            this._cameraGroupBox.TabIndex = 3;
            this._cameraGroupBox.TabStop = false;
            this._cameraGroupBox.Text = "Camera";
            // 
            // _onObjectCameraRadio
            // 
            this._onObjectCameraRadio.AutoSize = true;
            this._onObjectCameraRadio.Location = new System.Drawing.Point(6, 72);
            this._onObjectCameraRadio.Name = "_onObjectCameraRadio";
            this._onObjectCameraRadio.Size = new System.Drawing.Size(77, 19);
            this._onObjectCameraRadio.TabIndex = 2;
            this._onObjectCameraRadio.Text = "On object";
            this._onObjectCameraRadio.UseVisualStyleBackColor = true;
            this._onObjectCameraRadio.CheckedChanged += new System.EventHandler(this._onObjectCameraRadio_CheckedChanged);
            // 
            // _followingCameraRadio
            // 
            this._followingCameraRadio.AutoSize = true;
            this._followingCameraRadio.Location = new System.Drawing.Point(6, 47);
            this._followingCameraRadio.Name = "_followingCameraRadio";
            this._followingCameraRadio.Size = new System.Drawing.Size(77, 19);
            this._followingCameraRadio.TabIndex = 1;
            this._followingCameraRadio.Text = "Following";
            this._followingCameraRadio.UseVisualStyleBackColor = true;
            this._followingCameraRadio.CheckedChanged += new System.EventHandler(this._followingCameraRadio_CheckedChanged);
            // 
            // _fixedCameraRadio
            // 
            this._fixedCameraRadio.AutoSize = true;
            this._fixedCameraRadio.Checked = true;
            this._fixedCameraRadio.Location = new System.Drawing.Point(6, 22);
            this._fixedCameraRadio.Name = "_fixedCameraRadio";
            this._fixedCameraRadio.Size = new System.Drawing.Size(53, 19);
            this._fixedCameraRadio.TabIndex = 0;
            this._fixedCameraRadio.TabStop = true;
            this._fixedCameraRadio.Text = "Fixed";
            this._fixedCameraRadio.UseVisualStyleBackColor = true;
            this._fixedCameraRadio.CheckedChanged += new System.EventHandler(this._fixedCameraRadio_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 718);
            this.Controls.Add(this._cameraGroupBox);
            this.Controls.Add(this._shaderGroupBox);
            this.Controls.Add(this._renderTimeLabel);
            this.Controls.Add(this._mainPictureBox);
            this.Name = "MainForm";
            this.Text = "LifeOnMars";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this._mainPictureBox)).EndInit();
            this._shaderGroupBox.ResumeLayout(false);
            this._shaderGroupBox.PerformLayout();
            this._cameraGroupBox.ResumeLayout(false);
            this._cameraGroupBox.PerformLayout();
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
        private GroupBox _cameraGroupBox;
        private RadioButton _onObjectCameraRadio;
        private RadioButton _followingCameraRadio;
        private RadioButton _fixedCameraRadio;
    }
}
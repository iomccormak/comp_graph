namespace lab3
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.pic = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.palettePic = new System.Windows.Forms.PictureBox();
            this.colorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePic)).BeginInit();
            this.SuspendLayout();
            // 
            // pic
            // 
            this.pic.Location = new System.Drawing.Point(425, 65);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(793, 474);
            this.pic.TabIndex = 0;
            this.pic.TabStop = false;
            this.pic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pic_MouseClick);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(33, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(358, 93);
            this.button2.TabIndex = 2;
            this.button2.Text = "Стереть все";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Erase_Click);
            // 
            // palettePic
            // 
            this.palettePic.Image = ((System.Drawing.Image)(resources.GetObject("palettePic.Image")));
            this.palettePic.Location = new System.Drawing.Point(33, 180);
            this.palettePic.Name = "palettePic";
            this.palettePic.Size = new System.Drawing.Size(358, 359);
            this.palettePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.palettePic.TabIndex = 3;
            this.palettePic.TabStop = false;
            this.palettePic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.palettePic_MouseClick);
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.BackColor = System.Drawing.Color.White;
            this.colorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.colorLabel.Location = new System.Drawing.Point(645, 582);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(307, 52);
            this.colorLabel.TabIndex = 4;
            this.colorLabel.Text = "Текущий цвет";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1249, 687);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.palettePic);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pic);
            this.Name = "Form4";
            this.Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.palettePic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox palettePic;
        private System.Windows.Forms.Label colorLabel;
    }
}
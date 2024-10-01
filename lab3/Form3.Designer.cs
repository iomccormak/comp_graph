namespace lab3
{
    partial class Form3
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
            this.radioButtonBresenham = new System.Windows.Forms.RadioButton();
            this.radioButtonWU = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // radioButtonBresenham
            // 
            this.radioButtonBresenham.AutoSize = true;
            this.radioButtonBresenham.Checked = true;
            this.radioButtonBresenham.Location = new System.Drawing.Point(13, 13);
            this.radioButtonBresenham.Name = "radioButtonBresenham";
            this.radioButtonBresenham.Size = new System.Drawing.Size(139, 17);
            this.radioButtonBresenham.TabIndex = 0;
            this.radioButtonBresenham.TabStop = true;
            this.radioButtonBresenham.Text = "Алгоритм Брезенхэма";
            this.radioButtonBresenham.UseVisualStyleBackColor = true;
            this.radioButtonBresenham.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButtonWU
            // 
            this.radioButtonWU.AutoSize = true;
            this.radioButtonWU.Location = new System.Drawing.Point(13, 37);
            this.radioButtonWU.Name = "radioButtonWU";
            this.radioButtonWU.Size = new System.Drawing.Size(92, 17);
            this.radioButtonWU.TabIndex = 1;
            this.radioButtonWU.Text = "Алгоритм ВУ";
            this.radioButtonWU.UseVisualStyleBackColor = true;
            this.radioButtonWU.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Form3
            // 
            this.ClientSize = new System.Drawing.Size(894, 450);
            this.Controls.Add(this.radioButtonWU);
            this.Controls.Add(this.radioButtonBresenham);
            this.Name = "Form3";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonBresenham;
        private System.Windows.Forms.RadioButton radioButtonWU;
    }
}
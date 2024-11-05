namespace lab7
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxRotateLine = new System.Windows.Forms.TextBox();
            this.labelRotateLine = new System.Windows.Forms.Label();
            this.textBoxRotateAxis = new System.Windows.Forms.TextBox();
            this.labelRotateAxis = new System.Windows.Forms.Label();
            this.reflectTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.translationTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxAthenian = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.comboBoxPolyhedron = new System.Windows.Forms.ComboBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(185, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(747, 577);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBoxRotateLine);
            this.groupBox1.Controls.Add(this.labelRotateLine);
            this.groupBox1.Controls.Add(this.textBoxRotateAxis);
            this.groupBox1.Controls.Add(this.labelRotateAxis);
            this.groupBox1.Controls.Add(this.reflectTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.translationTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.applyButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxAthenian);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxOutput);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxScale);
            this.groupBox1.Controls.Add(this.comboBoxPolyhedron);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 596);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 409);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 43);
            this.button1.TabIndex = 19;
            this.button1.Text = "Сбросить аффинные преобразования";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxRotateLine
            // 
            this.textBoxRotateLine.Location = new System.Drawing.Point(6, 371);
            this.textBoxRotateLine.Name = "textBoxRotateLine";
            this.textBoxRotateLine.Size = new System.Drawing.Size(149, 20);
            this.textBoxRotateLine.TabIndex = 18;
            this.textBoxRotateLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxRotateLine.TextChanged += new System.EventHandler(this.textBoxRotateLine_TextChanged);
            // 
            // labelRotateLine
            // 
            this.labelRotateLine.AutoSize = true;
            this.labelRotateLine.Location = new System.Drawing.Point(6, 355);
            this.labelRotateLine.Name = "labelRotateLine";
            this.labelRotateLine.Size = new System.Drawing.Size(128, 13);
            this.labelRotateLine.TabIndex = 17;
            this.labelRotateLine.Text = "Поворот вокруг прямой";
            // 
            // textBoxRotateAxis
            // 
            this.textBoxRotateAxis.Location = new System.Drawing.Point(6, 327);
            this.textBoxRotateAxis.Name = "textBoxRotateAxis";
            this.textBoxRotateAxis.Size = new System.Drawing.Size(149, 20);
            this.textBoxRotateAxis.TabIndex = 16;
            this.textBoxRotateAxis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxRotateAxis.TextChanged += new System.EventHandler(this.textBoxRotateAxis_TextChanged);
            // 
            // labelRotateAxis
            // 
            this.labelRotateAxis.AutoSize = true;
            this.labelRotateAxis.Location = new System.Drawing.Point(6, 310);
            this.labelRotateAxis.Name = "labelRotateAxis";
            this.labelRotateAxis.Size = new System.Drawing.Size(117, 13);
            this.labelRotateAxis.TabIndex = 15;
            this.labelRotateAxis.Text = "Вращение вокруг оси";
            // 
            // reflectTextBox
            // 
            this.reflectTextBox.Location = new System.Drawing.Point(6, 281);
            this.reflectTextBox.Name = "reflectTextBox";
            this.reflectTextBox.Size = new System.Drawing.Size(149, 20);
            this.reflectTextBox.TabIndex = 14;
            this.reflectTextBox.Text = "XY";
            this.reflectTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.reflectTextBox.TextChanged += new System.EventHandler(this.textBoxReflection_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 265);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Отражение (XY/XZ/YZ)";
            // 
            // translationTextBox
            // 
            this.translationTextBox.Location = new System.Drawing.Point(6, 242);
            this.translationTextBox.Name = "translationTextBox";
            this.translationTextBox.Size = new System.Drawing.Size(150, 20);
            this.translationTextBox.TabIndex = 12;
            this.translationTextBox.Text = "0 0 0";
            this.translationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.translationTextBox.TextChanged += new System.EventHandler(this.textBoxTranslation_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Смещение (x, y, z)";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(6, 155);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(150, 23);
            this.applyButton.TabIndex = 10;
            this.applyButton.Text = "Применить";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Многогранник";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Аффинные  преобразования";
            // 
            // comboBoxAthenian
            // 
            this.comboBoxAthenian.FormattingEnabled = true;
            this.comboBoxAthenian.Items.AddRange(new object[] {
            "Смещение",
            "Поворот",
            "Масштаб",
            "Отражение",
            "Вращение"});
            this.comboBoxAthenian.Location = new System.Drawing.Point(6, 127);
            this.comboBoxAthenian.Name = "comboBoxAthenian";
            this.comboBoxAthenian.Size = new System.Drawing.Size(150, 21);
            this.comboBoxAthenian.TabIndex = 7;
            this.comboBoxAthenian.SelectedIndexChanged += new System.EventHandler(this.comboBoxAthenian_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 464);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Вывод";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(7, 483);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(149, 94);
            this.textBoxOutput.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Масштабирование";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(6, 203);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(150, 20);
            this.textBoxScale.TabIndex = 3;
            this.textBoxScale.Text = "1";
            this.textBoxScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // comboBoxPolyhedron
            // 
            this.comboBoxPolyhedron.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBoxPolyhedron.FormattingEnabled = true;
            this.comboBoxPolyhedron.Items.AddRange(new object[] {
            "Тетраэдр",
            "Гексаэдр",
            "Октаэдр",
            "Икосаэдр",
            "Додекаэдр",
            "Параллелепипед"});
            this.comboBoxPolyhedron.Location = new System.Drawing.Point(6, 85);
            this.comboBoxPolyhedron.Name = "comboBoxPolyhedron";
            this.comboBoxPolyhedron.Size = new System.Drawing.Size(150, 21);
            this.comboBoxPolyhedron.TabIndex = 2;
            this.comboBoxPolyhedron.SelectedIndexChanged += new System.EventHandler(this.comboBoxPolyhedron_SelectedIndexChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 44);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(99, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Аксонометрия";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(92, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Перспектива";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.saveButton);
            this.groupBox2.Controls.Add(this.loadButton);
            this.groupBox2.Location = new System.Drawing.Point(938, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 596);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(7, 20);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(163, 23);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Загрузить модель";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(7, 50);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(163, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить модель";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AcceptButton = this.applyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 605);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxRotateLine;
        private System.Windows.Forms.Label labelRotateLine;
        private System.Windows.Forms.TextBox textBoxRotateAxis;
        private System.Windows.Forms.Label labelRotateAxis;
        private System.Windows.Forms.TextBox reflectTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox translationTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxAthenian;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.ComboBox comboBoxPolyhedron;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
    }
}


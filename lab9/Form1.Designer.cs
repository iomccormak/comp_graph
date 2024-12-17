namespace lab9
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxPolyhedronList = new System.Windows.Forms.ListBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.resetButton = new System.Windows.Forms.Button();
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
            this.checkBoxLighting = new System.Windows.Forms.CheckBox();
            this.checkBoxColor = new System.Windows.Forms.CheckBox();
            this.checkBoxZBuffer = new System.Windows.Forms.CheckBox();
            this.resetCameraButton = new System.Windows.Forms.Button();
            this.checkBoxNonFrontFaces = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clearRotationFigureButton = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.createRotationFigureButton = new System.Windows.Forms.Button();
            this.pictureBoxRotationFigure = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxRotationFigure = new System.Windows.Forms.TextBox();
            this.radioButtonZRotationFigure = new System.Windows.Forms.RadioButton();
            this.radioButtonYRotationFigure = new System.Windows.Forms.RadioButton();
            this.radioButtonXRotationFigure = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBoxTexture = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRotationFigure)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(185, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(865, 733);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.listBoxPolyhedronList);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.resetButton);
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
            this.groupBox1.Size = new System.Drawing.Size(167, 736);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // listBoxPolyhedronList
            // 
            this.listBoxPolyhedronList.FormattingEnabled = true;
            this.listBoxPolyhedronList.Location = new System.Drawing.Point(6, 136);
            this.listBoxPolyhedronList.Name = "listBoxPolyhedronList";
            this.listBoxPolyhedronList.Size = new System.Drawing.Size(150, 134);
            this.listBoxPolyhedronList.TabIndex = 24;
            this.listBoxPolyhedronList.SelectedIndexChanged += new System.EventHandler(this.listBoxPolyhedronList_SelectedIndexChanged);
            this.listBoxPolyhedronList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxPolyhedronList_KeyDown);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 17);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(99, 17);
            this.radioButton3.TabIndex = 21;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Параллельная";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Click += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox1.Location = new System.Drawing.Point(83, 497);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Auto Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(6, 561);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(150, 43);
            this.resetButton.TabIndex = 19;
            this.resetButton.Text = "Сбросить аффинные преобразования";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // textBoxRotateLine
            // 
            this.textBoxRotateLine.Location = new System.Drawing.Point(6, 535);
            this.textBoxRotateLine.Name = "textBoxRotateLine";
            this.textBoxRotateLine.Size = new System.Drawing.Size(150, 20);
            this.textBoxRotateLine.TabIndex = 18;
            this.textBoxRotateLine.Text = "0 0 0 1 1 1 3";
            this.textBoxRotateLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxRotateLine.TextChanged += new System.EventHandler(this.textBoxRotateLine_TextChanged);
            // 
            // labelRotateLine
            // 
            this.labelRotateLine.AutoSize = true;
            this.labelRotateLine.Location = new System.Drawing.Point(6, 519);
            this.labelRotateLine.Name = "labelRotateLine";
            this.labelRotateLine.Size = new System.Drawing.Size(128, 13);
            this.labelRotateLine.TabIndex = 17;
            this.labelRotateLine.Text = "Поворот вокруг прямой";
            // 
            // textBoxRotateAxis
            // 
            this.textBoxRotateAxis.Location = new System.Drawing.Point(6, 494);
            this.textBoxRotateAxis.Name = "textBoxRotateAxis";
            this.textBoxRotateAxis.Size = new System.Drawing.Size(71, 20);
            this.textBoxRotateAxis.TabIndex = 16;
            this.textBoxRotateAxis.Text = "y 3";
            this.textBoxRotateAxis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxRotateAxis.TextChanged += new System.EventHandler(this.textBoxRotateAxis_TextChanged);
            // 
            // labelRotateAxis
            // 
            this.labelRotateAxis.AutoSize = true;
            this.labelRotateAxis.Location = new System.Drawing.Point(6, 478);
            this.labelRotateAxis.Name = "labelRotateAxis";
            this.labelRotateAxis.Size = new System.Drawing.Size(117, 13);
            this.labelRotateAxis.TabIndex = 15;
            this.labelRotateAxis.Text = "Вращение вокруг оси";
            // 
            // reflectTextBox
            // 
            this.reflectTextBox.Location = new System.Drawing.Point(6, 455);
            this.reflectTextBox.Name = "reflectTextBox";
            this.reflectTextBox.Size = new System.Drawing.Size(150, 20);
            this.reflectTextBox.TabIndex = 14;
            this.reflectTextBox.Text = "XY";
            this.reflectTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.reflectTextBox.TextChanged += new System.EventHandler(this.textBoxReflection_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 439);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Отражение (XY/XZ/YZ)";
            // 
            // translationTextBox
            // 
            this.translationTextBox.Location = new System.Drawing.Point(6, 417);
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
            this.label5.Location = new System.Drawing.Point(6, 401);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Смещение (x, y, z)";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(6, 324);
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
            this.label4.Location = new System.Drawing.Point(3, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Многогранник";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Аффинные  преобразования";
            // 
            // comboBoxAthenian
            // 
            this.comboBoxAthenian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAthenian.FormattingEnabled = true;
            this.comboBoxAthenian.Items.AddRange(new object[] { "Смещение", "Поворот", "Масштаб", "Отражение", "Вращение" });
            this.comboBoxAthenian.Location = new System.Drawing.Point(6, 297);
            this.comboBoxAthenian.Name = "comboBoxAthenian";
            this.comboBoxAthenian.Size = new System.Drawing.Size(150, 21);
            this.comboBoxAthenian.TabIndex = 7;
            this.comboBoxAthenian.SelectedIndexChanged += new System.EventHandler(this.comboBoxAthenian_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 607);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Вывод";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(6, 623);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(150, 94);
            this.textBoxOutput.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 362);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Масштабирование";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(6, 378);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(150, 20);
            this.textBoxScale.TabIndex = 3;
            this.textBoxScale.Text = "1";
            this.textBoxScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // comboBoxPolyhedron
            // 
            this.comboBoxPolyhedron.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPolyhedron.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboBoxPolyhedron.FormattingEnabled = true;
            this.comboBoxPolyhedron.Items.AddRange(new object[] { "Тетраэдр", "Гексаэдр", "Октаэдр", "Икосаэдр", "Додекаэдр", "Параллелепипед" });
            this.comboBoxPolyhedron.Location = new System.Drawing.Point(6, 109);
            this.comboBoxPolyhedron.Name = "comboBoxPolyhedron";
            this.comboBoxPolyhedron.Size = new System.Drawing.Size(150, 21);
            this.comboBoxPolyhedron.TabIndex = 2;
            this.comboBoxPolyhedron.SelectedIndexChanged += new System.EventHandler(this.comboBoxPolyhedron_SelectedIndexChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton2.Location = new System.Drawing.Point(6, 63);
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
            this.radioButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton1.Location = new System.Drawing.Point(6, 40);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(92, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Перспектива";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.checkBoxTexture);
            this.groupBox2.Controls.Add(this.checkBoxLighting);
            this.groupBox2.Controls.Add(this.checkBoxColor);
            this.groupBox2.Controls.Add(this.checkBoxZBuffer);
            this.groupBox2.Controls.Add(this.resetCameraButton);
            this.groupBox2.Controls.Add(this.checkBoxNonFrontFaces);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.clearRotationFigureButton);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.createRotationFigureButton);
            this.groupBox2.Controls.Add(this.pictureBoxRotationFigure);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBoxRotationFigure);
            this.groupBox2.Controls.Add(this.radioButtonZRotationFigure);
            this.groupBox2.Controls.Add(this.radioButtonYRotationFigure);
            this.groupBox2.Controls.Add(this.radioButtonXRotationFigure);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.saveButton);
            this.groupBox2.Controls.Add(this.loadButton);
            this.groupBox2.Location = new System.Drawing.Point(1056, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 746);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // checkBoxLighting
            // 
            this.checkBoxLighting.Location = new System.Drawing.Point(6, 296);
            this.checkBoxLighting.Name = "checkBoxLighting";
            this.checkBoxLighting.Size = new System.Drawing.Size(167, 17);
            this.checkBoxLighting.TabIndex = 26;
            this.checkBoxLighting.Text = "Вращать источник света";
            this.checkBoxLighting.UseVisualStyleBackColor = true;
            this.checkBoxLighting.CheckedChanged += new System.EventHandler(this.checkBoxLighting_CheckedChanged);
            // 
            // checkBoxColor
            // 
            this.checkBoxColor.AutoSize = true;
            this.checkBoxColor.Location = new System.Drawing.Point(6, 250);
            this.checkBoxColor.Name = "checkBoxColor";
            this.checkBoxColor.Size = new System.Drawing.Size(88, 17);
            this.checkBoxColor.TabIndex = 25;
            this.checkBoxColor.Text = "Задать цвет";
            this.checkBoxColor.UseVisualStyleBackColor = true;
            this.checkBoxColor.CheckedChanged += new System.EventHandler(this.checkBoxColor_CheckedChanged);
            // 
            // checkBoxZBuffer
            // 
            this.checkBoxZBuffer.AutoSize = true;
            this.checkBoxZBuffer.Location = new System.Drawing.Point(6, 227);
            this.checkBoxZBuffer.Name = "checkBoxZBuffer";
            this.checkBoxZBuffer.Size = new System.Drawing.Size(67, 17);
            this.checkBoxZBuffer.TabIndex = 24;
            this.checkBoxZBuffer.Text = "Z-буфер";
            this.checkBoxZBuffer.UseVisualStyleBackColor = true;
            this.checkBoxZBuffer.CheckedChanged += new System.EventHandler(this.checkBoxZBuffer_CheckedChanged);
            // 
            // resetCameraButton
            // 
            this.resetCameraButton.Location = new System.Drawing.Point(12, 435);
            this.resetCameraButton.Name = "resetCameraButton";
            this.resetCameraButton.Size = new System.Drawing.Size(165, 23);
            this.resetCameraButton.TabIndex = 23;
            this.resetCameraButton.Text = "Сбросить положение камеры";
            this.resetCameraButton.UseVisualStyleBackColor = true;
            this.resetCameraButton.Click += new System.EventHandler(this.resetCameraButton_Click);
            // 
            // checkBoxNonFrontFaces
            // 
            this.checkBoxNonFrontFaces.AutoSize = true;
            this.checkBoxNonFrontFaces.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBoxNonFrontFaces.Location = new System.Drawing.Point(6, 204);
            this.checkBoxNonFrontFaces.Name = "checkBoxNonFrontFaces";
            this.checkBoxNonFrontFaces.Size = new System.Drawing.Size(178, 17);
            this.checkBoxNonFrontFaces.TabIndex = 22;
            this.checkBoxNonFrontFaces.Text = "Отсечение невидимых граней";
            this.checkBoxNonFrontFaces.UseVisualStyleBackColor = true;
            this.checkBoxNonFrontFaces.CheckedChanged += new System.EventHandler(this.checkBoxNonFrontFaces_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 132);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Выберите график";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Укажите x0 x1 y0 y1 шаг";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 109);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(165, 20);
            this.textBox1.TabIndex = 18;
            this.textBox1.Text = "-10 10 -10 10 0,25";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "sin(x)+cos(y)", "cos(x+y)", "5*cos(x^2+y^2+1)/(x^2+y^2+1)+0.1)" });
            this.comboBox1.Location = new System.Drawing.Point(12, 148);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(165, 21);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Создать поверхность";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clearRotationFigureButton
            // 
            this.clearRotationFigureButton.Location = new System.Drawing.Point(97, 548);
            this.clearRotationFigureButton.Name = "clearRotationFigureButton";
            this.clearRotationFigureButton.Size = new System.Drawing.Size(80, 23);
            this.clearRotationFigureButton.TabIndex = 10;
            this.clearRotationFigureButton.Text = "Очистить";
            this.clearRotationFigureButton.UseVisualStyleBackColor = true;
            this.clearRotationFigureButton.Click += new System.EventHandler(this.clearRotationFigureButton_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox2.Location = new System.Drawing.Point(12, 72);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(165, 17);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "учитывать преобразования";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // createRotationFigureButton
            // 
            this.createRotationFigureButton.Location = new System.Drawing.Point(12, 548);
            this.createRotationFigureButton.Name = "createRotationFigureButton";
            this.createRotationFigureButton.Size = new System.Drawing.Size(79, 23);
            this.createRotationFigureButton.TabIndex = 9;
            this.createRotationFigureButton.Text = "Создать фигуру";
            this.createRotationFigureButton.UseVisualStyleBackColor = true;
            this.createRotationFigureButton.Click += new System.EventHandler(this.createRotationFigureButton_Click);
            // 
            // pictureBoxRotationFigure
            // 
            this.pictureBoxRotationFigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxRotationFigure.Location = new System.Drawing.Point(12, 580);
            this.pictureBoxRotationFigure.Name = "pictureBoxRotationFigure";
            this.pictureBoxRotationFigure.Size = new System.Drawing.Size(165, 147);
            this.pictureBoxRotationFigure.TabIndex = 8;
            this.pictureBoxRotationFigure.TabStop = false;
            this.pictureBoxRotationFigure.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxRotationFigure_Paint);
            this.pictureBoxRotationFigure.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxRotationFigure_MouseDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 503);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Количество разбиений";
            // 
            // textBoxRotationFigure
            // 
            this.textBoxRotationFigure.Location = new System.Drawing.Point(12, 522);
            this.textBoxRotationFigure.Name = "textBoxRotationFigure";
            this.textBoxRotationFigure.Size = new System.Drawing.Size(165, 20);
            this.textBoxRotationFigure.TabIndex = 6;
            this.textBoxRotationFigure.Text = "12";
            this.textBoxRotationFigure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxRotationFigure.TextChanged += new System.EventHandler(this.rotationFigureTextBox_TextChanged);
            // 
            // radioButtonZRotationFigure
            // 
            this.radioButtonZRotationFigure.AutoSize = true;
            this.radioButtonZRotationFigure.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonZRotationFigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonZRotationFigure.Location = new System.Drawing.Point(155, 464);
            this.radioButtonZRotationFigure.Name = "radioButtonZRotationFigure";
            this.radioButtonZRotationFigure.Size = new System.Drawing.Size(18, 30);
            this.radioButtonZRotationFigure.TabIndex = 5;
            this.radioButtonZRotationFigure.Text = "Z";
            this.radioButtonZRotationFigure.UseVisualStyleBackColor = true;
            // 
            // radioButtonYRotationFigure
            // 
            this.radioButtonYRotationFigure.AutoSize = true;
            this.radioButtonYRotationFigure.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonYRotationFigure.Checked = true;
            this.radioButtonYRotationFigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonYRotationFigure.Location = new System.Drawing.Point(131, 464);
            this.radioButtonYRotationFigure.Name = "radioButtonYRotationFigure";
            this.radioButtonYRotationFigure.Size = new System.Drawing.Size(18, 30);
            this.radioButtonYRotationFigure.TabIndex = 4;
            this.radioButtonYRotationFigure.TabStop = true;
            this.radioButtonYRotationFigure.Text = "Y";
            this.radioButtonYRotationFigure.UseVisualStyleBackColor = true;
            // 
            // radioButtonXRotationFigure
            // 
            this.radioButtonXRotationFigure.AutoSize = true;
            this.radioButtonXRotationFigure.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonXRotationFigure.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonXRotationFigure.Location = new System.Drawing.Point(107, 464);
            this.radioButtonXRotationFigure.Name = "radioButtonXRotationFigure";
            this.radioButtonXRotationFigure.Size = new System.Drawing.Size(18, 30);
            this.radioButtonXRotationFigure.TabIndex = 3;
            this.radioButtonXRotationFigure.Text = "X";
            this.radioButtonXRotationFigure.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 481);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Ось вращения";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 43);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(165, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить модель";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(12, 17);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(165, 23);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Загрузить модель";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 25;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkBoxTexture
            // 
            this.checkBoxTexture.Location = new System.Drawing.Point(6, 273);
            this.checkBoxTexture.Name = "checkBoxTexture";
            this.checkBoxTexture.Size = new System.Drawing.Size(167, 17);
            this.checkBoxTexture.TabIndex = 27;
            this.checkBoxTexture.Text = "Текстурировать";
            this.checkBoxTexture.UseVisualStyleBackColor = true;
            this.checkBoxTexture.CheckedChanged += new System.EventHandler(this.checkBoxTexture_CheckedChanged);
            // 
            // Form1
            // 
            this.AcceptButton = this.applyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 757);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRotationFigure)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox checkBoxTexture;

        private System.Windows.Forms.CheckBox checkBoxLighting;

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button resetButton;
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
        private System.Windows.Forms.RadioButton radioButtonXRotationFigure;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButtonZRotationFigure;
        private System.Windows.Forms.RadioButton radioButtonYRotationFigure;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxRotationFigure;
        private System.Windows.Forms.Button createRotationFigureButton;
        private System.Windows.Forms.PictureBox pictureBoxRotationFigure;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button clearRotationFigureButton;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxNonFrontFaces;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button resetCameraButton;
        private System.Windows.Forms.ListBox listBoxPolyhedronList;
        private System.Windows.Forms.CheckBox checkBoxZBuffer;
        private System.Windows.Forms.CheckBox checkBoxColor;
    }
}


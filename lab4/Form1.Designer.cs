namespace lab4
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
            this.deleteButton = new System.Windows.Forms.Button();
            this.newPolygonButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(12, 12);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(102, 23);
            this.deleteButton.TabIndex = 0;
            this.deleteButton.Text = "Очистить";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // newPolygonButton
            // 
            this.newPolygonButton.Location = new System.Drawing.Point(13, 42);
            this.newPolygonButton.Name = "newPolygonButton";
            this.newPolygonButton.Size = new System.Drawing.Size(101, 23);
            this.newPolygonButton.TabIndex = 1;
            this.newPolygonButton.Text = "Новый полигон";
            this.newPolygonButton.UseVisualStyleBackColor = true;
            this.newPolygonButton.Click += new System.EventHandler(this.NewPolygonButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 543);
            this.Controls.Add(this.newPolygonButton);
            this.Controls.Add(this.deleteButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button newPolygonButton;
    }
}


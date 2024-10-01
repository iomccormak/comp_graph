using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        internal Image _image;
        public Form1()
        {
            InitializeComponent();
            _image = pictureBox1.Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string resourcesPath = Path.Combine(projectDirectory, "Resources");

            if (!Directory.Exists(resourcesPath))
            {
                MessageBox.Show("Папка Resources не найдена: " + resourcesPath);
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = resourcesPath;

                openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var imageStream = openFileDialog.OpenFile())
                    {
                        _image = Image.FromStream(imageStream);
                        pictureBox1.Image = _image;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2(this);
            form2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3(this);
            form3.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4(this);
            form4.ShowDialog();
        }

    }
}
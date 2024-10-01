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

namespace lab3
{
    public partial class Form2 : Form
    {
        private bool _isDrawing = false;
        private Point _lastPoint;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Color _fillColor = Color.Red;
        private Color _targetColor;
        private Bitmap _imageBitmap;

        public Form2()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.White);

            pictureBox1.Image = _bitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _isDrawing = true;
                    _lastPoint = e.Location;
                }
            } 
            else if (radioButton2.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _targetColor = _bitmap.GetPixel(e.X, e.Y);
                    if (_targetColor.ToArgb() != _fillColor.ToArgb())
                    {
                        fillLines(e.X, e.Y);
                        pictureBox1.Invalidate();
                    }
                }
            } 
            else if (radioButton3.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {   
                    if (_imageBitmap != null)
                    {
                        _targetColor = _bitmap.GetPixel(e.X, e.Y);
                        _fillColor = _imageBitmap.GetPixel(_imageBitmap.Width / 2, _imageBitmap.Height / 2);

                        if (_targetColor.ToArgb() != _fillColor.ToArgb())
                        {
                            fillPictureLines(e.X, e.Y);
                            pictureBox1.Invalidate();
                        }
                    }                    
                }
            }
            else if (radioButton4.Checked)
            {

            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDrawing = false;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing && e.Button == MouseButtons.Left)
            {
                _graphics.DrawLine(Pens.Black, _lastPoint, e.Location);
                _lastPoint = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void fillLines(int x, int y)
        {
            int maxWidth = _bitmap.Width;
            int maxHeight = _bitmap.Height;

            if (x < 0 || y < 0 || x >= maxWidth || y >= maxHeight)
                return;

            if (_bitmap.GetPixel(x, y).ToArgb() != _targetColor.ToArgb())
                return;

            int leftX = x;
            int rightX = x;

            while (leftX > 0 && _bitmap.GetPixel(leftX - 1, y).ToArgb() == _targetColor.ToArgb())
            {
                leftX--;
            }

            while (rightX < maxWidth - 1 && _bitmap.GetPixel(rightX + 1, y).ToArgb() == _targetColor.ToArgb())
            {
                rightX++;
            }

            for (int i = leftX; i <= rightX; i++)
            {
                _bitmap.SetPixel(i, y, _fillColor);
            }

            for (int i = leftX; i <= rightX; i++)
            {
                if (y > 0 && _bitmap.GetPixel(i, y - 1).ToArgb() == _targetColor.ToArgb())
                {
                    fillLines(i, y - 1);
                }

                if (y < maxHeight - 1 && _bitmap.GetPixel(i, y + 1).ToArgb() == _targetColor.ToArgb())
                {
                    fillLines(i, y + 1);
                }
            }
        }

        private void fillPictureLines(int x, int y)
        {
            int maxWidth = _bitmap.Width;
            int maxHeight = _bitmap.Height;

            if (x < 0 || y < 0 || x >= maxWidth || y >= maxHeight)
                return;

            if (_bitmap.GetPixel(x, y).ToArgb() != _targetColor.ToArgb())
                return;

            int leftX = x;
            int rightX = x;

            while (leftX > 0 && _bitmap.GetPixel(leftX - 1, y).ToArgb() == _targetColor.ToArgb())
            {
                leftX--;
            }

            while (rightX < maxWidth - 1 && _bitmap.GetPixel(rightX + 1, y).ToArgb() == _targetColor.ToArgb())
            {
                rightX++;
            }

            for (int i = leftX; i <= rightX; i++)
            {
                _fillColor = GetColorFromImage(i, y);
                _bitmap.SetPixel(i, y, _fillColor);
            }

            for (int i = leftX; i <= rightX; i++)
            {
                if (y > 0 && _bitmap.GetPixel(i, y - 1).ToArgb() == _targetColor.ToArgb())
                {
                    fillPictureLines(i, y - 1);
                }

                if (y < maxHeight - 1 && _bitmap.GetPixel(i, y + 1).ToArgb() == _targetColor.ToArgb())
                {
                    fillPictureLines(i, y + 1);
                }
            }
        }
        private Color GetColorFromImage(int x, int y)
        {
            int xOffset = x - (_bitmap.Width / 2);
            int yOffset = y - (_bitmap.Height / 2);

            int imgX = _imageBitmap.Width / 2 + xOffset;
            int imgY = _imageBitmap.Height / 2 + yOffset;

            imgX %= _imageBitmap.Width;
            imgY %= _imageBitmap.Height;

            while (x < 0) x += _imageBitmap.Width;
            while (y < 0) y += _imageBitmap.Height;

            return _imageBitmap.GetPixel(imgX, imgY);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
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
                        _imageBitmap = new Bitmap(Image.FromStream(imageStream));
                    }
                }
            }
        }
    }
}

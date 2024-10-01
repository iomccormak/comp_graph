using FastBitmap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab2
{
    public partial class Form4 : Form
    {
        private Form1 _form1;
        private Image _image;
        private Bitmap _originalImage;
        private Bitmap _modifiedImage;

        public Form4(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
            _image = _form1._image;
            pictureBox1.Image = _image;

            _originalImage = new Bitmap(_image);
            _modifiedImage = new Bitmap(_originalImage);

            hueSlider.ValueChanged += (s, e) => ApplyHSVAdjustments();
            saturationSlider.ValueChanged += (s, e) => ApplyHSVAdjustments();
            valueSlider.ValueChanged += (s, e) => ApplyHSVAdjustments();

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveImage("output.png");
            MessageBox.Show("The image is saved as output.png", "Saving...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ApplyHSVAdjustments()
        {
            Bitmap bitmap = new Bitmap(_originalImage);

            using (var fastBitmap = new FastBitmap.FastBitmap(bitmap))
            {
                var hsvBitmap = fastBitmap.Select(color =>
                {
                    var (hue, saturation, value) = RGBtoHSV(color.R, color.G, color.B);
                    hue = (hue + hueSlider.Value - 180) % 360;
                    if (hue < 0) hue += 360;
                    saturation = Math.Min(1, Math.Max(saturation + ((saturationSlider.Value - 50) / 50d), 0));
                    value = Math.Min(1, Math.Max(value + ((valueSlider.Value - 50) / 50d), 0));
                    return HSVtoRGB(hue, saturation, value);
                });

                _modifiedImage = hsvBitmap;
                pictureBox1.Image = _modifiedImage;
            }
        }

        private (double hue, double saturation, double value) RGBtoHSV(int r, int g, int b)
        {
            double red = r / 255.0;
            double green = g / 255.0;
            double blue = b / 255.0;
            double max = Math.Max(Math.Max(red, green), blue);
            double min = Math.Min(Math.Min(red, green), blue);
            double delta = max - min;

            double saturation = (max == 0) ? 0 : delta / max;
            double value = max;
            double hue;
            if (delta == 0)
                hue = 0;
            else if (max == red)
                hue = (green - blue) / delta + (green < blue ? 6 : 0);
            else if (max == green)
                hue = (blue - red) / delta + 2;
            else
                hue = (red - green) / delta + 4;
            hue *= 60;

            return (hue, saturation, value);
        }

        private Color HSVtoRGB(double hue, double saturation, double value)
        {
            int hi = (int)Math.Floor(hue / 60) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);
            double p = value * (1 - saturation);
            double q = value * (1 - f * saturation);
            double t = value * (1 - (1 - f) * saturation);

            double rPrime, gPrime, bPrime;

            switch (hi)
            {
                case 0:
                    rPrime = value;
                    gPrime = t;
                    bPrime = p;
                    break;
                case 1:
                    rPrime = q;
                    gPrime = value;
                    bPrime = p;
                    break;
                case 2:
                    rPrime = p;
                    gPrime = value;
                    bPrime = t;
                    break;
                case 3:
                    rPrime = p;
                    gPrime = q;
                    bPrime = value;
                    break;
                case 4:
                    rPrime = t;
                    gPrime = p;
                    bPrime = value;
                    break;
                case 5:
                    rPrime = value;
                    gPrime = p;
                    bPrime = q;
                    break;
                default:
                    rPrime = gPrime = bPrime = 0;
                    break;
            }

            int r = (int)((rPrime * 255) + 0.5);
            int g = (int)((gPrime * 255) + 0.5);
            int b = (int)((bPrime * 255) + 0.5);
            return Color.FromArgb(r, g, b);
        }


        private void SaveImage(string filePath)
        {
            _modifiedImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FastBitmap;

namespace lab2
{
    public partial class Form2 : Form
    {
        private Form1 _form1;
        private Image _image;

        public Form2(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
            _image = _form1._image;
            pictureBox1.Image = _image;
            pictureBox2.Image = _image;
            pictureBox3.Image = _image;
            pictureBox4.Image = _image;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(_image);
            int[] intensity1 = new int[256];
            int[] intensity2 = new int[256];

            using (var fastBitmap = new FastBitmap.FastBitmap(bitmap))
            {
                var greyBitmap1 = fastBitmap.Select(color => {
                    var newColor = (int)(0.3 * color.R + 0.59 * color.G + 0.11 * color.B);
                    intensity1[newColor]++;
                    return Color.FromArgb(newColor, newColor, newColor);
                });
                pictureBox2.Image = greyBitmap1;
                BuildHistogram(intensity1, chart1);

                var greyBitmap2 = fastBitmap.Select(color => {
                    var newColor = (int)(0.21 * color.R + 0.72 * color.G + 0.07 * color.B);
                    intensity2[newColor]++;
                    return Color.FromArgb(newColor, newColor, newColor);
                });
                pictureBox3.Image = greyBitmap2;
                BuildHistogram(intensity2, chart2);

                var differenceBitmap = fastBitmap.Select(color => {
                    var newColor1 = (int)(0.3 * color.R + 0.59 * color.G + 0.11 * color.B);
                    var newColor2 = (int)(0.21 * color.R + 0.72 * color.G + 0.07 * color.B);
                    var newColor = Math.Abs(newColor1 - newColor2);
                    return Color.FromArgb(newColor, newColor, newColor);
                });
                pictureBox4.Image = differenceBitmap;
            }
        }

        private void BuildHistogram(int[] histogram, Chart chart)
        {
            chart.Series.Clear();
            Series series = new Series
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Gray
            };
            chart.Series.Add(series);

            for (int i = 0; i < histogram.Length; i++)
            {
                series.Points.AddXY(i, histogram[i]);
            }
        }
    }
}
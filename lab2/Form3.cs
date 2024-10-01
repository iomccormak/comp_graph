using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FastBitmap;

namespace lab2
{
    public partial class Form3 : Form
    {
        private Form1 _form1;
        private Image _image;

        public Form3(Form1 form1)
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

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form1.Show();
        }

        private Bitmap ExtractColorChannel(Bitmap bitmap, string channel)
        {
            Bitmap resultBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    int r = 0, g = 0, b = 0;

                    if (channel == "R")
                        r = color.R;
                    else if (channel == "G")
                        g = color.G;
                    else if (channel == "B")
                        b = color.B;

                    resultBitmap.SetPixel(i, j, Color.FromArgb(color.A, r, g, b));
                }
            }

            return resultBitmap;
        }

        private int[] BuildHistogram(Bitmap bitmap, Func<Color, int> channelSelector)
        {
            int[] histogram = new int[256];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    int intensity = channelSelector(bitmap.GetPixel(i, j));
                    histogram[intensity]++;
                }
            }
            return histogram;
        }

        private void DisplayHistogramInChart(int[] histogram, Chart chart, string title, Color color)
        {
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.Titles.Add(title);

            Series series = new Series
            {
                Name = "Intensity",
                Color = color,
                ChartType = SeriesChartType.Column
            };
            chart.Series.Add(series);

            for (int i = 0; i < histogram.Length; i++)
            {
                series.Points.AddXY(i, histogram[i]);
            }

            chart.ChartAreas[0].AxisX.Title = "Intensity";
            chart.ChartAreas[0].AxisY.Title = "Frequency";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            Bitmap redChannel = ExtractColorChannel(bitmap, "R");
            Bitmap greenChannel = ExtractColorChannel(bitmap, "G");
            Bitmap blueChannel = ExtractColorChannel(bitmap, "B");
            pictureBox2.Image = redChannel;
            pictureBox3.Image = greenChannel;
            pictureBox4.Image = blueChannel;
            int[] redHistogram = BuildHistogram(bitmap, c => c.R);
            int[] greenHistogram = BuildHistogram(bitmap, c => c.G);
            int[] blueHistogram = BuildHistogram(bitmap, c => c.B);
            DisplayHistogramInChart(redHistogram, chart1, "Red Channel Histogram", Color.Red);
            DisplayHistogramInChart(greenHistogram, chart2, "Green Channel Histogram", Color.Green);
            DisplayHistogramInChart(blueHistogram, chart3, "Blue Channel Histogram", Color.Blue);
        }
    }
}

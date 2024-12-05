using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace lab8
{
    public enum ModeView
    {
        Perspective,
        Axonometry,
        Parallel
    }

    public partial class Form1 : Form
    {
        Pen _pen;
        Bitmap _bitmap;
        Bitmap _bitmapRotationFigure;
        Graphics _graphics;
        Graphics _graphicsRotationFigure;
        Polyhedron _polyhedron;
        List<Polyhedron> _polyhedronList;
        Mode _mode;
        ModeView _modeView;
        ModeRotationFigure _modeRotationFigure;
        List<Point> _points;
        Func<float, float, float> _function;
        private bool saveWithAffin = false;
        Camera _camera;
        private bool colorPolyhedrons = false;

        enum Mode
        {
            None,
            Translation,
            Reflection,
            ScaleRelativeCenter,
            RotateAxis,
            RotateLine,
        }

        enum ModeRotationFigure
        {
            None,
            DrawPoints,
            DrawRotationFigure,
        }

        public Form1()
        {
            InitializeComponent();
            _polyhedron = new Hexahedron();
            _polyhedronList = new List<Polyhedron>();
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _bitmapRotationFigure = new Bitmap(pictureBoxRotationFigure.Width, pictureBoxRotationFigure.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
            //_graphics.ScaleTransform(1, -1);
            _graphicsRotationFigure = Graphics.FromImage(_bitmapRotationFigure);
            _graphicsRotationFigure.Clear(Color.White);
            _camera = new Camera(_graphics, _pen);
            pictureBox1.Image = _bitmap;
            pictureBoxRotationFigure.Image = _bitmapRotationFigure;
            applyButton.Enabled = false;
            checkBox1.Checked = false;
            comboBoxPolyhedron.SelectedIndex = 4;
            _mode = Mode.None;
            _modeView = ModeView.Parallel;
            _modeRotationFigure = ModeRotationFigure.DrawPoints;
            createRotationFigureButton.Enabled = false;
            _points = new List<Point>();
            this.MouseWheel += new MouseEventHandler(pictureBox1_OnMouseWheel);
            checkBoxNonFrontFaces.Checked = true;
            checkBoxColor.Checked = false;
            checkBoxZBuffer.Checked = false;
            DrawPolyhedron();
        }

        // public void DrawPolyhedron()
        // {
        //     _graphics.Clear(Color.White);

        //     if (radioButton1.Checked)
        //     {
        //         DrawPerspective();
        //     }
        //     else if (radioButton2.Checked)
        //     {
        //         DrawAxonometry();
        //     }
        //     else if (radioButton3.Checked)
        //     {
        //         DrawParallel();
        //     }
        // }

        public void DrawPolyhedron()
        {
            _camera.DrawScene(_graphics, _polyhedronList, _modeView, checkBoxNonFrontFaces.Checked, checkBoxZBuffer.Checked, pictureBox1.Width, pictureBox1.Height, colorPolyhedrons);
            pictureBox1.Refresh();
        }

        private new void Scale(float k)
        {
            float[][] MatrixScale = Matrices.Scale(k);

            _polyhedron.modelMatrix = Matrices.MultiplyMatrix(_polyhedron.modelMatrix, MatrixScale);

            DrawPolyhedron();
        }

        public void RotateLine(string input)
        {
            input = input.Trim();
            var parts = input.Split(' ');
            float angle = float.Parse(parts[6]);
            Point3D a = new Point3D(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            Point3D b = new Point3D(float.Parse(parts[3]), float.Parse(parts[4]), float.Parse(parts[5]));

            b.X -= a.X;
            b.Y -= a.Y;
            b.Z -= a.Z;

            float length = (float)Math.Sqrt(b.X * b.X + b.Y * b.Y + b.Z * b.Z);
            b.X /= length;
            b.Y /= length;
            b.Z /= length;

            float l = b.X;
            float m = b.Y;
            float n = b.Z;

            angle = (float)((angle / 180D) * Math.PI);

            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            float[][] rotationMatrix = Matrices.RotationLine(l, m, n, sin, cos);

            _polyhedron.modelMatrix = Matrices.MultiplyMatrix(_polyhedron.modelMatrix, rotationMatrix);

            DrawPolyhedron();
        }

        private void RotateAxis(string input)
        {
            var parts = input.Split(' ');
            string axis = parts[0];
            float angle = float.Parse(parts[1]);

            if (axis == "x")
            {
                XYZRotate(angle, CreateXRotationMatrix);
            }
            else if (axis == "y")
            {
                XYZRotate(angle, CreateYRotationMatrix);
            }
            else if (axis == "z")
            {
                XYZRotate(angle, CreateZRotationMatrix);
            }

            DrawPolyhedron();
        }

        private void XYZRotate(float angle, Func<float, float[][]> createRotationMatrix)
        {
            Point3D center = CalculateCenter(_polyhedron.points);
            Translation(-center.X, -center.Y, -center.Z, false);
            XYZRotatePoint(createRotationMatrix(angle));
            Translation(center.X, center.Y, center.Z, false);
        }

        private Point3D CalculateCenter(List<Point3D> points)
        {
            if (points == null || points.Count == 0)
            {
                throw new InvalidOperationException("Список точек пуст. Невозможно рассчитать центр.");
            }
            float x = points.Average(p => p.X);
            float y = points.Average(p => p.Y);
            float z = points.Average(p => p.Z);
            return new Point3D(x, y, z);
        }

        private void XYZRotatePoint(float[][] rotationMatrix)
        {
            _polyhedron.modelMatrix = Matrices.MultiplyMatrix(_polyhedron.modelMatrix, rotationMatrix);
        }

        public float[][] CreateXRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return Matrices.XRotationMatrix(rad);
        }

        public float[][] CreateYRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return Matrices.YRotationMatrix(rad);
        }

        public float[][] CreateZRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return Matrices.ZRotationMatrix(rad);
        }

        public void Translation(float dx, float dy, float dz, bool draw = true)
        {
            float[][] TranslationMatrix = Matrices.Translation(dx, dy, dz);

            _polyhedron.modelMatrix = Matrices.MultiplyMatrix(_polyhedron.modelMatrix, TranslationMatrix);

            if (draw)
                DrawPolyhedron();
        }

        private void Reflect(string plane)
        {
            float[][] reflectionMatrix;

            switch (plane)
            {
                case "XY":
                    reflectionMatrix = Matrices.XYreflectionMatrix();
                    break;
                case "XZ":
                    reflectionMatrix = Matrices.XZreflectionMatrix();
                    break;
                case "YZ":
                    reflectionMatrix = Matrices.YZreflectionMatrix();
                    break;
                default:
                    textBoxOutput.Text = "Неправильная плоскость для отражения.";
                    return;
            }

            _polyhedron.modelMatrix = Matrices.MultiplyMatrix(_polyhedron.modelMatrix, reflectionMatrix);

            DrawPolyhedron();
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _modeView = ModeView.Perspective;
            DrawPolyhedron();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _modeView = ModeView.Axonometry;
            DrawPolyhedron();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _modeView = ModeView.Parallel;
            DrawPolyhedron();
        }

        private void comboBoxPolyhedron_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxPolyhedron.SelectedIndex)
            {
                case 0:
                    _polyhedron = new Tetrahedron();
                    break;
                case 1:
                    _polyhedron = new Hexahedron();
                    break;
                case 2:
                    _polyhedron = new Octahedron();
                    break;
                case 3:
                    _polyhedron = new Icosahedron();
                    break;
                case 4:
                    _polyhedron = new Dodecahedron();
                    break;
                case 5:
                    _polyhedron = new Parallelepiped();
                    break;
            }
            Random random = new Random();
            foreach (var face in _polyhedron.faces)
            {
                face.faceColor = Color.FromArgb(
                    random.Next(256), // Красный компонент
                    random.Next(256), // Зелёный компонент
                    random.Next(256)  // Синий компонент
                );
            }
            listBoxPolyhedronList.Items.Add(_polyhedron);
            _polyhedronList.Add(_polyhedron);
            DrawPolyhedron();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            switch (_mode)
            {
                case Mode.None:
                    break;
                case Mode.ScaleRelativeCenter:
                    Scale(float.Parse(textBoxScale.Text, NumberStyles.Float, CultureInfo.InvariantCulture));
                    break;
                case Mode.Translation:
                    string[] parametrs = translationTextBox.Text.Split();
                    Translation(float.Parse(parametrs[0], NumberStyles.Float, CultureInfo.InvariantCulture),
                                float.Parse(parametrs[1], NumberStyles.Float, CultureInfo.InvariantCulture),
                                float.Parse(parametrs[2], NumberStyles.Float, CultureInfo.InvariantCulture));
                    break;
                case Mode.Reflection:
                    Reflect(reflectTextBox.Text.Trim().ToUpper());
                    break;
                case Mode.RotateAxis:
                    RotateAxis(textBoxRotateAxis.Text);
                    break;
                case Mode.RotateLine:
                    RotateLine(textBoxRotateLine.Text);
                    break;
            }
        }

        private void comboBoxAthenian_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxAthenian.SelectedIndex)
            {
                case 0:
                    _mode = Mode.Translation;
                    applyButton.Enabled = true;
                    break;
                case 1:
                    _mode = Mode.RotateLine;
                    applyButton.Enabled = true;
                    break;
                case 2:
                    _mode = Mode.ScaleRelativeCenter;
                    applyButton.Enabled = true;
                    break;
                case 3:
                    _mode = Mode.Reflection;
                    applyButton.Enabled = true;
                    break;
                case 4:
                    _mode = Mode.RotateAxis;
                    applyButton.Enabled = true;
                    break;
            }
        }
        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxScale.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float k) || k == 0)
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод коэффиента масштабирования.";
            }
            else
            {
                if (comboBoxAthenian.SelectedIndex != -1)
                {
                    applyButton.Enabled = true;
                }
                textBoxOutput.Text = string.Empty;
            }
        }

        private void textBoxTranslation_TextChanged(object sender, EventArgs e)
        {
            var parts = translationTextBox.Text.Split()
                                               .Select(part => part.Trim())
                                               .ToArray();

            if (parts.Length == 3 &&
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float tx) &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float ty) &&
                float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float tz))
            {
                if (comboBoxAthenian.SelectedIndex != -1)
                {
                    applyButton.Enabled = true;
                }
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Введите три значения смещения, разделенные пробелом.";
            }
        }

        private void textBoxReflection_TextChanged(object sender, EventArgs e)
        {
            var text = reflectTextBox.Text.Trim().ToUpper();

            if (text == "XY" || text == "XZ" || text == "YZ")
            {
                if (comboBoxAthenian.SelectedIndex != -1)
                {
                    applyButton.Enabled = true;
                }
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Введите значение координатной плоскости (XY/XZ/YZ/xy/xz/yz).";
            }
        }

        private void textBoxRotateAxis_TextChanged(object sender, EventArgs e)
        {
            var parts = textBoxRotateAxis.Text.Split().Select(part => part.Trim())
                                               .ToArray();

            if (parts.Length == 2 &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float angle))
            {
                applyButton.Enabled = comboBoxAthenian.SelectedIndex != -1;
                textBoxOutput.Text = string.Empty;
                checkBox1.Checked = true;
            }
            else
            {
                applyButton.Enabled = false;
                checkBox1.Checked = false;
                textBoxOutput.Text = "Некорректный ввод оси и угла. Формат: 'ось угол' (например, 'y 4').";
            }

        }

        private void textBoxRotateLine_TextChanged(object sender, EventArgs e)
        {
            var input = textBoxRotateLine.Text.Trim();
            var parts = input.Split(' ');

            if (parts.Length == 7 &&
                parts.All(part => float.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out _)))
            {
                applyButton.Enabled = comboBoxAthenian.SelectedIndex != -1;
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод. Формат: 'x1 y1 z1 x2 y2 z2 угол' (например, '10 10 10 40 40 40 45').";
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            _polyhedron.modelMatrix = Matrices.Identity();
            DrawPolyhedron();
            applyButton.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                pictureBox1.Width = this.ClientSize.Width - groupBox1.Width - groupBox2.Width - 30;
                pictureBox1.Height = this.ClientSize.Height - 25;
                _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                _graphics = Graphics.FromImage(_bitmap);
                _graphics.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);
                pictureBox1.Image = _bitmap;
                DrawPolyhedron();
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OBJ Files (*.obj)|*.obj";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _polyhedron = new Polyhedron();
                _polyhedron.ParseFromOBJ(openFileDialog.FileName);
                _polyhedronList.Add(_polyhedron);
                listBoxPolyhedronList.Items.Add(_polyhedron);
                DrawPolyhedron();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RotateAxis(textBoxRotateAxis.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void pictureBox1_OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (pictureBox1.ClientRectangle.Contains(pictureBox1.PointToClient(MousePosition)))
            {
                Scale(e.Delta > 0 ? 1.05f : 0.95f);
            }
        }

        private void rotationFigureTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxRotationFigure.Text, out int k) || k < 0)
            {
                createRotationFigureButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод количества разбиений.";
            }
            else
            {
                createRotationFigureButton.Enabled = _points.Count > 2;
                textBoxOutput.Text = string.Empty;
            }
        }

        private void createRotationFigureButton_Click(object sender, EventArgs e)
        {
            _modeRotationFigure = ModeRotationFigure.DrawRotationFigure;
            DrawRotationFigure();
            textBoxOutput.Text = "Нажмите на экран или на кнопку \"Очистить\", чтобы снова нарисовать точки.";
            pictureBoxRotationFigure.Invalidate();
        }

        private void pictureBoxRotationFigure_MouseDown(object sender, MouseEventArgs e)
        {
            switch (_modeRotationFigure)
            {
                case ModeRotationFigure.DrawPoints:
                    if (e.Button == MouseButtons.Left)
                    {
                        DrawPoint(e);
                    }
                    break;
                case ModeRotationFigure.DrawRotationFigure:
                    clearRotationFigureButton_Click(sender, e);
                    break;
                default:
                    break;
            }
        }
        private void DrawPoint(MouseEventArgs e)
        {
            _points.Add(e.Location);
            if (_points.Count > 2)
            {
                createRotationFigureButton.Enabled = true;
            }
            pictureBoxRotationFigure.Invalidate();
        }

        private void DrawRotationFigure()
        {
            List<Point3D> points = new List<Point3D>();

            foreach (Point point in _points)
            {
                Point3D newPoint = new Point3D(point);
                points.Add(newPoint);
            }
            points.Reverse();

            Pen pen = new Pen(Color.Black, 1);
            _graphicsRotationFigure.DrawPolygon(pen, _points.ToArray());

            int axis = radioButtonXRotationFigure.Checked ? 0 : radioButtonYRotationFigure.Checked ? 1 : 2;
            _polyhedron = new RotationPolyhedron(points, int.Parse(textBoxRotationFigure.Text, NumberStyles.Integer, CultureInfo.InvariantCulture), axis);

            listBoxPolyhedronList.Items.Add(_polyhedron);
            _polyhedronList.Add(_polyhedron);
            DrawPolyhedron();
        }

        private void pictureBoxRotationFigure_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            foreach (Point point in _points)
            {
                e.Graphics.DrawEllipse(pen, point.X, point.Y, 2, 2);
            }
        }

        private void clearRotationFigureButton_Click(object sender, EventArgs e)
        {
            _modeRotationFigure = ModeRotationFigure.DrawPoints;
            _graphicsRotationFigure.Clear(Color.White);
            _points.Clear();
            textBoxOutput.Text = "Нарисуйте больше 1 точки, чтобы найти выпуклую оболочку с помощью алгоритма Джарвиса.";
            createRotationFigureButton.Enabled = false;
            pictureBoxRotationFigure.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] inputs = textBox1.Text.Split(' ');
            float x0 = float.Parse(inputs[0]);
            float x1 = float.Parse(inputs[1]);
            float y0 = float.Parse(inputs[2]);
            float y1 = float.Parse(inputs[3]);
            float step = float.Parse(inputs[4]);
            _polyhedron = new FunctionalPolyhedron(x0, x1, y0, y1, step, _function);
            listBoxPolyhedronList.Items.Add(_polyhedron);
            _polyhedronList.Add(_polyhedron);
            DrawPolyhedron();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var input = textBox1.Text.Trim();
            var parts = input.Split(' ');

            if (parts.Length == 5 &&
                parts.All(part => float.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out _)))
            {
                button1.Enabled = comboBoxAthenian.SelectedIndex != -1;
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                button1.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод. Формат: 'x0 x1 y0 y1 шаг' (например, -10 10 -10 10 0,25).";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    _function = (x, y) => (float)Math.Sin(x) * (float)Math.Cos(y);
                    button1.Enabled = true;
                    break;
                case 1:
                    _function = (x, y) => (float)Math.Cos(x + y);
                    button1.Enabled = true;
                    break;
                case 2:
                    _function = (x, y) => (float)(5 * (Math.Cos(x * x + y * y + 1) / (x * x + y * y + 1) + 0.1));
                    button1.Enabled = true;
                    break;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                _polyhedron.SaveToFileInProjectFolder(saveWithAffin ? _polyhedron.modelMatrix : null);
                MessageBox.Show("Модель сохранена в папке проекта (Models/modifiedModel.obj)");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении модели: {ex.Message}");
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            saveWithAffin = !saveWithAffin;
        }

        private void comboBoxPolyList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxNonFrontFaces_CheckedChanged(object sender, EventArgs e)
        {
            DrawPolyhedron();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.S)
            {
                if (ActiveControl is System.Windows.Forms.TextBox)
                {
                    return;
                }
            }

            switch (e.KeyCode)
            {
                case Keys.W:
                    _camera.MoveUp();
                    break;
                case Keys.A:
                    _camera.MoveLeft();
                    break;
                case Keys.S:
                    _camera.MoveDown();
                    break;
                case Keys.D:
                    _camera.MoveRight();
                    break;
                case Keys.Q:
                    _camera.MoveBackward();
                    break;
                case Keys.E:
                    _camera.MoveForward();
                    break;
                case Keys.NumPad8:
                    _camera.RotateUp();
                    break;
                case Keys.NumPad2:
                    _camera.RotateDown();
                    break;
                case Keys.NumPad4:
                    _camera.RotateLeft();
                    break;
                case Keys.NumPad6:
                    _camera.RotateRight();
                    break;
                default:
                    break;
            }
            DrawPolyhedron();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _camera = new Camera(_graphics, _pen);
            DrawPolyhedron();
        }

        private void checkBoxColor_CheckedChanged(object sender, EventArgs e)
        {
            colorPolyhedrons = !colorPolyhedrons;
        }

        private void listBoxPolyhedronList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPolyhedronList.SelectedIndex >= 0)
            {
                _polyhedron = _polyhedronList[listBoxPolyhedronList.SelectedIndex];
            }
        }

        private void checkBoxZBuffer_CheckedChanged(object sender, EventArgs e)
        {
            DrawPolyhedron();
        }
    }
}

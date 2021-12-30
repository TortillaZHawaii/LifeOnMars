using Jednosc;
using Jednosc.Rendering;
using Jednosc.Scene;
using System.Diagnostics;
using System.Numerics;

namespace LifeOnMars
{
    public partial class MainForm : Form
    {
        private FlatShaderRenderer _renderer;
        private RenderScene _scene;
        private System.Windows.Forms.Timer _timer;
        private RenderObject _prop;

        public MainForm()
        {
            InitializeComponent();
            var bitmap = new DirectBitmap(_mainPictureBox.Width, _mainPictureBox.Height);
            _mainPictureBox.Image = bitmap.Bitmap;

            var camera = new Camera()
            {
                Position = 1 * Vector3.UnitX,
                Target = Vector3.Zero,
            };
            _scene = new RenderScene(camera);

            _renderer = new FlatShaderRenderer(bitmap, _scene);

            _timer = new System.Windows.Forms.Timer()
            {
                Interval = 1_000 / 60,
            };

            _timer.Tick += _timer_Tick;

            _timer.Start();

            _prop = new Jednosc.Scene.Examples.Cube()
            {
                Position = Vector3.Zero,
                Forward = Vector3.UnitZ
            };
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            DrawProp();
        }

        private void DrawCube()
        {
            var cube = new Jednosc.Scene.Examples.Cube();

            _renderer.Render(cube);

            _mainPictureBox.Invalidate();
        }

        private async void PickTeapot()
        {
            _prop = await RenderObject.FromFilenameAsync(@"D:\szkola\sem5\gk\teapot.obj");
        }

        private void DrawProp()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            _renderer.Render(_prop);
            stopwatch.Stop();
            _mainPictureBox.Invalidate();

            _renderTimeLabel.Text = $"RenderTime is {stopwatch.ElapsedMilliseconds} ms";
        }

        private void _mainPictureBox_Click(object sender, EventArgs e)
        {
            PickTeapot();
            //DrawProp();
        }

        private void PlayPauseAnimation()
        {
            if(_timer.Enabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Start();
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            var camPos = _scene.Camera.Position;
            var propPos = _prop.Position;
            float delta = 0.5f;

            switch(e.KeyChar)
            {
                // Camera movement
                case 'i':
                    _scene.Camera.Position = camPos with { Y = camPos.Y + delta, };
                    break;
                case 'j':
                    _scene.Camera.Position = camPos with { X = camPos.X - delta, };
                    break;
                case 'k':
                    _scene.Camera.Position = camPos with { Y = camPos.Y - delta, };
                    break;
                case 'l':
                    _scene.Camera.Position = camPos with { X = camPos.X + delta, };
                    break;
                case 'u':
                    _scene.Camera.Position = camPos with { Z = camPos.Z - delta, };
                    break;
                case 'o':
                    _scene.Camera.Position = camPos with { Z = camPos.Z + delta, };
                    break;

                // Object movement
                case 'w':
                    _prop.Position = propPos with { Y = propPos.Y + delta, };
                    break;
                case 'a':
                    _prop.Position = propPos with { X = propPos.X - delta, };
                    break;
                case 's':
                    _prop.Position = propPos with { Y = propPos.Y - delta, };
                    break;
                case 'd':
                    _prop.Position = propPos with { X = propPos.X + delta, };
                    break;
                case 'q':
                    _prop.Position = propPos with { Z = propPos.Z - delta, };
                    break;
                case 'e':
                    _prop.Position = propPos with { Z = propPos.Z + delta, };
                    break;

                case ' ':
                    PlayPauseAnimation();
                    break;
            }

            //_scene.Camera.Target = _prop.Position;
        }
    }
}
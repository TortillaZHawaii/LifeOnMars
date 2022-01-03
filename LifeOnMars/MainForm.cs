using Jednosc;
using Jednosc.Rendering;
using Jednosc.Scene;
using System.Diagnostics;
using System.Numerics;

namespace LifeOnMars
{
    public partial class MainForm : Form
    {
        private TextureRenderer _renderer;
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
                Position = -3 * Vector3.UnitX,
                Target = Vector3.Zero,
            };
            _scene = new RenderScene(camera);

            _renderer = new TextureRenderer(bitmap, _scene);

            _timer = new System.Windows.Forms.Timer()
            {
                Interval = 1_000 / 60,
            };

            _timer.Tick += _timer_Tick;

            _prop = RenderObject.FromFilename(@"C:\Users\dwyso\Downloads\african_head.obj");
            _prop.LoadTextureFromFilename(@"C:\Users\dwyso\Downloads\african_head_diffuse.png");
            _scene.Objects.Add(_prop);

            var light = new Light(Vector3.UnitX * -1f);

            _scene.Lights.Add(light);

            _timer.Start();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            DrawProp();
        }

        private async void PickTeapot()
        {
            _scene.Objects.Clear();
            _prop = await RenderObject.FromFilenameAsync(@"D:\szkola\sem5\gk\teapot.obj");
            _scene.Objects.Add(_prop);
        }

        private async void PickAfricanAsync()
        {
            _scene.Objects.Clear();
            _prop = await RenderObject.FromFilenameAsync(@"C:\Users\dwyso\Downloads\african_head.obj");
            _prop.LoadTextureFromFilename(@"C:\Users\dwyso\Downloads\african_head_diffuse.png");
            _scene.Objects.Add(_prop);
        }

        private void DrawProp()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            _renderer.Render();
            stopwatch.Stop();
            _mainPictureBox.Invalidate();

            _renderTimeLabel.Text = $"RenderTime is {stopwatch.ElapsedMilliseconds} ms";
        }

        private void _mainPictureBox_Click(object sender, EventArgs e)
        {
            //PickTeapot();
            //DrawProp();
            PickAfricanAsync();
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
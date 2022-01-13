using Jednosc;
using Jednosc.Bitmaps;
using Jednosc.Rendering;
using Jednosc.Rendering.Shaders.Factory;
using Jednosc.Scene;
using Jednosc.Scene.Lights;
using Jednosc.Scene.Props;
using System.Diagnostics;
using System.Numerics;

namespace LifeOnMars
{
    public partial class MainForm : Form
    {
        private IRenderer _renderer;
        private RenderScene _scene;
        private System.Windows.Forms.Timer _timer;
        private RenderObject _prop;
        private ShaderFactory _shaderFactory;

        public MainForm()
        {
            InitializeComponent();
            var bitmap = new DirectBitmap(_mainPictureBox.Width, _mainPictureBox.Height);
            _mainPictureBox.Image = bitmap.Bitmap;

            var camera = new Camera()
            {
                Position = 2 * Vector3.UnitZ,//new Vector3(-1.5f, 0.2f, 0.3f),
                Target = Vector3.Zero,
            };
            _scene = new RenderScene(camera);
            _shaderFactory = new ShaderFactory();
            _renderer = new RendererMultiThread(bitmap, _scene, _shaderFactory);

            _timer = new System.Windows.Forms.Timer()
            {
                Interval = 1_000 / 60,
            };

            _timer.Tick += _timer_Tick;

            // http://planetpixelemporium.com/sun.html
            //_prop = RenderObject.FromFilename(@"C:\Users\dwyso\Downloads\african_head.obj");
            //_prop.LoadTextureFromFilename(@"C:\Users\dwyso\Downloads\african_head_diffuse.png");
            //_prop.LoadNormalMapFromFilename(@"C:\Users\dwyso\Downloads\african_head_nm.png");
            //_scene.Objects.Add(_prop);

            //var sun = new SphereUV(20, 20, 1.0f);
            //sun.LoadNormalMapFromFilename(@"C:\Users\dwyso\Downloads\mars_1k_normal.jpg");
            //sun.LoadTextureFromFilename(@"C:\Users\dwyso\Downloads\sunmap.jpg");
            //_scene.Objects.Add(sun);
            //sun.Material = new Material()
            //{
            //    Ka = 1f,
            //    Kd = 0f,
            //    Ks = 0f,
            //    Alpha = 0,
            //};
            //sun.Position = lightPos;

            //var mars = new SphereUV(20, 20, 0.5f);
            //mars.Material = new Material()
            //{
            //    Kd = 0.2f,
            //    Ka = 0.0f,
            //    Ks = 1.99f,
            //    Alpha = 1000,
            //};
            //mars.LoadTextureFromFilename(@"C:\Users\dwyso\Downloads\marsmap1k.jpg");
            //mars.LoadNormalMapFromFilename(@"C:\Users\dwyso\Downloads\mars_1k_normal.jpg");
            //_scene.Objects.Add(mars);

            //_scene.Lights.Add(new Light(Vector3.One));
            //_scene.Lights.Add(new Light(-3 * Vector3.One, Vector3.UnitZ));
            var lightPos = 2 * Vector3.One;
            var blueBall = SphereUV.Create(20, 20, 1.0f, new Material(), new SingleColorBitmap(Color.Blue), new NormalColorBitmap());
            blueBall.Material = new Material() { Ka = 0f, Kd = 1f, Ks = 0f, Alpha = 10 };
            blueBall.LoadTextureFromFilename(@"D:\szkola\sem5\gk\blue.png");
            blueBall.LoadNormalMapFromFilename(@"D:\szkola\sem5\gk\blue.png");

            _prop = blueBall;
            _scene.Objects.Add(_prop);
            _scene.Lights.Add(new PointLight(lightPos));
            //_scene.Lights.Add(new Light(2 * Vector3.UnitZ, Vector3.One));
            //_scene.Lights.Add(new Light(-2 * Vector3.UnitX, Vector3.One));


            //_scene.Lights.Add(new Light(Vector3.UnitX, Vector3.UnitX));

            _scene.BackgroundColor = Color.Green;

            _timer.Start();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            DrawScene();
        }

        private void DrawScene()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            _renderer.RenderScene();
            stopwatch.Stop();
            _mainPictureBox.Invalidate();

            SetRenderTimeDisplay(stopwatch.ElapsedMilliseconds);
        }

        private void SetRenderTimeDisplay(long renderTimeInMs)
        {
            float fps = (float)1_000 / renderTimeInMs;

            _renderTimeLabel.Text = $"RenderTime: {renderTimeInMs} ms. FPS: {fps:F1}";
        }

        private void _mainPictureBox_Click(object sender, EventArgs e)
        {
            
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
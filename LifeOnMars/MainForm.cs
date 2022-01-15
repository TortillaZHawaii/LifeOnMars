using Jednosc;
using Jednosc.Bitmaps;
using Jednosc.Rendering;
using Jednosc.Rendering.Shaders.Factory;
using Jednosc.Scene;
using Jednosc.Scene.Lights;
using Jednosc.Scene.Props;
using LifeOnMars.SceneProps;
using System.Diagnostics;
using System.Numerics;

namespace LifeOnMars
{
    public partial class MainForm : Form
    {
        private IRenderer _renderer;
        private RenderScene _scene;
        private System.Windows.Forms.Timer _renderTimer;
        private System.Windows.Forms.Timer _animationTimer;
        private ShaderFactory _shaderFactory;

        private RenderObject _robot;
        private RenderObject _mars;
        private RenderObject _sun;

        private DirectionalLight _robotLight;
        private PointLight _sunLight;

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

            _renderTimer = new System.Windows.Forms.Timer()
            {
                Interval = 1_000 / 24,
            };
            _renderTimer.Tick += Render_Tick;

            _animationTimer = new System.Windows.Forms.Timer() 
            { 
                Interval = 1_000 / 30, 
            };
            _animationTimer.Tick += Animation_Tick;

            var lightPos = 2 * Vector3.One;
            _robot = PropsFactory.GetWheatley();

            _scene.Objects.Add(_robot);

            _scene.Lights.Add(new PointLight(lightPos));

            _scene.BackgroundColor = Color.Green;

            _renderTimer.Start();
            _animationTimer.Start();
        }

        private void Animation_Tick(object? sender, EventArgs e)
        {
            RotateProp();
        }

        private void Render_Tick(object? sender, EventArgs e)
        {
            DrawScene();
        }

        private static float alpha = 0f;

        private void RotateProp()
        {
            alpha += 0.1f;
            _robot.ModelMatrix =
                Matrix4x4.CreateScale(0.01f)
                * Matrix4x4.CreateRotationY(alpha)
                * Matrix4x4.CreateTranslation(0f, 0.5f * MathF.Sin(alpha), 0f);
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
            if(_renderTimer.Enabled)
            {
                _renderTimer.Stop();
                _animationTimer.Stop();
            }
            else
            {
                _renderTimer.Start();
                _animationTimer.Start();
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                PlayPauseAnimation();
        }

    }
}
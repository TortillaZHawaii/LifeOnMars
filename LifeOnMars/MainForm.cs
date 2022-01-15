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

        private SpotLight _robotLaser;
        private SpotLight _robotLightClose;

        private PointLight _sunLight;

        private Camera _camera;

        public MainForm()
        {
            InitializeComponent();
            var bitmap = new DirectBitmap(_mainPictureBox.Width, _mainPictureBox.Height);
            _mainPictureBox.Image = bitmap.Bitmap;

            _camera = new Camera()
            {
                Position = 7 * Vector3.One,
                Target = Vector3.Zero,
            };

            _scene = new RenderScene(_camera);
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

            _robot = PropsFactory.GetMars();
            _scene.Objects.Add(_robot);
            
            _mars = PropsFactory.GetMars();
            _mars.ModelMatrix = Matrix4x4.Identity;
            _scene.Objects.Add(_mars);

            _sun = PropsFactory.GetSun();
            _sun.ModelMatrix = Matrix4x4.Identity;
            _scene.Objects.Add(_sun);

            _sunLight = new PointLight(Vector3.Zero)
            {
                Al = 0.05f,
                Aq = 0f,
            };
            _robotLaser = new SpotLight(Vector3.Zero, Vector3.UnitX)
            {
                Color = Vector3.UnitZ,
                Power = 50,
            };
            _robotLightClose = new SpotLight(Vector3.Zero, Vector3.UnitX)
            {
                Color = Vector3.UnitX,
                Power = 2,
            };

            _scene.Lights.Add(_sunLight);
            _scene.Lights.Add(_robotLaser);

            _scene.BackgroundColor = Color.Green;

            _animationTimer.Start();
            _renderTimer.Start();
        }

        #region animations
        private float marsRotation = 0f;
        
        private float _robotRotation = 0f;
        private float _robotLightAngle = 0f;

        private void Animation_Tick(object? sender, EventArgs e)
        {
            marsRotation += 0.1f;
            _robotRotation += 0.05f;
            _robotLightAngle += 0.12f;
            const float lightAngleLimit = 0.5f;
            if(_robotLightAngle > lightAngleLimit)
            {
                _robotLightAngle = -lightAngleLimit;
            }

            AnimateSun();
            AnimateMars();
            AnimateRobot();

            AnimateCamera();
        }

        private void AnimateCamera()
        {
            // follow mars
            var marsPosition = _mars.ModelMatrix.Translation;
            _camera.Target = marsPosition;

            // follow robot
            float robotHeightAngle = 0.5f * _robotRotation;
            float robotOrbit = 2f + 0.5f * MathF.Sin(robotHeightAngle);
            float robotCameraOrbit = robotOrbit + 0.7f;
            _camera.Position =
                (Matrix4x4.CreateTranslation(robotCameraOrbit, 0.3f, 0f)
                * Matrix4x4.CreateRotationY(_robotRotation)
                * Matrix4x4.CreateTranslation(marsPosition))
                .Translation;
        }

        private void AnimateRobot()
        {
            var marsPosition = _mars.ModelMatrix.Translation;
            float robotHeightAngle = 0.5f * _robotRotation;
            float robotOrbit = 2f + 0.5f * MathF.Sin(robotHeightAngle);

            _robot.ModelMatrix =
                Matrix4x4.CreateScale(0.1f)
                * Matrix4x4.CreateTranslation(robotOrbit, 0, 0)
                * Matrix4x4.CreateRotationY(_robotRotation)
                * Matrix4x4.CreateTranslation(marsPosition);

            _robotLaser.Position = _robot.ModelMatrix.Translation;
            _robotLaser.Direction = (marsPosition + _robotLightAngle *  Vector3.UnitY) - _robotLaser.Position;
            
            _robotLightClose.Position = _robot.ModelMatrix.Translation;
            _robotLightClose.Direction = marsPosition - _robotLaser.Position;
            //_robotLightClose.Color = robotOrbit < 2f ? Vector3.UnitX : Vector3.Zero;
        }

        private void AnimateSun()
        {
            _sun.ModelMatrix = Matrix4x4.CreateTranslation(Vector3.Zero);
        }

        private void AnimateMars()
        {
            const float orbitRadius = 12f;

            _mars.ModelMatrix =
                Matrix4x4.Identity
                * Matrix4x4.CreateTranslation(0, 0, orbitRadius)
                * Matrix4x4.CreateRotationY(marsRotation);
        }


        #endregion

        private void Render_Tick(object? sender, EventArgs e)
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
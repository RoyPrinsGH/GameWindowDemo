using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinFormsApp1
{
    public partial class GameWindow : Form
    {
        public GameWindow()
        {
            InitializeComponent();
            SetupEnv();
            SetupObjects();
            SetupGraphics();

            Console.WriteLine("Game started.");
        }

        private void SetupGraphics()
        {
            DoubleBuffered = true;
        }

        private void SetupEnv()
        {
            if (Settings.Window.ConsoleEnabled)
            {
                AllocConsole();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private List<object> GameObjects { get; set; } = new List<object>();
        private void SetupObjects()
        {
            var player = new AnimatedSprite(ImageSources.WalkAnimation, 17, 25, 3, 20);
            player.Position = new Point(Width / 2, 0);

            GameObjects.Add(player);
        }

        private int TargetFramesPerSecond { get; set; } = 60;
        private TimeSpan TargetFrameTimeInMs => TimeSpan.FromMilliseconds(1000d / TargetFramesPerSecond);
        private long Ticks { get; set; }
        private readonly Stopwatch Stopwatch = new Stopwatch();
        private void OnPaint(object sender, PaintEventArgs e)
        {
            Stopwatch.Start();
            Ticks++;

            Graphics g = e.Graphics;

            g.Clear(Color.White);
                
            foreach (object obj in GameObjects)
            {
                if (obj is IDrawable drawableObj)
                {
                    drawableObj.Draw(g);
                }

                if (obj is IAnimatable animatableObj)
                {
                    animatableObj.AnimationTick(Ticks);
                }
            }

            while (Stopwatch.ElapsedMilliseconds < TargetFrameTimeInMs.Milliseconds)
            {
                // Wait
            }

            Stopwatch.Reset();
            Invalidate();
        }
    }
}
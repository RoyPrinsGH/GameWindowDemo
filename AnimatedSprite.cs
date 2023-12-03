using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public interface IDrawable
    {
        public void Draw(Graphics graphics);
    }

    public interface IAnimatable
    {
        public void AnimationTick(long gameTicks);
    }

    internal class AnimatedSprite : IDrawable, IAnimatable, IDisposable
    {
        private int AnimationTicks { get; set; }
        private int SpriteCount { get; init; }
        private int SpriteWidth { get; init; }
        private int SpriteHeight { get; init; }
        private Image? SpriteSheet { get; init; }

        public int FramesPerAnimationTick { get; set; }
        public Point Position { get; set; } = default;

        internal AnimatedSprite(string animationSpriteSheetPath, int width, int height, int spriteCount, int framesPerAnimationTick)
        {
            AnimationTicks = 0;

            if (spriteCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(spriteCount), $"{nameof(spriteCount)} must be positive");
            }
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), $"{nameof(width)} must be positive");
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), $"{nameof(height)} must be positive");
            }

            SpriteCount = spriteCount;
            SpriteWidth = width;
            SpriteHeight = height;
            FramesPerAnimationTick = framesPerAnimationTick;

            try
            {
                // This is IDisposable, so the whole class is now IDisposable
                SpriteSheet = Image.FromFile(animationSpriteSheetPath);
            }
            catch (FileNotFoundException e)
            {
                SpriteSheet = null;
                Console.WriteLine($"Sprite '{animationSpriteSheetPath}' not found: {e.Message}");
            }
        }

        public void Draw(Graphics g)
        {
            if (SpriteSheet is not null)
            {
                Rectangle gameScreenTargetRect = new Rectangle(Position.X, Position.Y, SpriteWidth, SpriteHeight);
                Rectangle spriteSheetImageRect = new Rectangle(SpriteWidth * AnimationTicks, 0, SpriteWidth, SpriteHeight);

                g.DrawImage(SpriteSheet, gameScreenTargetRect, spriteSheetImageRect, GraphicsUnit.Pixel);
            }
        }

        public void AnimationTick(long gameTicks)
        {
            if (gameTicks % FramesPerAnimationTick == 0)
            {
                AnimationTicks = (AnimationTicks +  1) % SpriteCount;
            }

            Position = new Point(Position.X, Position.Y + 1);
        }

        #region IDisposable Members and Helpers
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            SpriteSheet?.Dispose();
        }

        ~AnimatedSprite()
        {
            Dispose(false);
        }
        #endregion
    }
}

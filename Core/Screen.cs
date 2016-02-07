using Microsoft.Xna.Framework;

namespace MonoGame_2DPlatformer.Core
{
    class Screen
    {

        public static int width
        {
            get
            {
                return Game1.graphics.GraphicsDevice.Viewport.Width;
            }
        }

        public static int height
        {
            get
            {
                return Game1.graphics.GraphicsDevice.Viewport.Height;
            }
        }

        public static Rectangle bounds
        {
            get
            {
                return Game1.graphics.GraphicsDevice.Viewport.Bounds;
            }
        }

        public static bool fullScreen
        {
            set
            {
                Game1.graphics.IsFullScreen = value;
            }

        }

        public static void resolution(int width, int height)
        {
            Game1.graphics.PreferredBackBufferHeight = height;
            Game1.graphics.PreferredBackBufferWidth = width;
        }
       
            
    }
}

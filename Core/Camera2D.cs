using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGame_2DPlatformer.Core
{
    public class Camera2D
    {
        Matrix view;
        Matrix projection;
        float PosX = 16f;
        float PosY = 16f;
        Vector2 Pos;

        public Camera2D()
        {
            /// CURRENT FAKE CAMERA
            projection = Matrix.CreateOrthographicOffCenter(0, Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, 0, 0, 1);
            view = Matrix.Identity;
            view = Matrix.CreateTranslation(Pos.X, Pos.Y, 0); // correction the view after Farseer pixels to meter convertation

            Pos = new Vector2(PosX, PosY); //
        }

        public Matrix GetMatrix
        {
            get { return view; }
        }

        public float offsetRight
        {
            get { return Game1.graphics.GraphicsDevice.Viewport.Bounds.Right / 2; }
        }

        public float offsetLeft
        {
            get { return Game1.graphics.GraphicsDevice.Viewport.Bounds.Left; }
        }

        public Vector2 Position
        {
            get { return Pos; }
        }

        public void UpdatePos(float p)
        {
            Pos.X = -p + Screen.width/2 - 32; // <- Player moving speed
        }

        public void Update()
        {
            view = Matrix.CreateTranslation(Pos.X, Pos.Y, 0); // correction the view after Farseer pixels to meter convertation
        }
        
    }
}

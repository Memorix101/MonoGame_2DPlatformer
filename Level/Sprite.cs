using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace MonoGame_2DPlatformer
{
    class Sprite
    {
        protected Texture2D tile;
        Rectangle rect;
        Vector2 position;
        Color color = Color.White;
        Vector2 origin = Vector2.Zero;
        float layerDepth;
        float rotation;
        
        /// <summary>
        /// Props
        /// </summary>

        public virtual Rectangle BoundingRect
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    tile.Width,
                    tile.Height);
            }
        }
        
        public virtual Rectangle TileBoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    rect.Width,
                    rect.Height);
            }
        }
        
        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }
                
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int width
        {
            get { return tile.Width; }
        }

        public int height
        {
            get { return tile.Height; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value;  }
        }

 
        /// <summary>
        /// Stuff
        /// </summary>
        public void Load(string path)
        {
            tile = Game1.content.Load<Texture2D>(path);
        }

        public void Init(Vector2 p, Rectangle r, Color c)
        {
            rect = r;
            color = c;
            position = p;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tile, position, rect, color, rotation, origin, 1.0f, SpriteEffects.None, layerDepth);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{

    enum ItemTileType
    {
        Blank,
        Block,
    }

    class ItemTile : Sprite
    {
        Texture2D texture;
        float layer;
        ItemTileType type;
        Rectangle rect;
        BoundingBox boundingBox;
        bool destroy;
        bool walkable;

        public ItemTile(Vector2 p, float l, ItemTileType t)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites\\tileset");
            this.Position = p;
            layer = l;
            type = t;

            if (t == ItemTileType.Blank)
                rect = new Rectangle(0, 0, 0, 0);
            else if (t == ItemTileType.Block)
                rect = new Rectangle(12, 0, 32, 32);

            LoadBlock(p);
        }

        public ItemTileType Type
        {
            get { return type; }
        }

        public BoundingBox BoundingBox
        {
            get
            {
                return boundingBox;
            }
        }

        public override Rectangle TileBoundingBox
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

        public bool Destroy
        {
            set
            {
                destroy = value;
            }
        }

        public bool Walkable
        {
            get { return walkable; }
        }

        public void LoadBlock(Vector2 p)
        {
            switch(type)
            {
                case ItemTileType.Blank:
                    this.Rect = rect;
                    this.LayerDepth = layer;
                    walkable = false;
                    break;

                case ItemTileType.Block:
                    this.Rect = rect;
                    this.LayerDepth = layer;
                    walkable = true;
                    break;

            }
        }

        public void UpdateCollision(GameTime gameTime)
        {
           //boundingBox = new BoundingBox(new Vector3(this.Position.X, this.Position.Y, 0), new Vector3(Position.X - rect.Size.X, Position.Y - rect.Size.Y, 0));
         
            this.boundingBox.Min.X = this.Position.X;
            this.boundingBox.Min.Y = this.Position.Y;
            this.boundingBox.Max.X = this.Position.X + this.rect.Size.X;
            this.boundingBox.Max.Y = this.Position.Y + this.rect.Size.Y;
          
            
        }

        public void Draw(SpriteBatch batch)
        {
            if(!destroy)
            batch.Draw(texture, Position, rect, Color.White);
            //base.Draw(batch);
        }
    }
}

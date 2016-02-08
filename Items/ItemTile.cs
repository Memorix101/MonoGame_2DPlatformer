using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Factories;


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
        Body tileBody;

        public ItemTile(Vector2 p, float l, ItemTileType t)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites\\tileset");
            this.Position = p;
            layer = l;
            type = t;

            if (t == ItemTileType.Blank)
                rect = new Rectangle(0, 0, 1, 1);
            else if (t == ItemTileType.Block)
                rect = new Rectangle(12, 0, 32, 32);

            // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
            // 1 meters equals 64 pixels here

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
                tileBody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1.0f); //default 1:64 ratio 1 meter = 64 pixel
                tileBody.BodyType = BodyType.Kinematic;
                tileBody.Position = ConvertUnits.ToSimUnits(Position);

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

        public void LoadBlock(Vector2 p)
        {
            switch(type)
            {
                case ItemTileType.Blank:
                    this.Rect = rect;
                    this.LayerDepth = layer;
                    tileBody.CollidesWith = Category.None;
                    break;

                case ItemTileType.Block:
                    this.Rect = rect;
                    this.LayerDepth = layer;
                    tileBody.CollidesWith = Category.All;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Type == ItemTileType.Blank)
                spriteBatch.Draw(texture, Position, rect, Color.Transparent);
            else
                spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(tileBody.Position), rect, Color.White, tileBody.Rotation, new Vector2(ConvertUnits.ToSimUnits(rect.Width / 2.0f), ConvertUnits.ToSimUnits(rect.Height / 2.0f)), 1f, SpriteEffects.None, 1f);
            //base.Draw(batch);
        }
    }
}

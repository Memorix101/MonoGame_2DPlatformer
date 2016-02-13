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
        Body rigidbody;

        public ItemTile(Vector2 p, float l, ItemTileType t)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
            this.Position = p;
            layer = l;
            type = t;

            if (t == ItemTileType.Blank)
                rect = new Rectangle(0, 0, 1, 1);
            else if (t == ItemTileType.Block)
                rect = new Rectangle(1473, 702, 32, 32);
            // rect = new Rectangle(290, 160, 32, 32);
            //rect = new Rectangle(12, 0, 32, 32);

            //Set rigidbody behaivior here
            rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1.0f, ConvertUnits.ToSimUnits(Position)); //default 1:64 ratio 1 meter = 64 pixel
            rigidbody.BodyType = BodyType.Kinematic;
            rigidbody.UserData = (string)"Tile";
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat1; // <- cat2 is floor cat

            LoadBlock(p);
        }


        public Body GetRigidbody
        {
            get { return rigidbody; }
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
                 (int)ConvertUnits.ToDisplayUnits(rigidbody.Position.X),
                 (int)ConvertUnits.ToDisplayUnits(rigidbody.Position.Y),
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
                    rigidbody.CollidesWith = Category.None;
                    break;

                case ItemTileType.Block:
                    this.Rect = rect;
                    this.LayerDepth = layer;
                    rigidbody.CollidesWith = Category.All;
                    break;

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Type == ItemTileType.Blank)
                spriteBatch.Draw(texture, Position, rect, Color.Transparent);
            else
                spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, rigidbody.Rotation, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, SpriteEffects.None, 1f);
            //base.Draw(batch);
        }
    }
}

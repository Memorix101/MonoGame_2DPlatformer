using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace MonoGame_2DPlatformer
{
    class TestActor
    {
        Body rigidbody;
        Texture2D texture;
        Vector2 position = new Vector2(0f, 0f);
        Rectangle rect;

        public TestActor(Vector2 p)
       {
           texture = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
           // texture = Game1.content.Load<Texture2D>("Test/box");
           rect = new Rectangle(544, 352, 32, 32);

            rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1.0f, ConvertUnits.ToSimUnits(p));
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.Restitution = 0f;
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat1; // default
            rigidbody.CollidesWith = Category.All;
        }

        public Body body
        {
            get { return body; }
        }

        public void Update()
        {
            //rigidbody.ApplyForce(new Vector2(2, 2));
            //Console.WriteLine(ConvertUnits.ToDisplayUnits(rigidbody.Position));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, rigidbody.Rotation, 
                new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, SpriteEffects.None, 1f);
        }
    }
}

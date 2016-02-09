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

        public TestActor()
       {
            //  texture = Game1.content.Load<Texture2D>("Sprites/tileset");
            texture = Game1.content.Load<Texture2D>("Test/box");

            rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(texture.Width), ConvertUnits.ToSimUnits(texture.Height), 1.0f, ConvertUnits.ToSimUnits(position));
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.Restitution = 0.3f;
            rigidbody.Friction = 0.5f;
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
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), null, Color.White, rigidbody.Rotation, 
                new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), 1f, SpriteEffects.None, 1f);
        }
    }
}

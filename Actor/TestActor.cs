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
        Body rectangle;
        Texture2D texture;

        public TestActor()
       {
            texture = Game1.content.Load<Texture2D>("Test/box");
            rectangle = BodyFactory.CreateRectangle(Game1.world, 1f, 1f, 1.0f);
            rectangle.BodyType = BodyType.Dynamic;
            rectangle.Position = new Vector2(3.5f, 5f);
            rectangle.Restitution = 0.3f;
            rectangle.Friction = 0.5f;
            rectangle.CollidesWith = Category.All;
        }

        public Body body
        {
            get { return body; }
        }

        public void Update()
        {
            //rectangle.ApplyForce(new Vector2(2, 2));
            //Console.WriteLine(ConvertUnits.ToDisplayUnits(rectangle.Position));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rectangle.Position), null, Color.White, rectangle.Rotation, 
                new Vector2( texture.Width / 2.0f, texture.Height / 2.0f), 1f, SpriteEffects.None, 1f);
        }
    }
}

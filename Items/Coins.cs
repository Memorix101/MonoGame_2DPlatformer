using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;


using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{
    class Coins : Sprite
    {
        Texture2D texture;
        //Vector2 p_pos;
        Rectangle coinRect = new Rectangle(0, 0, 32, 32);
       public Body rigidbody;

        float speed = 10f;

        float time;
        float rectFrame = 1;

        public Coins(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/coins_gold");
            this.Position = p;

            //Set rigidbody behaivior here
            rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(coinRect.Width), ConvertUnits.ToSimUnits(coinRect.Width), 1.0f, ConvertUnits.ToSimUnits(this.Position));
            rigidbody.BodyType = BodyType.Static;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat3; // cat3 is coins

            rigidbody.OnCollision += Rigidbody_OnCollision;
        }

        private bool Rigidbody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  throw new NotImplementedException();
            return false;
        }

        public void Update(GameTime gameTime)
        {
            this.Rect = coinRect;
            time += speed * Time.DeltaTime;
            rectFrame += speed * Time.DeltaTime;

            if (time >= 8f)
            {
                rectFrame = 1f;
                time = 0F;
            }
           
           // GameDebug.Log("HIIII " + time);

            if (rectFrame >= 1f)
                coinRect = new Rectangle(0, 0, 32, 32);
            if (rectFrame >= 2f)
                coinRect = new Rectangle(32, 0, 32, 32);
            if (rectFrame >= 3f)
                coinRect = new Rectangle(64, 0, 32, 32);
            if (rectFrame >= 4f)
                coinRect = new Rectangle(96, 0, 32, 32);
            if (rectFrame >= 5f)
                coinRect = new Rectangle(128, 0, 32, 32);
            if (rectFrame >= 6f)
                coinRect = new Rectangle(160, 0, 32, 32);
            if (rectFrame >= 7f)
                coinRect = new Rectangle(192, 0, 32, 32);
            if (rectFrame >= 8f)
                coinRect = new Rectangle(224, 0, 32, 32);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(texture, Position, coinRect, Color.White);
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), coinRect, Color.White, rigidbody.Rotation, new Vector2(coinRect.Width / 2.0f, coinRect.Height / 2.0f), 1f, SpriteEffects.None, 1f);
            //base.Draw(spriteBatch);
        }
    }
}

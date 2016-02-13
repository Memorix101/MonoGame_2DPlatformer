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
    class Exit : Sprite
    {
        Texture2D texture;
        //Vector2 p_pos;
        Rectangle coinRect = new Rectangle(32, 640, 32, 32);
        public Body rigidbody;

        public Exit(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
            this.Position = p;

            //Set rigidbody behaivior here
            rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(coinRect.Width), ConvertUnits.ToSimUnits(coinRect.Width), 1.0f, ConvertUnits.ToSimUnits(this.Position));
            rigidbody.BodyType = BodyType.Static;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat3; // cat3 is coins

            rigidbody.OnCollision += Rigidbody_OnCollision;
        }

        public Body GetRigidbody
        {
            get { return rigidbody; }
        }

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();
            return false;
        }

        public void Update(GameTime gameTime)
        {
            this.Rect = coinRect;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(texture, Position, coinRect, Color.White);
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), coinRect, Color.White, rigidbody.Rotation, new Vector2(coinRect.Width / 2.0f, coinRect.Height / 2.0f), 1f, SpriteEffects.None, 1f);
            //base.Draw(spriteBatch);
        }
    }
}

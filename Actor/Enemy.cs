﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;


using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{
    enum FaceDir
    {
        left,
        right,
    }

    class Enemy : Sprite
    {
        Texture2D texture;
        //Vector2 p_pos;
        Rectangle rect = new Rectangle(0, 896, 32, 32);
        public Body rigidbody;

        SoundEffect sn_kill_enemy;

        FaceDir faceDir;

        bool dirChange;
        bool killed;
        bool bite;

        float walkSpeed = 1f;

        float distance = 0.5f;
        float distanceUp = 0.35f;

        float speed = 5f;

        float time;
        float rectFrame = 1;

        public Enemy(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
            this.Position = p;

            sn_kill_enemy = Game1.content.Load<SoundEffect>("Sounds/Randomize3");

            killed = false;
            bite = false;
            faceDir = FaceDir.right;

            //Set rigidbody behaivior here
            // rigidbody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(rect.Width), ConvertUnits.ToSimUnits(rect.Height), 1.0f, ConvertUnits.ToSimUnits(this.Position));
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(rect.Width/2), 1.0f, ConvertUnits.ToSimUnits(this.Position));
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.FixedRotation = true;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 0f;
            rigidbody.CollisionCategories = Category.Cat3; // cat3 is coins

            rigidbody.OnCollision += Rigidbody_OnCollision;
        }

        public Body GetRigidbody
        {
            get { return rigidbody; }
        }

        public bool IsKilled
        {
            private set { killed = value; }
            get { return killed; }
        }

        public bool Bite
        {
            private set { bite = value; }
            get { return bite; }
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

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();

            if (other.CollisionCategories == Category.Cat2)
            {
                // me.IgnoreCollisionWith(other);
                bite = true;
            }

            return true;
        }

        void RayUp()
        {
            IsKilled = false;
            Func<Fixture, Vector2, Vector2, float, float> get_first_callback = delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {

                if (fixture.CollisionCategories == Category.Cat2)
                {
                    IsKilled = true;
                    sn_kill_enemy.Play();
                }

                return 0;
            };

                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(-distanceUp/2, -distanceUp));
                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(0, -distanceUp));
                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(distanceUp/2, -distanceUp));
        }

        void RaySide()
        {
            dirChange = false;
            Func<Fixture, Vector2, Vector2, float, float> get_first_callback = delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                if (fixture.CollisionCategories == Category.Cat4)
                {
                    dirChange = true;
                }

                return 0;
            };

            if (faceDir == FaceDir.right)
                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(distance, 0));
            if (faceDir == FaceDir.left)
                Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(-distance, 0));
        }


        public void Update(GameTime gameTime)
        {
            this.Rect = rect;
            if (!IsKilled)
            {
                rigidbody.LinearVelocity = new Vector2(walkSpeed, rigidbody.LinearVelocity.Y);

                RaySide();
                RayUp();

                if (dirChange)
                {
                    walkSpeed *= -1f;
                    dirChange = false;

                    if (faceDir == FaceDir.left)
                    {
                        faceDir = FaceDir.right;
                    }
                    else if (faceDir == FaceDir.right)
                    {
                        faceDir = FaceDir.left;
                    }

                }
                Frames();
            }
            else
            {
                rect = new Rectangle(64, 896, 32, 32);
            }
        }

        
        private void Frames()
        {
            time += speed * Time.DeltaTime;
            rectFrame += speed * Time.DeltaTime;

            if (time >= 3f)
            {
                rectFrame = 1f;
                time = 0F;
            }

            // GameDebug.Log("HIIII " + time);

            if (rectFrame >= 1f)
                rect = new Rectangle(0, 896, 32, 32);
            if (rectFrame >= 2f)
                rect = new Rectangle(32, 896, 32, 32);
            if (rectFrame >= 3f)
                rect = new Rectangle(64, 896, 32, 32);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(texture, Position, coinRect, Color.White);
            if(faceDir == FaceDir.left)
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, rigidbody.Rotation, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, SpriteEffects.None, 1f);
            else if (faceDir == FaceDir.right)
                spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), rect, Color.White, rigidbody.Rotation, new Vector2(rect.Width / 2.0f, rect.Height / 2.0f), 1f, SpriteEffects.FlipHorizontally, 1f);
            //base.Draw(spriteBatch);


#if DEBUG
            SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;

            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(distanceUp/2, -distanceUp)), Color.Yellow);
            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(0, -distanceUp)), Color.Yellow);
            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(-distanceUp/2, -distanceUp)), Color.Yellow);

            if (faceDir == FaceDir.right)
                spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(distance, 0)), Color.BlueViolet);
            else if (faceDir == FaceDir.left)
                spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(-distance, 0)), Color.BlueViolet);
#endif

        }
    }
}

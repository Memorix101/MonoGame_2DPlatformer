using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;

using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{
    enum PlayerDir
    {
        left,
        right,
    }

    class Player : Sprite
    {

        Texture2D texture;
        const float moveSpeed = 3f;
        const float jumpForce = -50f;
        int jumpCount = 0;

        Body rigidbody;
        PlayerDir playerDir;

        private const float gravity = 50f;

     //   float airTime;

        Rectangle playerRect = new Rectangle(0, 0, 32, 32);

        bool buttonDown;
        bool grounded;
        Vector2 velocity;
        bool inAir;

        public Player(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/wheelie_right");
            this.Position = p;
            this.LayerDepth = 1f;
            this.playerDir = PlayerDir.right;

            // Setup physics
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(playerRect.Height/2), 1f, ConvertUnits.ToSimUnits(this.Position));
            //Set rigidbody behaivior here
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.FixedRotation = true;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat2; // cat2 player
            rigidbody.CollidesWith = Category.All;

            rigidbody.OnCollision += Rigidbody_OnCollision; // Tack collision
        }

        public float Grav
        {
            get { return gravity; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public bool isGrounded
        {
            get { return grounded; }
            set { grounded = value; }
        }

        public override Rectangle TileBoundingBox
        {
            get
            {
                return new Rectangle(
                 (int)ConvertUnits.ToDisplayUnits(rigidbody.Position.X),
                 (int)ConvertUnits.ToDisplayUnits(rigidbody.Position.Y),
                    playerRect.Width,
                    playerRect.Height);
            }
        }

        /// <summary>
        /// ///////
        /// </summary>
        /// <param name="gameTime"></param>

        public void Update(GameTime gameTime)
        {
            this.Rect = playerRect;
            GameDebug.Log("-" + jumpCount); 
            Input(gameTime);
        }

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();

            //TODO Raycast for grounded stuff

            if (contact.IsTouching)
            {
                if (other.CollisionCategories == Category.Cat1)
                {
                    //GameDebug.Log("ASDASFASDGSD");
                    jumpCount = 0;
                   // isGrounded = true;
                }
            }

            return true;
        }

        void Jump(GameTime gameTime)
        {
            if(!inAir)
            {            
                rigidbody.ApplyForce(new Vector2(0f, jumpForce));  
            }           
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rigidbody.LinearVelocity = new Vector2(-moveSpeed, rigidbody.LinearVelocity.Y);
                playerDir = PlayerDir.left;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rigidbody.LinearVelocity = new Vector2(moveSpeed, rigidbody.LinearVelocity.Y);
                playerDir = PlayerDir.right;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !buttonDown)
            {
                GameDebug.Log("JUMP!!!!");
                Jump(gameTime);
                buttonDown = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && buttonDown && jumpCount <= 1)//&& isGrounded && !inAir)
            {
                GameDebug.Log("DO NOT JUMP!!!!!");
                jumpCount++;
                buttonDown = false;
            }

        //    Console.WriteLine("JUMP" + inAir + grounded);
            // p_pos.X += moveSpeed * Time.DeltaTime;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(playerDir == PlayerDir.left)
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), playerRect, Color.White, 0f, 
                new Vector2(playerRect.Width / 2.0f, playerRect.Height / 2.0f), 1f, SpriteEffects.FlipHorizontally, LayerDepth);
            else if (playerDir == PlayerDir.right)
             spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), playerRect, Color.White, 0f, 
                 new Vector2(playerRect.Width / 2.0f, playerRect.Height / 2.0f), 1f, SpriteEffects.None, LayerDepth);
            //base.Draw(spriteBatch);
        }
    }
}

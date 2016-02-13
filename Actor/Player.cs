using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.DebugView;

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
        float distance = 0.5f;

        Body rigidbody;
        PlayerDir playerDir;

        private const float gravity = 50f;

       // Rectangle playerRect = new Rectangle(0, 0, 32, 32);
        Rectangle playerRect = new Rectangle(512, 673, 32, 32);

        bool buttonDown;
        bool grounded;

        public Player(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
            this.Position = p;
            this.LayerDepth = 1f;
            this.playerDir = PlayerDir.right;

            // Setup physics
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(playerRect.Width / 2), 1f, ConvertUnits.ToSimUnits(this.Position));
            //Set rigidbody behaivior here
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.UserData = (string)"Player";
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

            GameDebug.Log("-" + rigidbody.Friction.ToString());

            Raycast();

            if (isGrounded)
            {
                  //  rigidbody.Friction = 1;
                jumpCount = 0;
                GameDebug.Log(isGrounded.ToString());
            }
            else
            {
                  //  rigidbody.Friction = 0f;
                GameDebug.Log(isGrounded.ToString());
            }
            
            CameraBounds();
            Input(gameTime);
        }

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();
          /*
            if (contact.IsTouching)
            {
                //  if (other.CollisionCategories == Category.Cat1)
                if (other.CollisionCategories == Category.Cat1)
                {
                    jumpCount = 0;
                    // isGrounded = true;
                }
            }
            */

            return true;
        }

        void Raycast()
        {
            isGrounded = false;
            Func<Fixture, Vector2, Vector2, float, float> get_first_callback = delegate (Fixture fixture, Vector2 point, Vector2 normal, float fraction)
            {
                isGrounded = true;
                return 0;
            };

            Game1.world.RayCast(get_first_callback, rigidbody.Position, rigidbody.Position + new Vector2(0, distance)); //* (Math.Abs(rigidbody.LinearVelocity.Y) + 1)
        }

        void CameraBounds()
        {
            float _pos = rigidbody.Position.X;
            float _posMax = ConvertUnits.ToSimUnits(Game1.camera.offsetRight - Rect.Width);
            float _posMin = ConvertUnits.ToSimUnits(Game1.camera.offsetLeft + Rect.Width);

            _pos = MathHelper.Clamp(rigidbody.Position.X, 0, rigidbody.Position.X);
            rigidbody.Position = new Vector2(_pos, rigidbody.Position.Y);

            if(rigidbody.Position.X >= _posMax)
            {
                Game1.camera.UpdatePos(ConvertUnits.ToDisplayUnits(rigidbody.Position.X));
                Game1.DebugCam.UpdatePos();
            }
        }

        void Jump(GameTime gameTime)
        {
            if (jumpCount <= 1)
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
                jumpCount += 1;
                Jump(gameTime);
                buttonDown = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && buttonDown)
            {
                GameDebug.Log("DO NOT JUMP!!!!!");
                buttonDown = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (playerDir == PlayerDir.left)
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), playerRect, Color.White, 0f, 
                new Vector2(playerRect.Width / 2.0f, playerRect.Height / 2.0f), 1f, SpriteEffects.FlipHorizontally, LayerDepth);
            else if (playerDir == PlayerDir.right)
             spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), playerRect, Color.White, 0f, 
                 new Vector2(playerRect.Width / 2.0f, playerRect.Height / 2.0f), 1f, SpriteEffects.None, LayerDepth);
            //base.Draw(spriteBatch);
            
#if DEBUG
            SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;
            spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(0, distance)), Color.Red);
#endif

        }
    }
}

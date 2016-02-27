using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
             
       SoundEffect sn_kill_player;

        Body rigidbody;
        PlayerDir playerDir;

        bool killed;

        private const float gravity = 50f;

        float time;
        float rectFrame = 1;
        float speed = 8f;

        SoundEffect sn_jump;

        // Rectangle playerRect = new Rectangle(0, 0, 32, 32);
        //  Rectangle playerRect = new Rectangle(512, 673, 32, 32);
        Rectangle playerRect = new Rectangle(512, 608, 32, 32);

        bool buttonDown;
        bool grounded;

        public Player(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
            this.Position = p;
            this.LayerDepth = 1f;
            this.playerDir = PlayerDir.right;
            killed = false;

            sn_kill_player = Game1.content.Load<SoundEffect>("Sounds/cute_low_impact_01");
            sn_jump = Game1.content.Load<SoundEffect>("Sounds/Jump");

            // Setup physics
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(playerRect.Width / 2), 1f, ConvertUnits.ToSimUnits(this.Position));
            //Set rigidbody behaivior here
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.SleepingAllowed = false;
            rigidbody.UserData = (string)"Player";
            rigidbody.FixedRotation = true;
            rigidbody.Restitution = 0f; // No bounciness
            rigidbody.Friction = 1f;
            rigidbody.CollisionCategories = Category.Cat2; // cat2 player
            rigidbody.CollidesWith = Category.All;

            rigidbody.OnCollision += Rigidbody_OnCollision; // Tack collision
        }

        public bool isDead
        {
            private set { killed = value; }
            get { return killed; }
        }

        public Body GetRigidbody
        {
            get { return rigidbody; }
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
            if (!killed)
            {
                this.Rect = playerRect;

                GameDebug.Log("-" + rigidbody.Friction.ToString());

                Raycast();

                if (isGrounded)
                {
                    //  rigidbody.Friction = 1;
                    jumpCount = 0;
                    GameDebug.Log(isGrounded.ToString());
                    playerRect = new Rectangle(512, 608, 32, 32);
                }
                else
                {
                    JumpFrames();
                    //  rigidbody.Friction = 0f;
                    // GameDebug.Log(isGrounded.ToString());
                }


                CameraBounds();
                Input(gameTime);
            }

        }

        private void Frames()
        {
            time += speed * Time.DeltaTime;
            rectFrame += speed * Time.DeltaTime;

            if (time >= 4f)
            {
                rectFrame = 1f;
                time = 0F;
            }

            // GameDebug.Log("HIIII " + time);

            if (rectFrame >= 1f)
                playerRect = new Rectangle(736, 608, 32, 32);
            if (rectFrame >= 2f)
                playerRect = new Rectangle(768, 608, 32, 32);
            if (rectFrame >= 3f)
                playerRect = new Rectangle(800, 608, 32, 32);
            if (rectFrame >= 4f)
                playerRect = new Rectangle(832, 608, 32, 32);
        }

        private void JumpFrames()
        {
            time += speed * Time.DeltaTime;
            rectFrame += speed * Time.DeltaTime;

            if (time >= 2f)
            {
                //rectFrame = 1f;
               // time = 0F;
            }

            // GameDebug.Log("HIIII " + time);

            if (rectFrame >= 1f)
                playerRect = new Rectangle(544, 608, 32, 32);
            if (rectFrame >= 2f)
                playerRect = new Rectangle(576, 608, 32, 32);
        }

        public void ReceiveDamage()
        {
            if(!isDead)
                sn_kill_player.Play();

            killed = true;
            rigidbody.CollisionCategories = Category.None;
            // rigidbody.Dispose();
        }

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();
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
            float _posDown = ConvertUnits.ToSimUnits(Screen.height - Rect.Width);

            _pos = MathHelper.Clamp(rigidbody.Position.X, 0, rigidbody.Position.X);
            rigidbody.Position = new Vector2(_pos, rigidbody.Position.Y);

            if(rigidbody.Position.X >= _posMax)
            {
                Game1.camera.UpdatePos(ConvertUnits.ToDisplayUnits(rigidbody.Position.X));
                Game1.DebugCam.UpdatePos();
            }

            if (rigidbody.Position.Y >= _posDown)
            {
                ReceiveDamage();
            }
        }

        void Jump(GameTime gameTime)
        {
            if (jumpCount <= 1)
            {
                sn_jump.Play();
                rigidbody.ApplyForce(new Vector2(0f, jumpForce));
            }
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
              //  rigidbody.LinearVelocity = new Vector2(-moveSpeed, rigidbody.LinearVelocity.Y);
              rigidbody.Position = new Vector2(rigidbody.Position.X -moveSpeed * Time.DeltaTime, rigidbody.Position.Y);
                playerDir = PlayerDir.left;

                if (isGrounded)
                    Frames();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                //  rigidbody.LinearVelocity = new Vector2(moveSpeed, rigidbody.LinearVelocity.Y);
                rigidbody.Position = new Vector2(rigidbody.Position.X + moveSpeed * Time.DeltaTime, rigidbody.Position.Y);
                playerDir = PlayerDir.right;

                if (isGrounded)
                    Frames();
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
            if (!killed)
            {
#if DEBUG
                SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;
                spriteBatch.DrawLine(ConvertUnits.ToDisplayUnits(rigidbody.Position), ConvertUnits.ToDisplayUnits(rigidbody.Position + new Vector2(0, distance)), Color.Red);
#endif
            }

        }
    }
}

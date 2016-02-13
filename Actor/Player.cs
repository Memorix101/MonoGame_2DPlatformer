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

        float _angle;
        float l;
        Vector2 point1;
        Vector2 d;
        Vector2 point2;
        Vector2 normal;
        Vector2 point;
        bool hitClosest;

        /// <summary>
        /// /
        /// </summary>

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
        bool inAir;

        public Player(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/TuxJR"); //"Sprites/wheelie_right"
            this.Position = p;
            this.LayerDepth = 1f;
            this.playerDir = PlayerDir.right;

            // Setup physics
            rigidbody = BodyFactory.CreateCircle(Game1.world, ConvertUnits.ToSimUnits(playerRect.Height / 2), 1f, ConvertUnits.ToSimUnits(this.Position));
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

            GameDebug.Log("-" + jumpCount + " - ");
            ////

            _angle = 0f;
            l = 11.0f;
            point1 = new Vector2(0.0f, 10.0f);
            d = new Vector2(l * (float)Math.Cos(_angle), l * (float)Math.Sin(_angle));
            point2 = point1 + d;

            point = ConvertUnits.ToDisplayUnits(rigidbody.Position);
            normal = Vector2.Zero;

            ////

            Raycast();
            CameraBounds();
            Input(gameTime);
        }

        private bool Rigidbody_OnCollision(Fixture me, Fixture other, Contact contact)
        {
            //  throw new NotImplementedException();

            //TODO Raycast for grounded stuff

            if (contact.IsTouching)
            {
                //  if (other.CollisionCategories == Category.Cat1)
                if (other.CollisionCategories == Category.Cat1)
                {
                    //GameDebug.Log("ASDASFASDGSD");
                    jumpCount = 0;
                    // isGrounded = true;
                }
            }

            return true;
        }

        void Raycast()
        {
            Game1.world.RayCast((f, p, n, fr) =>
            {
                Body body = f.Body;
                if (body.UserData != null)
                {
                    int index = (int)body.UserData;
                    if (index == 0)
                    {
                        // filter
                        return -1.0f;
                    }
                }

                hitClosest = true;
                point = p;
                normal = n;
                return fr;
            }, point1, point2);

            if (hitClosest)
            {
                GameDebug.Log("CASE IF");
                /*
                Game1.DebugView.BeginCustomDraw(ref Game1.camera.projection, ref Game1.camera.view);
                Game1.DebugView.DrawPoint(point, .5f, new Color(0.4f, 0.9f, 0.4f)); 

                Game1.DebugView.DrawSegment(point1, point, new Color(0.8f, 0.8f, 0.8f));
                Vector2 head = point + 0.5f * normal;
                Game1.DebugView.DrawSegment(point, head, new Color(0.9f, 0.9f, 0.4f));
                Game1.DebugView.EndCustomDraw();
                */
            }
            else
            {

                GameDebug.Log("CASE ELSE");


                /*
                Matrix view2 = Matrix.CreateScale(32); //default 32
                view2 *= Game1.DebugCam.view;

                Game1.DebugView.BeginCustomDraw(ref Game1.camera.projection, ref view2);
                Game1.DebugView.DrawString(0,0,"Press 1-5 to drop stuff, m to change the mode");
                Game1.DebugView.DrawSegment(point1, point2, new Color(0.8f, 0.8f, 0.8f));
                Game1.DebugView.EndCustomDraw();
                */
            }

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
                jumpCount += 1;
                buttonDown = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && buttonDown && jumpCount <= 1)//&& isGrounded && !inAir)
            {
                GameDebug.Log("DO NOT JUMP!!!!!");
                buttonDown = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

#if DEBUG
            SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;
            spriteBatch.DrawLine(point1, point2, Color.Yellow);
#endif

            if (playerDir == PlayerDir.left)
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), playerRect, Color.White, 0f, 
                new Vector2(playerRect.Width / 2.0f, playerRect.Height / 2.0f), 1f, SpriteEffects.FlipHorizontally, LayerDepth);
            else if (playerDir == PlayerDir.right)
             spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(rigidbody.Position), playerRect, Color.White, 0f, 
                 new Vector2(playerRect.Width / 2.0f, playerRect.Height / 2.0f), 1f, SpriteEffects.None, LayerDepth);
            //base.Draw(spriteBatch);
        }
    }
}

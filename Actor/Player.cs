using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

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
        const float moveSpeed = 100f;
        Vector2 pos;

        Body playerBody;

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
            this.Position = p;  pos = p;
            this.LayerDepth = 1f;
            this.playerDir = PlayerDir.right;

            /// Setup physics

            // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
            // 1 meters equals 64 pixels here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            playerBody = BodyFactory.CreateRectangle(Game1.world, ConvertUnits.ToSimUnits(playerRect.Width), ConvertUnits.ToSimUnits(playerRect.Height), 1f);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Position = ConvertUnits.ToSimUnits(this.Position);
            playerBody.CollidesWith = Category.All;
        

            // Give it some bounce and friction
            //playerBody.Restitution = 0.3f;
            //playerBody.Friction = 0.5f;
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

        /// <summary>
        /// ///////
        /// </summary>
        /// <param name="gameTime"></param>

        public void Update(GameTime gameTime)
        {
            this.Rect = playerRect;
            this.Position = pos;

            /*
          //  pos.Y += Grav * airTime * Time.DeltaTime;
         //   pos.Y -= velocity.Y * Time.DeltaTime;

            /*
            if (velocity.Y > 50f * Time.DeltaTime)
                velocity.Y -= gravity * Time.DeltaTime;
            else
                velocity.Y = 0f;
            */
            /*
            if (!grounded)
            {
         //       airTime += 5f * Time.DeltaTime; //fall back to floor
                inAir = true;
            }
            else if (grounded)
            {
                if (inAir)
                    velocity.Y = 0f;

         //       airTime = 0f;
                inAir = false;
            }
            */
            Input(gameTime);
        }

        void Jump(GameTime gameTime)
        {
            if(!inAir)
            {
                velocity.Y = 200f;                
            }           
        }

        private void Input(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                pos.X -= moveSpeed * Time.DeltaTime;
                playerDir = PlayerDir.left;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                pos.X += moveSpeed * Time.DeltaTime;
                playerDir = PlayerDir.right;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !buttonDown)
            {
                GameDebug.Log("JUMP!!!!");
                Jump(gameTime);
                buttonDown = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && buttonDown && isGrounded && !inAir)
            {
                GameDebug.Log("DO NOT JUMP!!!!!");
                buttonDown = false;
            }

            Console.WriteLine("JUMP" + inAir + grounded);
            // p_pos.X += moveSpeed * Time.DeltaTime;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(playerDir == PlayerDir.left)
            spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(playerBody.Position), playerRect, Color.White, 0f, new Vector2(ConvertUnits.ToSimUnits(playerRect.Width / 2.0f), ConvertUnits.ToSimUnits(playerRect.Height / 2.0f)), 1f, SpriteEffects.FlipHorizontally, LayerDepth);
            else if (playerDir == PlayerDir.right)
             spriteBatch.Draw(texture, ConvertUnits.ToDisplayUnits(playerBody.Position), playerRect, Color.White, 0f, new Vector2(ConvertUnits.ToSimUnits(playerRect.Width / 2.0f), ConvertUnits.ToSimUnits(playerRect.Height / 2.0f)), 1f, SpriteEffects.None, LayerDepth);
            //base.Draw(spriteBatch);
        }
    }
}

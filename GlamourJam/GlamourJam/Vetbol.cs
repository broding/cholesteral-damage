using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Flakcore;
using System.Diagnostics;

namespace GlamourJam
{
	class Vetbol:Node
	{
		public Sprite image = new Sprite();
		public bool isSticking = false;
		public PlayerIndex player = PlayerIndex.One;
		public bool onfloor = false;
		public GamePadState padState;
		public GamePadState prevPadState;
		public int maxSpeed = 350;
        public int jumpSpeed = 500;
        public int extraJump = 50;
        public float speedX = 0;
        public float speedY = 0;
		public Vector2 jumpDirection = new Vector2(0, -1);
        public bool capturing = false;
        public PlayerIndex index;
        public string CollisionState = "idle";

		public Vetbol(PlayerIndex playerIndex)
		{
            index = playerIndex;
			Position = new Vector2(100, 100);
			image.LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblob"), 48, 48);
			image.AddAnimation("IDLE", new int[1] { 0 }, 0);
			image.AddAnimation("CRAWLING", new int[1] { 1 }, 0);
			image.AddAnimation("JUMP", new int[1] { 2 }, 0);
			image.Position = new Vector2(24, 14);
			Width = 32;
			Height = 32;
			image.Origin = new Vector2(24, 24);
			AddChild(image);

			Sprite bb = new Sprite();
			bb = Sprite.CreateRectangle(new Vector2(Width, Height), Color.Aqua);
			bb.Alpha = 0.5f;
			AddChild(bb);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
            Controller.Collide(this, "tilemap", Collision);
            Controller.Collide(this, "capturePoint", null, BeingCaptured);
			padState = GamePad.GetState(index);

			if (speedX < 0)
			{
				image.Facing = Facing.Right;
			} else if (speedX > 0)
			{
				image.Facing = Facing.Left;
			}

			//Move when sticking
            speedY += 15;
            if(CollisionState == "idle"){
            //Move Horizontally
			if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
            {
                if (onfloor)
                {
                    if ((-1 * maxSpeed) <= speedX && speedX <= (1 * maxSpeed))
                    {
                        speedX = padState.ThumbSticks.Left.X * maxSpeed;
                    }
                 }else{
                     if ((-1 * maxSpeed) <= speedX && speedX <= (1 * maxSpeed))
                     {
                         speedX = padState.ThumbSticks.Left.X * (maxSpeed);
                     }
                }
			} else
            {
                if (onfloor)
                {
                    speedX *= 0.90f;
                }
                else
                {
                    speedX *= 0.99f;
                }
			}

                if (onfloor)
                {
                    if(speedY>=5){
                        //Jump
                        if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                        {
                            //Jump();
                            onfloor = false;
                            speedY = -jumpSpeed;
                        }
                    }
                }
            }
            if (CollisionState=="bottom")
            {
               /* speedY = 0;
                //Move Horizontally
                if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
                {
                    speedX = padState.ThumbSticks.Left.X * maxSpeed;
                }
                else
                {
                    speedX = 0;
                }
                
              
                if (!onfloor)
                {
                   // CollisionState = "idle";
                }
                else
                {
                    speedY = 0;
                }
                //Jump
                if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                {
                    //Jump();
                    speedY = -jumpSpeed;
                    CollisionState = "idle";
                }*/
            }
            
            if(CollisionState == "left"){
                //Move Horizontally
              /* if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
                {
                    if ((-1 * maxSpeed) <= speedX && speedX <= (1 * maxSpeed))
                    {
                        speedX = padState.ThumbSticks.Left.X * maxSpeed;
                    }
                }
                  else
               {
                  speedX = 0;
               }*/
                speedY = 0f;
                //Jump
                if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                {
                    //Jump();
                    speedX += jumpSpeed + extraJump;
                    speedY = -jumpSpeed/2;
                    CollisionState = "idle";
                }
                if ((padState.ThumbSticks.Left.X > 0))
                {
                  //  CollisionState = "idle";
                }
            }
             if(CollisionState == "right"){
                 //Move Horizontally
               /* if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
                {
                     if ((-1 * maxSpeed) <= speedX && speedX <= (1 * maxSpeed))
                     {
                         speedX = padState.ThumbSticks.Left.X * maxSpeed;
                     }
                }
                   else
                {
                   speedX = 0;
                }*/
                 speedY *= 0f;
                 //Jump
                 if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                 {
                     //Jump();
                     speedX -= jumpSpeed + extraJump;
                     speedY = -jumpSpeed/2;
                     CollisionState = "idle";
                 }
                 if((padState.ThumbSticks.Left.X < 0)){
                    // CollisionState = "idle";
                 }
            }
            if(CollisionState == "top"){
                 speedY += 5;
            }
            if (isSticking)
                /*{
                    speedX = padState.ThumbSticks.Left.X * (maxSpeed / 2);
                    speedY = -padState.ThumbSticks.Left.Y * (maxSpeed / 2);
                } else
                //Gravity
                {
                    speedY += 5;
                }*/

                speedY *= 0.95f;
            Velocity.X = speedX;
            Velocity.Y = speedY;

           // System.Diagnostics.Debug.WriteLine("[VetBol]State:["+CollisionState+"]V.Y:[" + Velocity.Y + "]V.X[" + Velocity.X + "]");
			//Jump
			//if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed && onfloor)
			//{
			//	onfloor = false;
				//Jump();
			//	Velocity.Y = -jumpSpeed;
			//}


			//ANIMATIONS
			if (Velocity.X < 0)
			{
				image.Facing = Facing.Right;
				image.Position.X = 26;
				image.PlayAnimation("CRAWLING");
			} else if (Velocity.X > 0)
			{
				image.Facing = Facing.Left;
				image.Position.X = 6;
				image.PlayAnimation("CRAWLING");
			}
			if (Velocity.X > -10 && Velocity.X < 15)
			{
				image.PlayAnimation("IDLE");
			}
			if (CollisionState == "bottom")
			{
				image.Rotation = MathHelper.ToRadians(0);
			} else if (CollisionState == "left")
			{
				image.Rotation = MathHelper.ToRadians(90);
			} else if (CollisionState == "right")
			{
				image.Rotation = MathHelper.ToRadians(-90);
			} else if (CollisionState == "idle")
			{
				image.Rotation = MathHelper.ToRadians(0);
			}
			if ((!onfloor && CollisionState == "idle") || Velocity.Y > 45)
			{
				image.PlayAnimation("JUMP");
				switch (image.Facing)
				{
					case Facing.Left:
						image.Rotation = -(float)Math.Atan2(Velocity.X, Velocity.Y) - MathHelper.ToRadians(225);
						break;
					case Facing.Right:
						image.Rotation = -(float)Math.Atan2(Velocity.X, Velocity.Y) - MathHelper.ToRadians(-225);
						break;
				}

			}

			//RESET FOR NEXT FRAME
			isSticking = false;
			prevPadState = padState;
		}
        public bool BeingCaptured(Node player, Node capturePoint)
        {
                (capturePoint as CapturePoint).startCapturing(this);

                (capturePoint as CapturePoint).isCollidingPlayer = true;

            return false;
        }
		public void Collision(Node player, Node collidingTile)
        {
            /*jumpDirection = Vector2.Zero;

            if (player.Touching.Bottom)
            {
                jumpDirection.Y = -1;
            }
            if (player.Touching.Left)
            {
                jumpDirection.X = 1;
            }
            if (player.Touching.Right)
            {
                jumpDirection.X = -1;
            }
            if (player.Touching.Top)
            {
                jumpDirection.Y = 1;
            }*/
            if (player.Touching.Bottom)
            {
                onfloor = true;
                speedY = 0;
               // CollisionState = "bottom";
            }
            
            if (player.Touching.Left){
                CollisionState = "left";
                speedX = 0;
            }
            if (player.Touching.Right)
            {
                CollisionState = "right";
                speedX = 0;
            }
            if (player.Touching.Top)
            {
                speedY = 0;
            }

            //System.Diagnostics.Debug.WriteLine("[VetBol]CollisionState:" + CollisionState);
            //if (player.Touching.Bottom || player.Touching.Left || player.Touching.Right)
            //{
            //	onfloor = true;
            //}
            //if (player.Touching.Left || player.Touching.Right || player.Touching.Top)
            //{
            //	isSticking = true;
            //	onfloor = true;
            //}
        }

		public void Jump()
		{
			int tempJumpSpeed = jumpSpeed;
			if (jumpDirection.X != 0 && jumpDirection.Y != 0)
			{
				tempJumpSpeed /= 2;
			}
			Velocity = jumpDirection * tempJumpSpeed;
		}
	}
}

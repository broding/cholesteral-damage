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
using GlamourJam.States;
using Microsoft.Xna.Framework.Audio;

namespace GlamourJam
{
	class Vetbol:Node
	{
        public static GameState state;

        private float bombTimer;
        private float bombSpawnTime = 1000;

		public Sprite image = new Sprite();
		public bool isSticking = false;
		public PlayerIndex player = PlayerIndex.One;
		public bool onfloor = false;
		public GamePadState padState;
		public GamePadState prevPadState;
		public int maxSpeed = 350;
		public int jumpSpeed = 500;
		public int extraJump = 150;
        private int wallJumpCount = 0;
		public float speedX = 0;
		public float speedY = 0;
		public Vector2 jumpDirection = new Vector2(0, -1);
		public bool capturing = false;
		public PlayerIndex index;
		public string CollisionState = "idle";

		private SoundEffect soundEffectWalk;
		private SoundEffect soundEffectLand;
		private SoundEffect soundEffectJump;
		private float soundTimer = 0;

		public Vetbol(PlayerIndex playerIndex)
		{
			index = playerIndex;
			Position = new Vector2(100, 100);
			image.LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblob"), 48, 48);
			image.AddAnimation("IDLE", new int[1] { 0 }, 0);
			image.AddAnimation("CRAWLING", new int[2] { 1,2 }, 0.5f);
            image.AddAnimation("JUMP", new int[1] { 3 }, 0);
            image.AddAnimation("ONWALL", new int[1] { 4 }, 0);
			image.Position = new Vector2(24, 14);
			Width = 32;
			Height = 32;
			image.Origin = new Vector2(24, 24);
			AddChild(image);

			soundEffectWalk = Controller.Content.Load<SoundEffect>("sounds/walking");
			soundEffectLand = Controller.Content.Load<SoundEffect>("sounds/landing");
			soundEffectJump = Controller.Content.Load<SoundEffect>("sounds/jump");

			/*Sprite bb = new Sprite();
			bb = Sprite.CreateRectangle(new Vector2(Width, Height), Color.Aqua);
			bb.Alpha = 0.5f;
			AddChild(bb);*/
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Controller.Collide(this, "tilemap", Collision);
			Controller.Collide(this, "capturePoint", null, BeingCaptured);
			padState = GamePad.GetState(index);
            wallJumpCount--;
            if (wallJumpCount <= 0)
            {
                wallJumpCount = 0;
            }
			if (speedX < 0)
			{
				image.Facing = Facing.Right;
			} else if (speedX > 0)
			{
				image.Facing = Facing.Left;
			}

            this.bombTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (padState.IsButtonDown(Buttons.B) && this.bombTimer > this.bombSpawnTime)
            {
                this.bombTimer = 0;
                state.SpawnBomb(this.Position);
            }

			//Move when sticking
			speedY += 15;
			if (CollisionState == "idle")
			{
				//Move Horizontally
				if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
				{
					if (onfloor)
					{
						//if ((-1 * maxSpeed) <= speedX && speedX <= (1 * maxSpeed))
						//{
							speedX = padState.ThumbSticks.Left.X * maxSpeed;

							soundTimer += gameTime.ElapsedGameTime.Milliseconds;
							if (soundEffectWalk.Duration.Milliseconds <= soundTimer + 140.0f)
							{
								soundTimer = 0;
								soundEffectWalk.Play(0.15f, 0, 0);
							}
						//}
					} else
					{
						if (wallJumpCount==0)
						{
							speedX = padState.ThumbSticks.Left.X * (maxSpeed);
						}
					}
				} else
				{
					if (onfloor)
					{
						speedX *= 0.90f;
					} else
					{
						speedX *= 0.99f;
					}
				}

				if (onfloor)
				{
					if (speedY >= 5)
					{
						//Jump
						if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
						{
							soundEffectJump.Play(0.5f, 0, 0);
							//Jump();
							onfloor = false;
                            speedY = -650;
						}
					}
				}
			}
			if (CollisionState == "bottom")
			{
			}

			if (CollisionState == "left")
			{
				//Move Horizontally
				speedY = 0f;
				//Jump
				if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
				{
					soundEffectJump.Play(0.5f, 0, 0);
                    if (padState.ThumbSticks.Left.X >= 0.3)
                    {
                        //Jump();
                        speedX += (jumpSpeed + extraJump) * padState.ThumbSticks.Left.X;
                        speedY -= ((jumpSpeed) * padState.ThumbSticks.Left.Y) + extraJump;
                        Vector2 speed = new Vector2(speedX, speedY);
                        speed.Normalize();
                        speed *= 650;
                        speedX = speed.X;
                        speedY = speed.Y;
                    }
                    else
                    {
                        //Jump();
                        speedX += (jumpSpeed + extraJump) *0.3f;
                        speedY -= ((jumpSpeed) * padState.ThumbSticks.Left.Y) + extraJump;
                        Vector2 speed = new Vector2(speedX, speedY);
                        speed.Normalize();
                        speed *= 650;
                        speedX = speed.X;
                        speedY = speed.Y;
                        wallJumpCount =10;
                    }
                    CollisionState = "idle";
				}
				if ((padState.ThumbSticks.Left.X > 0))
				{
					//  CollisionState = "idle";
				}
			}
			if (CollisionState == "right")
			{
				speedY *= 0f;
				//Jump
				if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
				{
					soundEffectJump.Play(0.5f, 0, 0);
                    if (padState.ThumbSticks.Left.X <= -0.3)
                    {
                        //Jump();
                        speedX += (jumpSpeed + extraJump) * padState.ThumbSticks.Left.X;
                        speedY -= ((jumpSpeed) * padState.ThumbSticks.Left.Y) + extraJump;
                        Vector2 speed = new Vector2(speedX, speedY);
                        speed.Normalize();
                        speed *= 650;
                        speedX = speed.X;
                        speedY = speed.Y;
                    }
                    else
                    {
                        //Jump();
                        speedX += (jumpSpeed + extraJump) * -0.3f;
                        speedY -= ((jumpSpeed) * padState.ThumbSticks.Left.Y) + extraJump;
                        Vector2 speed = new Vector2(speedX, speedY);
                        speed.Normalize();
                        speed *= 650;
                        speedX = speed.X;
                        speedY = speed.Y;
                        wallJumpCount =10;
                    }
                   
					CollisionState = "idle";
				}
				if ((padState.ThumbSticks.Left.X < 0))
				{
					// CollisionState = "idle";
				}
			}
			if (CollisionState == "top")
			{
				speedY += 5;
			}
			if (isSticking)
				speedY *= 0.95f;
			Velocity.X = speedX;
			Velocity.Y = speedY;


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
                image.Rotation = MathHelper.ToRadians(0);
                image.Facing = Facing.Right;
                image.Position.X = 26;
                image.PlayAnimation("ONWALL");
				//image.Rotation = MathHelper.ToRadians(90);
			} else if (CollisionState == "right")
            {
                image.Rotation = MathHelper.ToRadians(0);
                image.Facing = Facing.Left;
                image.Position.X = 6;
                image.PlayAnimation("ONWALL");
				//image.Rotation = MathHelper.ToRadians(-90);
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
			if (!onfloor)
				soundEffectLand.Play(0.5f, 0, 0);
			if (player.Touching.Bottom)
			{
				onfloor = true;
				speedY = 0;
				speedX = 0;
				// CollisionState = "bottom";
			}

			if (player.Touching.Left)
			{
                if (!onfloor)
                {
                    CollisionState = "left";
                    speedX = 0;
                }
			}
			if (player.Touching.Right)
			{
                if (!onfloor)
                {
                    CollisionState = "right";
                    speedX = 0;
                }
			}
			if (player.Touching.Top)
			{
				speedY = 0;
			}
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

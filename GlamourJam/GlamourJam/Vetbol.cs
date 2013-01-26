using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Flakcore;

namespace GlamourJam
{
	class Vetbol:Sprite
	{
		public bool isSticking = false;
		public PlayerIndex player = PlayerIndex.One;
		public bool onfloor = false;
		public GamePadState padState;
		public GamePadState prevPadState;
		public int maxSpeed = 500;
		public int jumpSpeed = 1000;
		public Vector2 jumpDirection = new Vector2(0, -1);

		public Vetbol()
		{
			Position = new Vector2(100, 100);
			LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblob"), 48, 48);
			AddAnimation("IDLE", new int[1] { 0 }, 0);
			AddAnimation("CRAWLING", new int[1] { 1 }, 0);
			AddAnimation("JUMP", new int[1] { 2 }, 0);
		}

		protected override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, WorldProperties worldProperties)
		{
			base.Draw(spriteBatch, worldProperties);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Controller.Collide(this, "tilemap", Collision);
			padState = GamePad.GetState(player);

			//Move when sticking
			if (isSticking)
			{
				Velocity.X = padState.ThumbSticks.Left.X * (maxSpeed / 2);
				Velocity.Y = -padState.ThumbSticks.Left.Y * (maxSpeed / 2);
			} else
			//Gravity
			{
				Velocity.Y += 5;
			}

			//Move Horizontally
			if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
			{
				Velocity.X = padState.ThumbSticks.Left.X * maxSpeed;
			} else
			{
				Velocity.X = 0;
			}

			//Jump
			if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed && onfloor)
			{
				onfloor = false;
				//Jump();
				Velocity.Y = -jumpSpeed;
			}


			//ANIMATIONS
			if ((onfloor && padState.ThumbSticks.Left.X == 0))
			{
			}
			if (Velocity == Vector2.Zero)
			{
				PlayAnimation("IDLE");
			} else if (!onfloor && !isSticking)
			{
				PlayAnimation("JUMP");
			} else
			{
				PlayAnimation("CRAWLING");
			}

			/*if (!onfloor && !isSticking)
			{
				PlayAnimation("JUMP");
			} else if (onfloor && padState.ThumbSticks.Left.X != 0)
			{
				PlayAnimation("CRAWLING");
			} else if (isSticking)
			{
			} else if (padState.ThumbSticks.Left == Vector2.Zero)
			{
				PlayAnimation("IDLE");
			}*/

			//RESET FOR NEXT FRAME
			isSticking = false;
			prevPadState = padState;
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
			}
			if (player.Touching.Left || player.Touching.Right || player.Touching.Top)
			{
				isSticking = true;
				onfloor = true;
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

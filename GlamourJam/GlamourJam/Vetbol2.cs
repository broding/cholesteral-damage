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
	class Vetbol2:Sprite
	{
		bool onFloor;
		bool onWallLeft;
		int maxSpeed = 500;
		float airVelocityX;
		GamePadState padState;
		GamePadState prevPadState;

		public Vetbol2()
		{
			Position = new Vector2(100, 100);
			LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblob"), 48, 48);
			AddAnimation("IDLE", new int[1] { 0 }, 0);
			AddAnimation("CRAWLING", new int[1] { 1 }, 0);
			AddAnimation("JUMP", new int[1] { 2 }, 0);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Controller.Collide(this, "tilemap", Collision);
			padState = GamePad.GetState(PlayerIndex.One);

			Velocity.Y += 5;
			if (Velocity.Y > 55)
			{
				onFloor = false;
			}

			if (onFloor)
			{
				Velocity.X = padState.ThumbSticks.Left.X * maxSpeed;
			} else if (onWallLeft)
			{
				Velocity.X = (padState.ThumbSticks.Left.X * maxSpeed);
				Velocity.Y = 250;
			} else
			{
				Velocity.X = (padState.ThumbSticks.Left.X * maxSpeed) + airVelocityX;
				airVelocityX *= 0.995f;
			}

			if (Velocity.X < -55 || Velocity.X > 55)
			{
				onWallLeft = false;
			}

			if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed && (onFloor || onWallLeft))
			{
				Jump();
			}

			//onWallLeft = false;
			prevPadState = padState;
		}

		private void Collision(Node player, Node tile)
		{
			if (player.Touching.Bottom)
			{
				onFloor = true;
				airVelocityX = 0;
				Velocity.Y = 0;
			}
			if (player.Touching.Left)
			{
				onWallLeft = true;
				Velocity.X = 0;
			}
		}

		private void Jump()
		{
			onFloor = false;
			Velocity.Y = -1000;
			if (onWallLeft)
			{
				airVelocityX = 1500;
				Velocity.X = (padState.ThumbSticks.Left.X * maxSpeed) + airVelocityX;
			}
		}
	}
}

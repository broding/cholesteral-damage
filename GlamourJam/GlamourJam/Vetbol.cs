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

		public Vetbol()
		{
			Position = new Vector2(100, 100);
			LoadTexture("images/kikker");
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
				Velocity.X = padState.ThumbSticks.Left.X * 500;
				Velocity.Y = -padState.ThumbSticks.Left.Y * 500;
			} else
			//Gravity
			{
				Velocity.Y += 5;
			}

			//Move Horizontally
			if (!(padState.ThumbSticks.Left.X == 0))
			{
				Velocity.X = padState.ThumbSticks.Left.X * 500;
			} else
			{
				Velocity.X = 0;
			}

			//Jump
			if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed && onfloor)
			{
				onfloor = false;
				Velocity.Y = -1000;
			}

			isSticking = false;
			//onfloor = false;
			prevPadState = padState;
		}

		public void Collision(Node player, Node collider)
		{
			isSticking = true;
			if (player.Touching.Bottom)
			{
				onfloor = true;
			}
		}
	}
}

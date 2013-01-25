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
			Position = new Vector2(0, 0);
			LoadTexture("images/kikker");
		}

		protected override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, WorldProperties worldProperties)
		{
			base.Draw(spriteBatch, worldProperties);
		}

		public override void Update(GameTime gameTime)
		{
			padState = GamePad.GetState(player);

			if (Position.Y + Height >= Controller.ScreenSize.Y)
			{
				onfloor = true;
				Velocity.Y = 0;
				Position.Y = Controller.ScreenSize.Y - Height;
			} else {
				Velocity.Y += 5;
			}

			if (!(padState.ThumbSticks.Left.X == 0))
			{
				Position.X += padState.ThumbSticks.Left.X;
			}
			if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed && onfloor)
			{
				Velocity.Y = -1000;
				onfloor = false;
			}

			prevPadState = padState;

			base.Update(gameTime);
		}
	}
}

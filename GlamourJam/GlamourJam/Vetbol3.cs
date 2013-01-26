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
	class Vetbol3:Sprite
	{
		bool onFloor;
		bool onWallLeft;
		int maxSpeed = 500;
		float airVelocityX;
		GamePadState padState;
		GamePadState prevPadState;

		public Vetbol3()
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
			Velocity.X = padState.ThumbSticks.Left.X * maxSpeed;

			prevPadState = padState;
		}

		private void Collision(Node player, Node tile)
		{
			System.Diagnostics.Debug.WriteLine(player.Touching.Bottom);
		}
	}
}

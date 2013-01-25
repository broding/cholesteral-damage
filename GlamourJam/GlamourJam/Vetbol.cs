using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;

namespace GlamourJam
{
	class Vetbol:Sprite
	{
		public bool isSticking = false;

		public Vetbol()
		{
			Position = new Vector2(0, 0);
			Width = 20;
			Height = 20;
			Color = Color.Red;
		}

		protected override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, WorldProperties worldProperties)
		{
			base.Draw(spriteBatch, worldProperties);
		}

		public override void Update(GameTime gameTime)
		{

			base.Update(gameTime);
		}
	}
}

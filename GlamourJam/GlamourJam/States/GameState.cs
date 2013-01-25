using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace GlamourJam.States
{
    class GameState : State
    {
        public GameState()
        {
			Vetbol player = new Vetbol();
			AddChild(player);
        }

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
		}
    }
}

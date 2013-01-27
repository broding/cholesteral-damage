using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Utils;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GlamourJam
{
    class EndScreen : State
    {
        private Sprite heart;
		public GamePadState padState;

		public EndScreen(PlayerIndex playerIndex, Color color)
		{

            Sprite bg = new Sprite();
            bg.LoadTexture(@"Assets/HeartBG");
            bg.Scale /= Controller.CurrentDrawCamera.zoom;
            this.AddChild(bg);

            heart = new Sprite();
            heart.LoadTexture(@"Assets/Heart");
            heart.Scale /= (Controller.CurrentDrawCamera.zoom * 1.1f);
            heart.Position.X += 110;
            this.AddChild(heart);

			Label lbl = new Label("Player " + (float)(playerIndex + 1) + " has won!", Controller.FontController.GetFont("bigFont"));
			lbl.Position.X = ((Controller.ScreenSize.X / Controller.CurrentDrawCamera.zoom) / 2) - (lbl.Width / 2);
			AddChild(lbl);

			Label lblAdvance = new Label("Press A to apply CPR", Controller.FontController.GetFont("bigFont"));
			lblAdvance.Position.X = ((Controller.ScreenSize.X / Controller.CurrentDrawCamera.zoom) / 2) - (lblAdvance.Width / 2);
			lblAdvance.Position.Y = 550;
			AddChild(lblAdvance);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			padState = GamePad.GetState(PlayerIndex.One);

			if (padState.Buttons.A == ButtonState.Pressed)
			{
				Controller.SwitchState(new MenuState());
			}
		}
    }
}

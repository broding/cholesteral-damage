using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Microsoft.Xna.Framework;
using Flakcore;

namespace GlamourJam.States
{
    class GameState : State
    {
        public Tilemap tilemap;
		public Vetbol3 player;

        public GameState()
        {
            TiledSprite bg = new TiledSprite(1500, 1000);
            bg.LoadTexture("background");
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
			this.AddChild(tilemap);

			WhiteBloodCell player2 = new WhiteBloodCell(new Vector2(50), PlayerIndex.Two);
			player2.LoadTexture(@"whiteBloodCell");
			player2.MaxVelocity = new Vector2(500);
            player2.JumpPower = 1000;
            AddChild(player2);


			WhiteBloodCell player3 = new WhiteBloodCell(new Vector2(50), PlayerIndex.Three);
            player3.LoadTexture(@"whiteBloodCell");
            player3.MaxVelocity = new Vector2(500);
            player3.JumpPower = 1000;
			AddChild(player3);

            WhiteBloodCell player4 = new WhiteBloodCell(new Vector2(50), PlayerIndex.Four);
            player4.LoadTexture(@"whiteBloodCell");
            player4.MaxVelocity = new Vector2(500);
            player4.JumpPower = 1000;
            AddChild(player4);

            CapturePoint capturepoint = new CapturePoint();
            AddChild(capturepoint);

			
			player = new Vetbol3();
			AddChild(player);
        }
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{

			base.Update(gameTime);
		}
    }
}

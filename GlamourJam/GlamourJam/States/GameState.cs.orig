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
		public Vetbol player;

        public GameState()
        {
            TiledSprite bg = new TiledSprite(1500, 1000);
            bg.LoadTexture("background");
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
            this.AddChild(tilemap);

<<<<<<< HEAD

			player = new Vetbol();
=======
            WhiteBloodCell player2 = new WhiteBloodCell(new Vector2(50), PlayerIndex.Two);
            player2.LoadTexture(@"whiteBloodCell");
			Vetbol player = new Vetbol();
>>>>>>> 5ddef02eb1342aa557538c10f8a9238eda88c0e5
			AddChild(player);
            AddChild(player2);
        }
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{

			base.Update(gameTime);
		}
    }
}

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
        private FatBomb fatBomb;
        private Array capturepointarray;

		public Vetbol player;
        public Vetbol player2;
        public Vetbol player3;
        public Vetbol player4;
        private List<Tile> playerRespawn;
        private Random rnd = new Random();

        public GameState()
        {
            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
			this.AddChild(tilemap);


            fatBomb = new FatBomb(new Vector2(100), player);
            AddChild(fatBomb);

            playerRespawn = tilemap.RemoveTiles(7);

            List<Tile> removeTiles = tilemap.RemoveTiles(3);
            foreach (Tile tile in removeTiles)
            {
                CapturePoint capturepoint = new CapturePoint();
                capturepoint.Position = tile.Position + ( new Vector2(-27, -62));
                AddChild(capturepoint);
            }

            player = new Vetbol(PlayerIndex.One);
            player.Position = playerRespawn[rnd.Next(playerRespawn.Count)].Position;
            player2 = new Vetbol(PlayerIndex.Two);
            player2.Position = playerRespawn[rnd.Next(playerRespawn.Count)].Position;
			AddChild(player);
            AddChild(player2);
        }
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
		}
    }
}

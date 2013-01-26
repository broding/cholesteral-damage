using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Utils;

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
        private List<Tile> playerSpawn;
        private Random rnd = new Random();

        public Pool<FatBomb> BombPool;

        public GameState()
        {
            Vetbol.state = this;
            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
			this.AddChild(tilemap);

            playerSpawn = tilemap.RemoveTiles(7);
            int playerRespawn = rnd.Next(playerSpawn.Count);
            
            List<Tile> removeTiles = tilemap.RemoveTiles(3);
            foreach (Tile tile in removeTiles)
            {
                CapturePoint capturepoint = new CapturePoint();
                capturepoint.Position = tile.Position + ( new Vector2(-27, -62));
                AddChild(capturepoint);
            }

            player = new Vetbol(PlayerIndex.One);
            player.Position = playerSpawn[playerRespawn].Position;
            player2 = new Vetbol(PlayerIndex.Two);
            player2.Position = playerSpawn[playerRespawn+1].Position;
			AddChild(player);
            AddChild(player2);

            this.BombPool = new Pool<FatBomb>(50, false, FatBomb.IsValid, this.NewBomb);

            this.SpawnBomb(new Vector2(260, 260));
        }


        private FatBomb NewBomb()
        {
            FatBomb bomb = new FatBomb();
            bomb.Deactivate();
            this.AddChild(bomb);

            return bomb;
        }

        public void SpawnBomb(Vector2 position)
        {
            FatBomb bomb = this.BombPool.New();
            bomb.Position = position;
            bomb.Activate();
        }

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
		}
    }
}

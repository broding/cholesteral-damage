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
        private List<Tile> playerSpawn;
        private List<Tile> NotUsedSpawnPoints;
        private List<Vetbol> players;
        private Random rnd = new Random();

        public Pool<FatBomb> BombPool;

        public GameState()
        {
            Controller.LayerController.AddLayer("bombLayer");
            FatBomb.state = this;
            Vetbol.state = this;
            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
			this.AddChild(tilemap);

            this.players = new List<Vetbol>();

            playerSpawn = tilemap.RemoveTiles(7);
            int playerRespawn = rnd.Next(playerSpawn.Count);
            
            List<Tile> removeTiles = tilemap.RemoveTiles(3);
            foreach (Tile tile in removeTiles)
            {
                CapturePoint capturepoint = new CapturePoint();
                capturepoint.Position = tile.Position + ( new Vector2(-27, -62));
                AddChild(capturepoint);
            }
            
            NotUsedSpawnPoints = playerSpawn;


            for (int i = 0; i < Controller.Input.getPadStateList.Where(c => c.IsConnected).Count(); i++)
            {
                this.players.Add(new Vetbol((PlayerIndex)i));
            }

            for (int j = 0; j < players.Count; j++)
            {
                this.players[j].Position = this.getAvailablePosition();
                this.AddChild(this.players[j]);
            }

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

        public void ExplodeBomb(FatBomb bomb)
        {
            int width = 136;
            int height = 136;

            BoundingRectangle rect = new BoundingRectangle((int)bomb.Position.X - width / 2, (int)bomb.Position.Y - height / 2, width, height);

            foreach (Vetbol player in this.players)
            {
                if (player.GetBoundingBox().Intersects(rect))
                {
                    player.Deactivate();
                }
            }
        }

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
		}

        private Vector2 getAvailablePosition()
        {
            Random number = new Random();
            int listNumber = number.Next(NotUsedSpawnPoints.Count);
            Vector2 position = NotUsedSpawnPoints[listNumber].Position;

            NotUsedSpawnPoints.RemoveAt(listNumber);

            return position;
        }
    }
}

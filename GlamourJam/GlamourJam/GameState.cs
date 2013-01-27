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

namespace GlamourJam.States
{
    class GameState : State
    {
        public Tilemap tilemap;
        private FatBomb fatBomb;
        private List<Tile> playerSpawn;
        private List<Tile> NotUsedSpawnPoints;
        private List<Vetbol> players;
		private Random rnd = new Random();
		private SoundEffect soundEffectBomb;
        private HUD hud;

		public Pool<FatBomb> BombPool;
		List<CapturePoint> capturePoints = new List<CapturePoint>();
		private float updateScoreTime = 5000;
		Vetbol lastPlayerAlive;

        public GameState()
        {
            Controller.LayerController.AddLayer("bombLayer");
            FatBomb.state = this;
            Vetbol.state = this;
            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            bg.Depth = 0f;
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
			this.AddChild(tilemap);

            this.players = new List<Vetbol>();

            playerSpawn = tilemap.RemoveTiles(7);
            int playerRespawn = rnd.Next(playerSpawn.Count);

			List<Tile> capturePointTiles = tilemap.RemoveTiles(3);
			foreach (Tile tile in capturePointTiles)
            {
                CapturePoint capturepoint = new CapturePoint();
                capturepoint.Position = tile.Position + ( new Vector2(-27, -61));
				capturePoints.Add(capturepoint);
                AddChild(capturepoint);
            }

            NotUsedSpawnPoints = new List<Tile>();
            NotUsedSpawnPoints.AddRange(playerSpawn);


            for (int i = 0; i < Controller.Input.getPadStateList.Where(c => c.IsConnected).Count(); i++)
            {
                this.players.Add(new Vetbol((PlayerIndex)i));
            }

            for (int j = 0; j < players.Count; j++)
            {
				this.players[j].score = 100;
                this.players[j].Position = this.getAvailablePosition();
                this.AddChild(this.players[j]);
            }

            this.BombPool = new Pool<FatBomb>(50, false, FatBomb.IsValid, this.NewBomb);

			soundEffectBomb = Controller.Content.Load<SoundEffect>("sounds/explode");

            this.hud = new HUD(this.players);
            this.AddChild(hud);
        }


        private FatBomb NewBomb()
        {
            FatBomb bomb = new FatBomb();
            bomb.Deactivate();
            this.AddChild(bomb);

            return bomb;
        }

        public void SpawnBomb(Vetbol vetbol, Vector2 direction)
        {
            FatBomb bomb = this.BombPool.New();
            direction.Y *= -1;
            bomb.Velocity = direction * 1200;
            bomb.Position = vetbol.Position;
            bomb.Activate(vetbol);
        }

        public void ExplodeBomb(FatBomb bomb)
        {
			soundEffectBomb.Play(0.5f, 0, 0);

            int width = 136;
            int height = 136;

            BoundingRectangle rect = new BoundingRectangle((int)bomb.Position.X - width / 2, (int)bomb.Position.Y - height / 2, width, height);

            if (NotUsedSpawnPoints.Count == 0)
            {
                NotUsedSpawnPoints = new List<Tile>();
                NotUsedSpawnPoints.AddRange(playerSpawn);
            }

            foreach (Vetbol player in this.players)
            {
                if (player.GetBoundingBox().Intersects(rect) && !player.IsFlickering)
                {
                    Controller.Input.SetVibrationWithTimer(player.index, TimeSpan.FromMilliseconds(300));
                    player.Deactivate();
                    this.RespawnPlayer(player);
                }
            }
        }

		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			updateScoreTime -= gameTime.ElapsedGameTime.Milliseconds;
			if (updateScoreTime <= 0)
			{
				updateScoreTime = 5000;
				int playersAlive = 0;
				foreach (Vetbol player in this.players)
				{
					int pointsOwned = 0;
					foreach (CapturePoint point in capturePoints)
					{
						if (point.owner == player)
							pointsOwned++;
					}
					player.score -= (capturePoints.Count - pointsOwned);
					if (player.score <= 0)
					{
						player.Deactivate();
						//TODO feedback of dead player in HUD
					} else
					{
						playersAlive++;
						lastPlayerAlive = player;
					}
					//TODO: update HUD score
					tilemap.beatRate -= 1;
					//TODO: adjust beatrate of the map
				}
				if (playersAlive == 1)
				{
					//TODO lastPlayerAlive = winner
				}
			}
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

        private void RespawnPlayer(Vetbol player)
        {
            player.Position = getAvailablePosition();
            player.Activate();
            player.IsFlickering = true;
        }
    }
}

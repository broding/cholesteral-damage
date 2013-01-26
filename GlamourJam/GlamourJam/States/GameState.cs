using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Display.ParticleEngine;

namespace GlamourJam.States
{
    class GameState : State
    {
        public Tilemap tilemap;
		public Vetbol player;

        public Vetbol player2;

        public Vetbol player3;

        public Vetbol player4;
        ParticleEngine particle;


        public GameState()
        {
            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);
            this.AddChild(bg);
			this.AddChild(tilemap);

            CapturePoint capturepoint = new CapturePoint();
            AddChild(capturepoint);

			player = new Vetbol(PlayerIndex.One);
            player2 = new Vetbol(PlayerIndex.Two);
			AddChild(player);
            AddChild(player2);

            particle = new ParticleEngine(Controller.Content.Load<ParticleEffect>("splashBottom"));

            this.AddChild(particle);
            particle.Position = new Vector2(400, 400);

            YellowBlur blur = new YellowBlur();
            this.AddChild(blur);

            this.ConvertBloodTiles();
        }
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
            if(Controller.Input.JustPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Keys.Space))
            {
                particle.Start(300);
            }
			base.Update(gameTime);
		}

        public void ConvertBloodTiles()
        {
            List<Tile> tiles = this.tilemap.RemoveTiles(7);

            foreach (Tile tile in tiles)
            {
                ParticleEngine engine = new ParticleEngine(Controller.Content.Load<ParticleEffect>("bloodParticle"));
                engine.Position = tile.Position + new Vector2(0, 16);
                this.AddChild(engine);
            }
        }
    }
}

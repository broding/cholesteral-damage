using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Utils;
using Flakcore.Display.ParticleEngine;
using GlamourJam.States;
using Microsoft.Xna.Framework.Graphics;

namespace GlamourJam
{
    class FatBomb : Sprite, IPoolable
    {
        public static GameState state;
        private const int bombTime = 4;
        private TimeSpan timeSpan = TimeSpan.FromSeconds(bombTime);
        public int PoolIndex { get; set; }
        public Action<int> ReportDeadToPool { get; set; }

        private float ColorTimer;
        private float ColorTime;

        private ParticleEngine BoomParticles;

        public FatBomb()
        {
            this.BoomParticles = new ParticleEngine(Controller.Content.Load<ParticleEffect>("splashBottom"));
            this.AddCollisionGroup("bomb");
            this.LoadTexture(Controller.Content.Load<Texture2D>(@"Assets/Bomb"), 48, 48);
            this.AddAnimation("IDLE", new int[1] {0}, 0);
            this.AddAnimation("CLOSETOEXPLODE", new int[4] { 0, 1, 0, 2 }, 0.3f);
            this.AddAnimation("EXPLODE", new int[6] { 0, 1, 2, 3, 4, 5 }, 0.17f);
            this.gravity = 0;
            Controller.LayerController.GetLayer("bombLayer").AddChild(this.BoomParticles);
            this.Scale *= 1.5f;
        }

        public float gravity { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!this.Active)
                return;

            base.Update(gameTime);

            Controller.Collide(this, "tilemap", this.TilemapCollision);

            timeSpan -= gameTime.ElapsedGameTime;

            if (timeSpan.TotalMilliseconds <= 0)
            {
                Explode();
            }

            this.Velocity.Y += gravity;

            this.ColorTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this.ColorTimer > this.ColorTime)
            {
                this.SwitchColor();
                this.ColorTimer = 0;
            }

            if (timeSpan.TotalMilliseconds > 3000)
            {
                this.ColorTime = 800;
                this.PlayAnimation("IDLE");
            }
            else if (timeSpan.TotalMilliseconds < 3000 && timeSpan.TotalMilliseconds > 1000)
            {
                this.ColorTime = 200;
                this.PlayAnimation("CLOSETOEXPLODE");
            }
            else
            {
                this.ColorTime = 50;
                this.PlayAnimation("EXPLODE");
            }
        }

        private void TilemapCollision(Node bomb, Node tilemap)
        {
            this.Velocity = Vector2.Zero;
        }

        public void Explode()
        {
            this.BoomParticles.Position = this.Position + new Vector2(16, 16);
            this.BoomParticles.Explode();
            this.Deactivate();
            state.ExplodeBomb(this);
        }

        public override void Activate()
        {
            base.Activate();
            timeSpan = TimeSpan.FromSeconds(bombTime);
        }

        private void SwitchColor()
        {
            if (this.Color == Color.IndianRed)
                this.Color = Color.White;
            else
                this.Color = Color.IndianRed;
        }

        public static bool IsValid(FatBomb bomb)
        {
            return !bomb.Active;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Utils;
using Flakcore.Display.ParticleEngine;

namespace GlamourJam
{
    class FatBomb : Sprite, IPoolable
    {
        private TimeSpan timeSpan = TimeSpan.FromSeconds(6);
        public int PoolIndex { get; set; }
        public Action<int> ReportDeadToPool { get; set; }

        private float ColorTimer;
        private float ColorTime;

        private ParticleEngine BoomParticles;

        public FatBomb()
        {
            this.BoomParticles = new ParticleEngine(Controller.Content.Load<ParticleEffect>("splashBottom"));
            this.AddCollisionGroup("bomb");
            this.LoadTexture(@"whiteBloodCell");
            this.gravity = 5;
            Controller.LayerController.GetLayer("bombLayer").AddChild(this.BoomParticles);
        }

        public float gravity { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!this.Active)
                return;

            base.Update(gameTime);

            Controller.Collide(this, "tilemap");

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
                this.ColorTime = 800;
            else if (timeSpan.TotalMilliseconds < 3000 && timeSpan.TotalMilliseconds > 1000)
                this.ColorTime = 200;
            else
                this.ColorTime = 50;
        }

        public void Explode()
        {
            this.BoomParticles.Position = this.Position + new Vector2(16, 16);
            this.BoomParticles.Explode();
            this.Deactivate();
        }

        public override void Activate()
        {
            base.Activate();
            timeSpan = TimeSpan.FromSeconds(1);
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

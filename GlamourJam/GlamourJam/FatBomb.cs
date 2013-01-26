using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Utils;

namespace GlamourJam
{
    class FatBomb : Sprite, IPoolable
    {
        private TimeSpan timeSpan = TimeSpan.FromSeconds(2);
        public int PoolIndex { get; set; }
        public Action<int> ReportDeadToPool { get; set; }

        public FatBomb()
        {
            this.AddCollisionGroup("bomb");
            this.LoadTexture(@"whiteBloodCell");
            this.gravity = 5;
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
        }

        public void Explode()
        {
            this.Deactivate();
        }

        public override void Activate()
        {
            base.Activate();
            timeSpan = TimeSpan.FromSeconds(2);
        }



        public static bool IsValid(FatBomb bomb)
        {
            return !bomb.Active;
        }
    }
}

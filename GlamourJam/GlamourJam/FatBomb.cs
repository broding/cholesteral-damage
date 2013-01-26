using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore;

namespace GlamourJam
{
    class FatBomb : Sprite
    {
        private TimeSpan timeSpan = TimeSpan.FromSeconds(2);

        public FatBomb(Vector2 position, Vetbol bol)
        {
            this.AddCollisionGroup("bomb");
            this.LoadTexture(@"whiteBloodCell");
            this.Position = position;
            if(bol != null)
                this.Color = bol.image.Color;
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

    }
}

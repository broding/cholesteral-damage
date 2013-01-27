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
    class FatBomb : Node, IPoolable
    {
        public static GameState state;
        private const int bombTime = 2;
        private TimeSpan timeSpan = TimeSpan.FromSeconds(bombTime);
        private bool stuckToAWall = false;
        public int PoolIndex { get; set; }
        public Action<int> ReportDeadToPool { get; set; }

        private float ColorTimer;
        private float ColorTime;

        private Sprite Sprite;

        private ParticleEngine BoomParticles;

        private Vetbol Vetbol;

        public FatBomb()
        {
            this.Collidable = true;
            this.BoomParticles = new ParticleEngine(Controller.Content.Load<ParticleEffect>("splashBottom"));
            this.AddCollisionGroup("bomb");

            this.Sprite = new Sprite();
            this.Sprite.LoadTexture(Controller.Content.Load<Texture2D>(@"Assets/Bomb"), 48, 48);
            this.Sprite.Position = new Vector2(-32, -32);
            this.Sprite.AddAnimation("IDLE", new int[1] { 0 }, 0);
            this.Sprite.AddAnimation("CLOSETOEXPLODE", new int[4] { 0, 1, 0, 2 }, 0.3f);
            this.Sprite.AddAnimation("EXPLODE", new int[6] { 0, 1, 2, 3, 4, 5 }, 0.17f);
            this.AddChild(this.Sprite);
            this.gravity = 5;
            Controller.LayerController.GetLayer("bombLayer").AddChild(this.BoomParticles);
            this.Sprite.Scale *= 1.5f;

            this.Width = 16;
            this.Height = 16;
        }

        public float gravity { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!this.Active)
                return;

            base.Update(gameTime);

            Controller.Collide(this, "tilemap", this.TilemapCollision);
            Controller.Collide(this, "player", null, this.PlayerCollision);

            timeSpan -= gameTime.ElapsedGameTime;

            if (timeSpan.TotalMilliseconds <= 0)
            {
                Explode();
            }

            if(!stuckToAWall)
            {
                this.Velocity.Y += gravity;
            }

            this.ColorTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this.ColorTimer > this.ColorTime)
            {
                this.SwitchColor();
                this.ColorTimer = 0;
            }

            if (timeSpan.TotalMilliseconds > 3000)
            {
                this.ColorTime = 800;
                this.Sprite.PlayAnimation("IDLE");
            }
            else if (timeSpan.TotalMilliseconds < 3000 && timeSpan.TotalMilliseconds > 1000)
            {
                this.ColorTime = 200;
                this.Sprite.PlayAnimation("CLOSETOEXPLODE");
            }
            else
            {
                this.ColorTime = 50;
                this.Sprite.PlayAnimation("EXPLODE");
            }
        }

        private void TilemapCollision(Node bomb, Node tilemap)
        {
            this.Velocity = Vector2.Zero;
            stuckToAWall = true;
        }

        private bool PlayerCollision(Node bomb, Node player)
        {
            if (this.Vetbol != player && !this.stuckToAWall)
            {
                if(!(player as Vetbol).stunned)
                    (player as Vetbol).Stun(2500);
            }

            return false;
        }

        public void Explode()
        {
            this.BoomParticles.Position = this.Position + new Vector2(16, 16);
            this.BoomParticles.Explode();
            this.Deactivate();
            this.stuckToAWall = false;
            state.ExplodeBomb(this);
        }

        public override void Activate()
        {
            base.Activate();
            timeSpan = TimeSpan.FromSeconds(bombTime);
        }

        private void SwitchColor()
        {
            if (this.Sprite.Color == Color.MediumVioletRed)
                this.Sprite.Color = Color.White;
            else
                this.Sprite.Color = Color.MediumVioletRed;
        }

        public static bool IsValid(FatBomb bomb)
        {
            return !bomb.Active;
        }

        internal void Activate(Vetbol vetbol)
        {
            this.Vetbol = vetbol;

            this.Activate();
        }
    }
}

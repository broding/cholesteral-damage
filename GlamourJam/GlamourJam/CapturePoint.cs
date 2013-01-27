using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore;
using Flakcore.Display.ParticleEngine;
namespace GlamourJam
{
    class CapturePoint:Sprite
    {
        public bool captured = false;
        public bool isPlayerCapturing = false;
        public double timer = 0;
        public bool contested = false;
        public Vetbol owner;
        private Vetbol playerCapturing;
        public bool isCollidingPlayer = false;

        private Texture2D GlowTexture;

        private ParticleEngine BloodParticles;


        public CapturePoint()
        {
            Immovable = true;
            this.AddCollisionGroup("capturePoint");
            this.Collidable = true;
            LoadTexture(Controller.Content.Load<Texture2D>("images/CapturePoint"), 128, 96);
            AddAnimation("uncaptured", new int[2] { 0 ,0 }, 0);
            AddAnimation("captured", new int[2] { 1, 1 }, 0);

            this.BloodParticles = new ParticleEngine(Controller.Content.Load<ParticleEffect>("bloodParticle"));
            Controller.LayerController.GetLayer("bombLayer").AddChild(this.BloodParticles);
            this.BloodParticles.Position = this.Position;
            this.BloodParticles.Start();

            this.GlowTexture = Controller.Content.Load<Texture2D>("glow");

            AddAnimation("p1uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p1captured", new int[3] { 1, 2, 3 },0.5f);
            AddAnimation("p2uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p2captured", new int[3] { 4, 5, 6 }, 0.5f);
            AddAnimation("p3uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p3captured", new int[3] { 7, 8, 9 }, 0.5f);
            AddAnimation("p4uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p4captured", new int[3] { 10, 11, 12 }, 0.5f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.BloodParticles.Position = this.Position + new Vector2(55, 53);

            if (!this.isCollidingPlayer)
            {
                timer = 0;
                this.playerCapturing = null;
                this.isPlayerCapturing = false;
            }
            if (isPlayerCapturing == true && playerCapturing != owner)
                timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= 2)
            {
                timer = 0;
                captured = true;
                owner = playerCapturing;

                if (owner.index == PlayerIndex.One) 
                {
                    PlayAnimation("p1captured");
                }

                if (owner.index == PlayerIndex.Two)
                {
                    PlayAnimation("p2captured");
                }
                if (owner.index == PlayerIndex.Three)
                {
                    PlayAnimation("p3captured");
                }
                if (owner.index == PlayerIndex.Four)
                {
                    PlayAnimation("p4captured");
                }  

            }

            this.isCollidingPlayer = false;
        }

        public void startCapturing(Vetbol vetblob)
        {
            isPlayerCapturing = true;
            playerCapturing = vetblob;
        }

        public override void DrawCall(SpriteBatch spriteBatch, WorldProperties worldProperties)
        {
            base.DrawCall(spriteBatch, worldProperties);

            this.DrawColor(spriteBatch);
        }


        internal void DrawColor(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (this.owner != null)
                color = this.owner.image.Color;

            spriteBatch.Draw(this.GlowTexture,
                    this.Position - new Vector2(50, 50),
                    new Rectangle(0, 0, 900, 900),
                    color * 0.2f,
                    this.Rotation,
                    new Vector2(300, 300),
                    this.Scale,
                    this.SpriteEffects,
                    0.001f);
        }
    }
    
	
}

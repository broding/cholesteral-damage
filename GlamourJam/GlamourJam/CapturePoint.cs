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
        private Vetbol owner;
        private Vetbol playerCapturing;
        public bool isCollidingPlayer = false;

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
                    PlayAnimation("captured");
                }

                if (owner.index == PlayerIndex.Two)
                {
                    PlayAnimation("uncaptured");
                }   

            }

            this.isCollidingPlayer = false;
        }

        public void startCapturing(Vetbol vetblob)
        {
            isPlayerCapturing = true;
            playerCapturing = vetblob;
        }

    }
    
	
}

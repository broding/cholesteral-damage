using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore;
using Flakcore.Display.ParticleEngine;
using Microsoft.Xna.Framework.Audio;
namespace GlamourJam
{
    class CapturePoint:Sprite
    {
        public static State state;

        public bool captured = false;
        public bool isPlayerCapturing = false;
        public double timer = 0;
        public bool contested = false;
        public Vetbol owner;
        private Vetbol playerCapturing;
        public bool isCollidingPlayer = false;

        private Texture2D GlowTexture;
        private Sprite takingOverRectangle;
        private ParticleEngine BloodParticles;
		SoundEffect soundTakeOver;
		SoundEffect soundTaken;

        public CapturePoint()
        {
            Immovable = true;
            this.AddCollisionGroup("capturePoint");
            this.Collidable = true;
            LoadTexture(Controller.Content.Load<Texture2D>("images/CapturePoint"), 128, 96);
            AddAnimation("uncaptured", new int[2] { 0 ,0 }, 0);
            AddAnimation("captured", new int[2] { 1, 1 }, 0);

            this.BloodParticles = new ParticleEngine(Controller.Content.Load<ParticleEffect>("bloodParticle"));
            state.AddChild(this.BloodParticles);
            this.BloodParticles.Position = this.Position;
            this.BloodParticles.Start();

            this.GlowTexture = Controller.Content.Load<Texture2D>("glow");
            CreateTakingOverRectangle();
            AddAnimation("p1uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p1captured", new int[3] { 1, 2, 3 },0.5f);
            AddAnimation("p2uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p2captured", new int[3] { 4, 5, 6 }, 0.5f);
            AddAnimation("p3uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p3captured", new int[3] { 7, 8, 9 }, 0.5f);
            AddAnimation("p4uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p4captured", new int[3] { 10, 11, 12 }, 0.5f);

			soundTakeOver = Controller.Content.Load<SoundEffect>("sounds/takeOver");
			soundTaken = Controller.Content.Load<SoundEffect>("sounds/winCapture");
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
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                takingOverRectangle.Scale.X = (float)(1-(timer / 2));
                takingOverRectangle.Visable = true;
            }
            else
            {
                takingOverRectangle.Visable = false;
            }

            if (timer >= 2)
            {
                takingOverRectangle.Visable = false;
                timer = 0;
                captured = true;
                owner = playerCapturing;
				soundTaken.Play(0.5f, 0, 0);

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
			if (!isPlayerCapturing)
				soundTakeOver.Play();

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
            float alpha = 0.1f;
            if (this.owner != null)
            {
                color = this.owner.image.Color;
                alpha = 0.3f;
            }

            spriteBatch.Draw(this.GlowTexture,
                    this.Position - new Vector2(50, 50),
                    new Rectangle(0, 0, 900, 900),
                    color * alpha,
                    this.Rotation,
                    new Vector2(300, 300),
                    this.Scale,
                    this.SpriteEffects,
                    0.001f);
        }
        private void CreateTakingOverRectangle()
        {
            takingOverRectangle = Sprite.CreateRectangle(new Vector2(48, 10), Color.GreenYellow);
            takingOverRectangle.Origin = new Vector2(24f,24f);
            takingOverRectangle.Position = new Vector2((this.Width / 2), 125);
            takingOverRectangle.Visable = false;
            AddChild(takingOverRectangle);
            takingOverRectangle.Depth = 0.9f;
        }
    }
    
	
}

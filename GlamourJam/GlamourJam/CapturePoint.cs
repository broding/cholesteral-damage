using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore;
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


        public CapturePoint()
        {
            Immovable = true;
            this.AddCollisionGroup("capturePoint");
            this.Collidable = true;
            LoadTexture(Controller.Content.Load<Texture2D>("images/CapturePoint"), 128, 96);
            AddAnimation("p1uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p1captured", new int[3] { 1, 2, 3 }, 0);
            AddAnimation("p2uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p2captured", new int[3] { 1, 2, 3 }, 0);
            AddAnimation("p3uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p3captured", new int[3] { 1, 2, 3 }, 0);
            AddAnimation("p4uncaptured", new int[1] { 0 }, 0);
            AddAnimation("p4captured", new int[3] { 1, 2, 3 }, 0);
       
           
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

    }
    
	
}

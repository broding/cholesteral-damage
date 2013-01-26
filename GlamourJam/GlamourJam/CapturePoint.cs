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


        public CapturePoint()
        {
            Immovable = true;
            this.AddCollisionGroup("capturePoint");
            this.Collidable = true;
            this.LoadTexture("images/pannenkoek");
            Position = new Vector2(150, 150);
           
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isPlayerCapturing == true && playerCapturing != owner)
                timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= 5)
            {
                timer = 0;
                captured = true;
                playerCapturing = owner;

                if (owner.index == PlayerIndex.One) 
                {
                    LoadTexture("images/kikker");
                }

                if (owner.index == PlayerIndex.Two)
                {
                    LoadTexture("whiteBloodCell");
                }   
                if (owner.index == PlayerIndex.Two)
                {
                    LoadTexture("whiteBloodCell");
                }

            }
        }

        public void startCapturing(Vetbol vetblob)
        {
            isPlayerCapturing = true;
            playerCapturing = vetblob;
        }

    }
    
	
}

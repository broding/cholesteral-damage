using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Flakcore;

namespace GlamourJam
{
	class Vetbol:Sprite
	{
		public bool isSticking = false;
		public PlayerIndex player = PlayerIndex.One;
		public bool onfloor = false;
		public GamePadState padState;
		public GamePadState prevPadState;
		public int maxSpeed = 500;
        public int jumpSpeed = 1000;
        public float speedX = 0;
        public float speedY = 0;
		public Vector2 jumpDirection = new Vector2(0, -1);
        public string CollisionState = "idle";

		public Vetbol()
		{
			Position = new Vector2(100, 100);
			LoadTexture("images/kikker");
		}

		protected override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, WorldProperties worldProperties)
		{
			base.Draw(spriteBatch, worldProperties);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
            Controller.Collide(this, "tilemap", Collision);
			padState = GamePad.GetState(player);
			//Move when sticking
            speedY += 15;
            if(CollisionState == "idle"){
            //Move Horizontally
			if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
			{
                speedX = padState.ThumbSticks.Left.X * maxSpeed;
			} else
            {
                if (onfloor)
                {
                    speedX *= 0.90f;
                }
                else
                {
                    speedX *= 0.99f;
                }
			}

                if (onfloor)
                {
                    if(speedY>=5){
                        //Jump
                        if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                        {
                            //Jump();
                            onfloor = false;
                            speedY = -jumpSpeed;
                        }
                    }
                }
            }
            if (CollisionState=="bottom")
            {
               /* speedY = 0;
                //Move Horizontally
                if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
                {
                    speedX = padState.ThumbSticks.Left.X * maxSpeed;
                }
                else
                {
                    speedX = 0;
                }
                
              
                if (!onfloor)
                {
                   // CollisionState = "idle";
                }
                else
                {
                    speedY = 0;
                }
                //Jump
                if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                {
                    //Jump();
                    speedY = -jumpSpeed;
                    CollisionState = "idle";
                }*/
            }
            
            if(CollisionState == "left"){
                //Move Horizontally
               // if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
               // {
             //       speedX = padState.ThumbSticks.Left.X * maxSpeed;
             //   }
                //    //   else
             //   {
                    //speedX = 0;
                //    }
                speedY *= 0.9f;
                //Jump
                if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                {
                    //Jump();
                    speedX += jumpSpeed;
                    speedY = -jumpSpeed;
                    CollisionState = "idle";
                }
                if ((padState.ThumbSticks.Left.X > 0))
                {
                    CollisionState = "idle";
                }
            }
             if(CollisionState == "right"){
                 //Move Horizontally
              //   if (!(padState.ThumbSticks.Left.X == 0) && !isSticking)
              //   {
              //       speedX = padState.ThumbSticks.Left.X * maxSpeed;
              //   }
                 //   else
              //   {
                     //speedX = 0;
                 //   }
                 speedY *= 0.9f;
                 //Jump
                 if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed)
                 {
                     //Jump();
                     speedX -= jumpSpeed;
                     speedY = -jumpSpeed;
                     CollisionState = "idle";
                 }
                 if((padState.ThumbSticks.Left.X < 0)){
                     CollisionState = "idle";
                 }
            }
            if(CollisionState == "top"){
                 speedY += 5;
            }
            if (isSticking)
                /*{
                    speedX = padState.ThumbSticks.Left.X * (maxSpeed / 2);
                    speedY = -padState.ThumbSticks.Left.Y * (maxSpeed / 2);
                } else
                //Gravity
                {
                    speedY += 5;
                }*/

                speedY *= 0.95f;
            Velocity.X = speedX;
            Velocity.Y = speedY;

            System.Diagnostics.Debug.WriteLine("[VetBol]State:["+CollisionState+"]V.Y:[" + Velocity.Y + "]V.X[" + Velocity.X + "]");
			//Jump
			//if (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A != ButtonState.Pressed && onfloor)
			//{
			//	onfloor = false;
				//Jump();
			//	Velocity.Y = -jumpSpeed;
			//}

			isSticking = false;
			prevPadState = padState;
		}
        public void NotTouching(Node player, Node collidingTile)
        {
            System.Diagnostics.Debug.WriteLine("[VetBol]State:Not touching");
        }
		public void Collision(Node player, Node collidingTile)
        {
            /*jumpDirection = Vector2.Zero;

            if (player.Touching.Bottom)
            {
                jumpDirection.Y = -1;
            }
            if (player.Touching.Left)
            {
                jumpDirection.X = 1;
            }
            if (player.Touching.Right)
            {
                jumpDirection.X = -1;
            }
            if (player.Touching.Top)
            {
                jumpDirection.Y = 1;
            }*/
            if (player.Touching.Bottom)
            {
                onfloor = true;
                speedY = 0;
               // CollisionState = "bottom";
            }
            
            if (player.Touching.Left){
                CollisionState = "left";
            }
            if (player.Touching.Right)
            {
                CollisionState = "right";
            }
          /*  if (player.Touching.Top)
            {
                CollisionState = "top";
            }*/

            System.Diagnostics.Debug.WriteLine("[VetBol]CollisionState:" + CollisionState);
            //if (player.Touching.Bottom || player.Touching.Left || player.Touching.Right)
            //{
            //	onfloor = true;
            //}
            //if (player.Touching.Left || player.Touching.Right || player.Touching.Top)
            //{
            //	isSticking = true;
            //	onfloor = true;
            //}
        }

		public void Jump()
		{
			int tempJumpSpeed = jumpSpeed;
			if (jumpDirection.X != 0 && jumpDirection.Y != 0)
			{
				tempJumpSpeed /= 2;
			}
			Velocity = jumpDirection * tempJumpSpeed;
		}
	}
}

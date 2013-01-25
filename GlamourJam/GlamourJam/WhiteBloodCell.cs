using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Microsoft.Xna.Framework;
using Flakcore;
using Microsoft.Xna.Framework.Graphics;

namespace GlamourJam
{
    class WhiteBloodCell : Sprite
    {

        private PlayerIndex player;
        private MovingDirections movingDirection = MovingDirections.None;
        private MovingDirections movingDirection2 = MovingDirections.None;

        public enum MovingDirections
        {
            Left,

            Right,

            Up,

            Down,

            None
        }

        public WhiteBloodCell(Vector2 startPosition, PlayerIndex player)
        {
            this.MaxVelocity = new Vector2(100);
            this.JumpPower = 2;
            this.Position = startPosition;
            this.player = player;
        }

        public float JumpPower { get; set; }

        public bool OnFloor { get; private set; }

        public void Jump()
        {
            this.Velocity.Y = -JumpPower;
        }

        public void Move(float velocity, MovingDirections direction)
        {
            switch(direction)
            {
                case MovingDirections.Left:
                case MovingDirections.Right:
                    this.Velocity.X = direction == MovingDirections.Left ? -velocity : velocity;
                    break;
                case MovingDirections.Up:
                case MovingDirections.Down:
                    this.Velocity.Y = direction == MovingDirections.Up ? -velocity : velocity;
                    break;
            }
        }

        public void Move(Vector2 velocity, MovingDirections direction, MovingDirections direction2)
        {
            velocity.X = Math.Abs(velocity.X);
            velocity.Y = Math.Abs(velocity.Y);

            if (direction != direction2)
            {
                switch (direction)
                {
                    case MovingDirections.Left:
                    case MovingDirections.Right:
                        if (direction2 != MovingDirections.Left && direction2 != MovingDirections.Right)
                        {
                            this.Velocity.X = direction == MovingDirections.Left ? -velocity.X : velocity.X;
                            this.Velocity.Y = direction2 == MovingDirections.Up ? -velocity.Y : velocity.Y;
                        }
                        break;
                    case MovingDirections.Up:
                    case MovingDirections.Down:
                        if (direction2 != MovingDirections.Down && direction2 != MovingDirections.Up)
                        {
                            this.Velocity.X = direction2 == MovingDirections.Left ? -velocity.X : velocity.X;
                            this.Velocity.Y = direction == MovingDirections.Up ? -velocity.Y : velocity.Y;
                        }
                        break;
                }
            }
        }

        public void AddGravitation(float gravitation)
        {
            this.Velocity.Y += gravitation;
        }

        public void AddBloodFlow(Vector2 bloodFlowVelocity)
        {
            this.Velocity += bloodFlowVelocity;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
            base.Update(gameTime);

            float leftThumbX = Controller.Input.GetPadState(this.player).ThumbSticks.Left.X;
            float leftThumbY = Controller.Input.GetPadState(this.player).ThumbSticks.Left.Y;

            Vector2 currentVelocity = new Vector2(0);

            if (Controller.Input.JustPressed(this.player, Microsoft.Xna.Framework.Input.Buttons.A) && OnFloor)
            {
                this.OnFloor = false;
                Jump();
            }

            if (Math.Abs(leftThumbX) > 0.1f)
            {
                
                this.movingDirection = leftThumbX > 0 ? MovingDirections.Right : MovingDirections.Left;

                currentVelocity.X = MathHelper.Lerp(0, MaxVelocity.X, Math.Abs(leftThumbX));

                this.Move(currentVelocity.X, this.movingDirection);
            }
            else
            {
                this.movingDirection = MovingDirections.None;
                this.movingDirection2 = MovingDirections.None;
                this.Velocity.X = 0;
            }

            AddGravitation(5);
            Controller.Collide(this, "tilemap", BloodCellCollides);
        }

        public void BloodCellCollides(Node node1, Node node2)
        {
            if (node1.CollidableSides.Bottom)
            {
                this.OnFloor = true;
                this.Velocity.Y = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Flakcore;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace GlamourJam
{
    class YellowBlur : Sprite
    {
        public YellowBlur()
        {
            this.LoadTexture(@"YellowBlur");//Controller.Content.Load<Texture2D>("YellowBlur"), (int)(Controller.ScreenSize.X / Controller.CurrentDrawCamera.zoom), (int)(Controller.ScreenSize.Y / Controller.CurrentDrawCamera.zoom));
            Debug.WriteLine(Controller.CurrentDrawCamera.zoom);
            this.AlphaState = AlphaStates.NotVisible;
            this.Scale = new Vector2(1 / Controller.CurrentDrawCamera.zoom, 1 / Controller.CurrentDrawCamera.zoom); ;
        }

        public enum AlphaStates
        {
            NotVisible = 0,

            FirstPointCaptured = 2,

            SecondPointCaptured = 4,

            ThirdPointCaptured = 6,

            FinalPointCaptured = 8
        }

        public AlphaStates AlphaState { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);


            this.Alpha =  ((float)AlphaState / 10f);
#if DEBUG
            if (Controller.Input.JustPressed(PlayerIndex.One, Keys.Space))
            {
                NextState();
            }
#endif
        }

        public void NextState()
        {
            if (AlphaState != AlphaStates.FinalPointCaptured)
            {
                AlphaState += 2;
            }
        }
    }
}

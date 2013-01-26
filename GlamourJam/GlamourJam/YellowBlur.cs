using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Flakcore;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GlamourJam
{
    class YellowBlur : Sprite
    {
        public YellowBlur()
        {
            this.LoadTexture(@"YellowBlur");
            this.AlphaState = AlphaStates.NotVisible;
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

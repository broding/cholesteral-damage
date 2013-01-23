using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace GlamourJam
{
    class TestState : State
    {
        public TestState()
        {
            Sprite sprite = new Sprite();
            sprite.LoadTexture("smile");
            sprite.Position.X = 50;
            sprite.Position.Y = 50;
            this.AddChild(sprite);
        }
    }
}

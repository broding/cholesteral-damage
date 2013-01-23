using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Flakcore;
using Microsoft.Xna.Framework.Graphics;

namespace GlamourJam
{
    class TestState : State
    {
        public TestState()
        {
            Layer layer = Controller.LayerController.AddLayer("tilemap");
            Layer smileLayer = Controller.LayerController.AddLayer("smile");
            Tilemap tilemap = new Tilemap();
            tilemap.LoadMap(@"Content/tilemap.tmx", 32, 32);
            layer.AddChild(tilemap);

            Sprite sprite = new Sprite();
            sprite.LoadTexture("smile");
            sprite.Position.X = 50;
            sprite.Position.Y = 50;
            smileLayer.AddChild(sprite);

            layer.PostEffectAction = this.TilemapEffect;
        }

        private void TilemapEffect(Layer layer)
        {
            Effect effect = Controller.Content.Load<Effect>("testEffect");

            effect.CurrentTechnique.Passes[0].Apply(); 
        }
    }
}

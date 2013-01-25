using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Microsoft.Xna.Framework;
using Flakcore;

namespace GlamourJam.States
{
    class GameState : State
    {
        public Tilemap tilemap;
        private Sprite iets;

        public GameState()
        {
            tilemap = new Tilemap();
            tilemap.LoadMap("Content/testmap.tmx", 32, 32);

            this.AddChild(tilemap);
        }
    }
}

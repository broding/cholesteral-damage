using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Flakcore.Display;
using Flakcore;
using Flakcore.Utils;

namespace Display.Tilemap
{
    public class Tile : Sprite
    {
        public int Gid { get; private set; }
        public Tileset Tileset { get; private set; }

        public Tile(int x, int y, int gid, Rectangle sourceRect, Tileset tileset, string[] collisionGroups)
        {
            this.Position = new Vector2(x*Tilemap.tileWidth, y*Tilemap.tileHeight);
            this.Gid = gid;
            this.SourceRectangle = sourceRect;
            this.Tileset = tileset;
            this.Immovable = true;
            this.Collidable = false;
            this.Width = Tilemap.tileWidth;
            this.Height = Tilemap.tileHeight;
            this.Texture = this.Tileset.Texture;

            for (int i = 0; collisionGroups.Length > i; i++)
                this.AddCollisionGroup(collisionGroups[i]);
        }

        public override BoundingRectangle GetBoundingBox()
        {
            return new BoundingRectangle(Position.X, Position.Y, Tilemap.tileWidth, Tilemap.tileHeight);
        }
    }
}

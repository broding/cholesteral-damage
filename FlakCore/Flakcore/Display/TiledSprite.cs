using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    class TiledSprite : Sprite
    {
        public int TiledWidth;
        public int TiledHeight;

        public TiledSprite(int width, int height)
        {
            this.TiledWidth = width;
            this.TiledHeight = height;
        }

        public TiledSprite()
        {
        }

        public override void LoadTexture(Texture2D texture, int width, int height)
        {
            base.LoadTexture(texture, width, height);

            this.Width = width * this.TiledWidth;
            this.Height = height * this.TiledHeight;
        }

        protected override void DrawCall(SpriteBatch spriteBatch, ParentNode parentNode)
        {
            int amountX = this.TiledWidth / this.Texture.Width;
            int amountY = this.TiledHeight / this.Texture.Height;

            ParentNode newParentNode = new ParentNode();
            newParentNode.Alpha = this.Alpha;

            for (int x = 0; x < amountX; x++)
            {
                for (int y = 0; y < amountY; y++)
                {
                    newParentNode.Position = parentNode.Position + new Vector2(x * this.Texture.Width * this.Scale.X, y * this.Texture.Height * this.Scale.Y);
                    base.DrawCall(spriteBatch, newParentNode); 
                }
            }
        }
    }
}

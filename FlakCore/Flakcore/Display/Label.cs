using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    public class Label : Sprite
    {
        public string Text;
        public SpriteFont SpriteFont;
        public HorizontalAlign HorizontalAlign;
        public VerticalAlign VerticalAlign;

        private Vector2 TextSize;

        public Label() : this("", Controller.Content.Load<SpriteFont>("DefaultFont"))
        { 
        }

        public Label(string text) : this(text, Controller.Content.Load<SpriteFont>("DefaultFont"))
        {
        }

        public Label(string text, SpriteFont spriteFont)
        {
            this.Text = text;
            this.SpriteFont = spriteFont;
            this.SpriteFont = Controller.Content.Load<SpriteFont>("DefaultFont");
            this.HorizontalAlign = HorizontalAlign.LEFT;
            this.VerticalAlign = VerticalAlign.TOP;

            this.TextSize = this.SpriteFont.MeasureString(text);
            this.Width = (int)this.TextSize.X;
            this.Height = (int)this.TextSize.Y;
        }


        protected override void DrawCall(SpriteBatch spriteBatch, ParentNode parentNode)
        {
            if (this.Text == "")
                return;

            this.TextSize = this.SpriteFont.MeasureString(this.Text);

            parentNode.Position.X *= this.ScrollFactor.X;
            parentNode.Position.Y *= this.ScrollFactor.Y;
            parentNode.Position.X += this.Width / 2 - this.TextSize.X / 2;
            parentNode.Position.Y += this.Height / 2 - this.TextSize.Y / 2;

            spriteBatch.DrawString(
                Controller.FontDefault,
                this.Text,
                parentNode.Position,
                this.Color * parentNode.Alpha,
                this.Rotation,
                this.Origin,
                this.Scale,
                this.SpriteEffects,
                Node.GetDrawDepth(this.GetParentDepth()));
        }
    }

    public enum HorizontalAlign
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public enum VerticalAlign
    {
        TOP,
        MIDDLE,
        BOTTOM
    }
}

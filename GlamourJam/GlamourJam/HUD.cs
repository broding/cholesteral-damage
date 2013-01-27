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
    class HUD
    {
        public HUD()
        {

            /*size.X = Controller.ScreenSize.X / playerList.Count;
            size.Y = 96;*/
        }
    };

    class PlayerHUDStatus : Node
    {
        private Vector2 size;
        private Sprite playerImage;

        public PlayerHUDStatus(Vector2 size, Vetbol player)
        {
            this.size = size;
            playerImage = new Sprite();
            if (player.index == PlayerIndex.One)
            {
                this.playerImage.LoadTexture(@"slimeblob");
            }
            else
            {
                this.playerImage.LoadTexture(@"slimeblobOther");
            }

            this.playerImage.Color = player.image.Color;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    };

    class CapturePointHUDStatus : Node
    {
        private Vector2 size;
        private Dictionary<CapturePoint, Texture2D> pointsCapturedBy = new Dictionary<CapturePoint, Texture2D>();

        public CapturePointHUDStatus(List<CapturePoint> capturePoints)
        {
            size.X = (Controller.ScreenSize.X / 4);
            size.Y = 128;
            for (int i = 0; i < capturePoints.Count; i++)
            {
                pointsCapturedBy.Add(capturePoints[i], Controller.Content.Load<Texture2D>(@"images/CapturePoint"));
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        public void UpdateCaptureHUDStatus()
        {

        }
    };
}

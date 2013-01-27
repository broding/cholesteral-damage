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
    class HUD : Node
    {
        PlayerHUDStatus playerHUD;
        Sprite background;

        public HUD(List<Vetbol> playerList)
        {
            background = new Sprite();
            background.LoadTexture(@"Assets/HudBG");
            this.AddChild(background);

            playerHUD = new PlayerHUDStatus(new Vector2(Controller.ScreenSize.X / playerList.Count, 96), new Vector2(0, Controller.ScreenSize.Y - 96), playerList[1], TimeSpan.FromSeconds(2));
            this.AddChild(playerHUD);
        }

        public void PlayerDied(Vetbol player)
        {
            playerHUD.Load();
        }

        public void PlayerSpawned(Vetbol player)
        {
            playerHUD.ToNormal();
        }
    };

    class PlayerHUDStatus : Node
    {
        private Vector2 size;
        private Sprite playerImage;
        private Sprite playerFrame;
        private Label playerTickets;

        public Vetbol player;

        public PlayerHUDStatus(Vector2 size, Vector2 position, Vetbol player, TimeSpan timer)
        {
            this.size = size;
            this.Position = position;
            this.player = player;

            playerImage = new Sprite();
            playerFrame = new Sprite();

            playerFrame.Position.X = this.Position.X + (size.X / 2) - (playerFrame.Width / 2);
            playerFrame.Position.Y = this.Position.Y;

            playerImage.Position.X = playerFrame.Position.X + (playerFrame.Width / 2) - (playerImage.Width / 2);
            playerImage.Position.Y = this.Position.Y;

            playerFrame.LoadTexture(Controller.Content.Load<Texture2D>(@"peter4croisants"), 96, 96);
            playerFrame.AddAnimation("IDLE", new int[1] { 0 }, 0);
            playerFrame.AddAnimation("LOADING", new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, (float)timer.TotalSeconds / 12);

            this.playerImage.LoadTexture(@"slimeblobOther");

            this.playerImage.Color = player.image.Color;

            this.playerTickets = new Label("test");
            this.playerTickets.Position = new Vector2(playerImage.Position.X, this.Position.Y + 90);

            this.AddChild(playerFrame);
            this.AddChild(playerImage);
            this.AddChild(playerTickets);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Load()
        {
            playerFrame.PlayAnimation("LOADING");
        }

        public void ToNormal()
        {
            playerFrame.PlayAnimation("IDLE");
        }
    };
}

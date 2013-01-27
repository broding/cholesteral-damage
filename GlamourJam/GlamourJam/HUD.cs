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
        Dictionary<PlayerIndex, PlayerHUDStatus> hudPlayers = new Dictionary<PlayerIndex, PlayerHUDStatus>();
        Sprite background;

        public HUD(List<Vetbol> playerList, TimeSpan timer)
        {
            background = new Sprite();
            background.LoadTexture(@"Assets/HudBG");
            this.Position = new Vector2(0, (768 / Controller.CurrentDrawCamera.zoom) - (96 / Controller.CurrentDrawCamera.zoom));
            background.Scale /= Controller.CurrentDrawCamera.zoom;
            this.AddChild(background);

            for (int i = 0; i < playerList.Count; i++)
            {
                hudPlayers.Add(playerList[i].index, new PlayerHUDStatus(new Vector2((1024f / Controller.CurrentDrawCamera.zoom) / playerList.Count, 96), playerList[i], timer));
                this.AddChild(hudPlayers[playerList[i].index]);
            }
        }

        public void PlayerDied(Vetbol player)
        {
            hudPlayers[player.index].Load();
        }

        public void PlayerSpawned(Vetbol player)
        {
            hudPlayers[player.index].ToNormal();
        }
    };

    class PlayerHUDStatus : Node
    {
        private Vector2 size;
        private Sprite playerImage;
        private Sprite playerFrame;
        private Label playerTickets;

        public Vetbol player;

        public PlayerHUDStatus(Vector2 size, Vetbol player, TimeSpan timer)
        {
            this.size = size;
            this.player = player;

            playerImage = new Sprite();
            playerFrame = new Sprite();

            playerFrame.Position.X = (size.X / 2) -((96 / Controller.CurrentDrawCamera.zoom) / 2) + (float)player.index * size.X;
            playerFrame.Position.Y = this.Position.Y;
            playerFrame.Scale /= Controller.CurrentDrawCamera.zoom;

            playerImage.Position.X = playerFrame.Position.X + ((96 - 20) / 2) - (playerImage.Width / 2);
            playerImage.Position.Y = this.Position.Y + ((96 - 65) / 2);
            playerImage.Scale /= Controller.CurrentDrawCamera.zoom;

            playerFrame.LoadTexture(Controller.Content.Load<Texture2D>(@"Assets/peter4croisants"), 96, 96);
            playerFrame.AddAnimation("IDLE", new int[1] { 0 }, 0);
            playerFrame.AddAnimation("LOADING", new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, (float)timer.TotalSeconds / 12);


            this.playerImage.LoadTexture(Controller.Content.Load < Texture2D >(@"images/slimeblobOther"), 48, 48);

            this.playerImage.Color = player.image.Color;

            this.playerTickets = new Label(player.score.ToString(), Controller.FontController.GetFont("DefaultFont"));
            this.playerTickets.Position = new Vector2(playerImage.Position.X + 10, this.Position.Y + 115);
            this.playerTickets.Scale /= Controller.CurrentDrawCamera.zoom - 0.3f;

            this.AddChild(playerFrame);
            this.AddChild(playerImage);
            this.AddChild(playerTickets);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.playerTickets.Text = player.score.ToString();
        }

        public void Load()
        {
            playerFrame.PlayAnimation("LOADING");
            playerImage.Color = Color.LightGray;
        }

        public void ToNormal()
        {
            playerFrame.PlayAnimation("IDLE");
            playerImage.Color = player.image.Color;
        }
    };
}

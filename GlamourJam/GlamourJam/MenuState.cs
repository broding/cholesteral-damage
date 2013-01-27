    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;
using Display.Tilemap;
using Microsoft.Xna.Framework;
using Flakcore;
using Flakcore.Utils;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using GlamourJam.States;

namespace GlamourJam
{
    class MenuState : State
    {
        private Dictionary<PlayerIndex, bool> playersReady = new Dictionary<PlayerIndex, bool>();
        private readonly TimeSpan scaleTime = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan timeBetweenHeartBeat = TimeSpan.FromMilliseconds(75);
        private TimeSpan scaleTimer;
        private Dictionary<Color, Vector2> playerData = new Dictionary<Color, Vector2>()
        {
            { Color.Yellow, new Vector2(285, 1180) },
            { Color.CornflowerBlue, new Vector2(685, 1180) },
            { Color.Lime, new Vector2(1085, 1180) },
            { Color.HotPink, new Vector2(1485, 1180) }
        };
        private GamePadState[] gamePadStates = new GamePadState[4];
        private Sprite[] player;
        private float heartScale = 1;
        private Sprite heart;
        private Sprite controls;
        private bool firstHeartBeat = true;

        private Label[] label = new Label[4];
        private Label[] labelPlayerindication = new Label[4];

        public MenuState()
        {
            scaleTimer = scaleTime;

            Sprite bg = new Sprite();
            bg.LoadTexture(@"Assets/HeartBG");
            bg.Scale /= Controller.CurrentDrawCamera.zoom;
            this.AddChild(bg);

            heart = new Sprite();
            heart.LoadTexture(@"Assets/Heart");
            heart.Scale /= (Controller.CurrentDrawCamera.zoom * 1.1f);
            heart.Position.X += 110;
            this.AddChild(heart);

            Sprite logo = new Sprite();
            logo.LoadTexture(@"Assets/logo");
            logo.Position.X = 300;
            logo.Position.Y = 20;
            logo.Scale *= 0.7f;
            this.AddChild(logo);

            controls = new Sprite();
            controls.LoadTexture(@"Assets/Controlls");
            controls.Scale /= Controller.CurrentDrawCamera.zoom;
            controls.Deactivate();
            this.AddChild(controls);

            Sprite controllsMessage = new Sprite();
            controllsMessage.LoadTexture(@"Assets/smallStartMessage");
            controllsMessage.Position.X = (1024 / Controller.CurrentDrawCamera.zoom) - (300 / Controller.CurrentDrawCamera.zoom);
            controllsMessage.Position.Y = (768 / Controller.CurrentDrawCamera.zoom) - (96 / Controller.CurrentDrawCamera.zoom) - (150 / Controller.CurrentDrawCamera.zoom);
            controllsMessage.Scale /= Controller.CurrentDrawCamera.zoom;
            this.AddChild(controllsMessage);

            Sprite backgroundBehindPlayer = new Sprite();
            backgroundBehindPlayer.LoadTexture(@"Assets/HudBG");
            backgroundBehindPlayer.Position = new Vector2(0, (768 / Controller.CurrentDrawCamera.zoom) - (96 / Controller.CurrentDrawCamera.zoom));
            backgroundBehindPlayer.Scale /= Controller.CurrentDrawCamera.zoom;
            this.AddChild(backgroundBehindPlayer);

            player = new Sprite[4];
            for (int i = 0; i < 4; i++)
            {
                player[i] = new Sprite();
                player[i].LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblobOther"), 48, 48);
                player[i].Scale /= Controller.CurrentDrawCamera.zoom;
                player[i].AddAnimation("IDLE", new int[1] { 0 }, 0);

                player[i].Color = playerData.ElementAt(i).Key;
                player[i].Position = playerData.ElementAt(i).Value;

                this.AddChild(player[i]);
            }

            for(int k = 0; k < 4; k++)
            {
                label[k] = new Label("", Controller.FontController.GetFont("DefaultFont"));
                label[k].Position = player[k].Position + new Vector2(-45, 100);
                label[k].Scale /= Controller.CurrentDrawCamera.zoom - 0.2f;
                this.AddChild(label[k]);
            }

            labelPlayerindication[0] = new Label("P1: ", Controller.FontController.GetFont("DefaultFont"));
            labelPlayerindication[0].Position = player[0].Position - new Vector2(60, -30);
            labelPlayerindication[0].Scale /= Controller.CurrentDrawCamera.zoom - 0.3f;
            labelPlayerindication[1] = new Label("P2: ", Controller.FontController.GetFont("DefaultFont"));
            labelPlayerindication[1].Position = player[1].Position - new Vector2(60, -30);
            labelPlayerindication[1].Scale /= Controller.CurrentDrawCamera.zoom - 0.3f;
            labelPlayerindication[2] = new Label("P3: ", Controller.FontController.GetFont("DefaultFont"));
            labelPlayerindication[2].Position = player[2].Position - new Vector2(60, -30);
            labelPlayerindication[2].Scale /= Controller.CurrentDrawCamera.zoom - 0.3f;
            labelPlayerindication[3] = new Label("P4: ", Controller.FontController.GetFont("DefaultFont"));
            labelPlayerindication[3].Position = player[3].Position - new Vector2(60, -30);
            labelPlayerindication[3].Scale /= Controller.CurrentDrawCamera.zoom - 0.3f;

            for (int l = 0; l < 4; l++)
            {
                this.AddChild(labelPlayerindication[l]);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < gamePadStates.Count(); i++)
            {
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i);
                if (gamePadStates[i].IsConnected && !playersReady.ContainsKey((PlayerIndex)i))
                {
                    playersReady.Add((PlayerIndex)i, false);
                    player[i].Alpha = 1.0f;
                    label[i].Text = "Press A when you're ready";
                }
                else if (!gamePadStates[i].IsConnected && playersReady.ContainsKey((PlayerIndex)i))
                {
                    playersReady.Remove((PlayerIndex)i);
                    player[i].Alpha = 0.2f;
                    label[i].Text = "Connect a controller";
                }
                else if(!gamePadStates[i].IsConnected)
                {
                    player[i].Alpha = 0.2f;
                    label[i].Text = "Connect a controller";
                }
            }

            for (int j = 0; j < playersReady.Count; j++)
            {
                if(Controller.Input.JustPressed(playersReady.ElementAt(j).Key, Buttons.A))
                {
                    playersReady[playersReady.ElementAt(j).Key] = !playersReady.ElementAt(j).Value;
                    if (playersReady[playersReady.ElementAt(j).Key])
                    {
                        label[j].Text = "You're Ready";
                    }
                    else
                    {
                        label[j].Text = "Press A when you're ready";
                    }
                }
            }

            if (Controller.Input.JustPressed(PlayerIndex.One, Buttons.Start) || Controller.Input.JustPressed(PlayerIndex.Two, Buttons.Start)
                || Controller.Input.JustPressed(PlayerIndex.Three, Buttons.Start) || Controller.Input.JustPressed(PlayerIndex.Four, Buttons.Start))
            {
                if (controls.Active)
                {
                    controls.Deactivate();
                }
                else
                {
                    controls.Activate();
                }
            }

            if (playersReady.Where(c => c.Value == true).Count() == playersReady.Count && playersReady.Count  > 1)
            {
                Controller.SwitchState(new GameState());
            }
            if (firstHeartBeat)
            {
                scaleTimer -= gameTime.ElapsedGameTime;
                if (scaleTimer.TotalMilliseconds <= 0)
                {
                    firstHeartBeat = false;
                    Heartbeat();
                    scaleTimer = timeBetweenHeartBeat;
                }
            }
            else
            {
                scaleTimer -= gameTime.ElapsedGameTime;
                if (scaleTimer.TotalMilliseconds <= 0)
                {
                    firstHeartBeat = true;
                    Heartbeat();
                    scaleTimer = scaleTime;
                }
            }
        }

        public void Heartbeat()
        {
            if (heartScale >= 1)
            {
                this.heartScale = 0.99f;
            }else
            {
                heartScale = 1.01f;
            }

            heart.Scale *= heartScale;
        }
    }
}

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
        private TimeSpan scaleTimer;
        private Dictionary<Color, Vector2> playerData = new Dictionary<Color, Vector2>()
        {
            { Color.Yellow, new Vector2(300, 1180) },
            { Color.Azure, new Vector2(700, 1180) },
            { Color.Lime, new Vector2(1100, 1180) },
            { Color.HotPink, new Vector2(1500, 1180) }
        };
        private GamePadState[] gamePadStates = new GamePadState[4];
        private Sprite[] player;
        private float heartScale = 1;
        private Sprite heart;

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
            heart.Position.X += 200;
            this.AddChild(heart);

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
                }
                else if (!gamePadStates[i].IsConnected && playersReady.ContainsKey((PlayerIndex)i))
                {
                    playersReady.Remove((PlayerIndex)i);
                    player[i].Alpha = 0.2f;
                }
                else if(!gamePadStates[i].IsConnected)
                {
                    player[i].Alpha = 0.2f;
                }
            }

            for (int j = 0; j < playersReady.Count; j++)
            {
                if(Controller.Input.JustPressed(playersReady.ElementAt(j).Key, Buttons.A))
                {
                    playersReady[playersReady.ElementAt(j).Key] = !playersReady.ElementAt(j).Value;
                }
            }

            if (playersReady.Where(c => c.Value == true).Count() == playersReady.Count && playersReady.Count  > 1)
            {
                Controller.SwitchState(new GameState());
            }

            scaleTimer -= gameTime.ElapsedGameTime;
            if (scaleTimer.TotalMilliseconds <= 0)
            {
                Heartbeat();
                scaleTimer = scaleTime;
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

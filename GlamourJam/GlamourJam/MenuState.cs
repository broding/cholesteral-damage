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
        private Dictionary<Color, Vector2> playerData = new Dictionary<Color, Vector2>()
        {
            { Color.Yellow, new Vector2(500, 700) },
            { Color.Azure, new Vector2(700) },
            { Color.Lime, new Vector2(900, 700) },
            { Color.HotPink, new Vector2(1100, 700) }
        };
        private GamePadState[] gamePadStates = new GamePadState[4];
        private Sprite[] player;

        public MenuState()
        {

            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            this.AddChild(bg);

            player = new Sprite[4];
            for (int i = 0; i < 4; i++)
            {
                player[i] = new Sprite();
                player[i].LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblobOther"), 48, 48);
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

            if (playersReady.Where(c => c.Value == true).Count() == playersReady.Count && playersReady.Count  > 0)
            {
                Controller.SwitchState(new GameState());
            }



            /*
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                player.Alpha = 1.0f;

            if (!GamePad.GetState(PlayerIndex.One).IsConnected)
                player.Alpha = 0.2f;

            if(GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                playerReady = true;

            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                player2.Alpha = 1.0f;

            if (!GamePad.GetState(PlayerIndex.Two).IsConnected)
                player2.Alpha = 0.2f;

            if(GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed)
                player2Ready = true;

            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                player3.Alpha = 1.0f;

            if (!GamePad.GetState(PlayerIndex.Three).IsConnected)
                player3.Alpha = 0.2f;

            if(GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed)
                player3Ready = true;

            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                player4.Alpha = 1.0f;

            if (!GamePad.GetState(PlayerIndex.Four).IsConnected)
                player4.Alpha = 0.2f;

            if(GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed)
                player4Ready = true; */

            //if (playerReady)
        }

    }


}

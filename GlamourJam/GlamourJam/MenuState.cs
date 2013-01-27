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
        GamePadState[] gamePadStates = new GamePadState[4];
        private Sprite player = new Sprite();
        private Sprite player2 = new Sprite();
        private Sprite player3 = new Sprite();
        private Sprite player4 = new Sprite();
        private bool playerReady = false;
        private bool player2Ready = false;
        private bool player3Ready = false;
        private bool player4Ready = false;
        public MenuState()
        {

            TiledSprite bg = new TiledSprite(2000, 2000);
            bg.LoadTexture("background");
            player.LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblobOther"), 48, 48);
            player.AddAnimation("IDLE", new int[1] { 0 }, 0);
            player2.LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblobOther"), 48, 48);
            player2.AddAnimation("IDLE", new int[1] { 0 }, 0);
            player3.LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblobOther"), 48, 48);
            player3.AddAnimation("IDLE", new int[1] { 0 }, 0);
            player4.LoadTexture(Controller.Content.Load<Texture2D>("images/slimeblobOther"), 48, 48);
            player4.AddAnimation("IDLE", new int[1] { 0 }, 0);

            player.Position = new Vector2(500, 700);
            player.Color = Color.Yellow;
            player2.Position = new Vector2(700, 700);
            player2.Color = Color.Azure;
            player3.Position = new Vector2(900, 700);
            player3.Color = Color.Lime;
            player4.Position = new Vector2(1100, 700);
            player4.Color = Color.HotPink;

            this.AddChild(bg);
            this.AddChild(player);
            this.AddChild(player2);
            this.AddChild(player3);
            this.AddChild(player4);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            gamePadStates[0] = GamePad.GetState(PlayerIndex.One);
            gamePadStates[1] = GamePad.GetState(PlayerIndex.Two);
            gamePadStates[2] = GamePad.GetState(PlayerIndex.Three);
            gamePadStates[3] = GamePad.GetState(PlayerIndex.Four);

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
                player4Ready = true;

            if (playerReady)
                Controller.SwitchState(new GameState());

            base.Update(gameTime);
        }

    }


}

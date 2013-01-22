using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Flakcore
{
    public class Input
    {
        private GamePadState[] PreviousGamepadStates = new GamePadState[4];
        private KeyboardState[] PreviousKeyboardStates = new KeyboardState[4];

        public void Update()
        {
            this.PreviousGamepadStates[(int)PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            this.PreviousGamepadStates[(int)PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            this.PreviousGamepadStates[(int)PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            this.PreviousGamepadStates[(int)PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);

            this.PreviousKeyboardStates[(int)PlayerIndex.One] = Keyboard.GetState(PlayerIndex.One);
            this.PreviousKeyboardStates[(int)PlayerIndex.Two] = Keyboard.GetState(PlayerIndex.Two);
            this.PreviousKeyboardStates[(int)PlayerIndex.Three] = Keyboard.GetState(PlayerIndex.Three);
            this.PreviousKeyboardStates[(int)PlayerIndex.Four] = Keyboard.GetState(PlayerIndex.Four);
        }

        public GamePadState GetPadState(PlayerIndex player)
        {
            return GamePad.GetState(player);
        }

        public bool JustPressed(PlayerIndex player, Buttons button)
        {
            GamePadState currentState = GamePad.GetState(player);

            if(currentState.IsButtonDown(button) && this.PreviousGamepadStates[(int)player].IsButtonUp(button))
                return true;
            else
                return false;
        }

        public bool JustPressed(PlayerIndex player, Keys key)
        {
            KeyboardState currentState = Keyboard.GetState(player);

            if (currentState.IsKeyDown(key) && this.PreviousKeyboardStates[(int)player].IsKeyUp(key))
                return true;
            else
                return false;
        }

        public InputState GetInputState(PlayerIndex player)
        {
            InputState state = new InputState();

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
                state.X = -1;
            else if(keyboardState.IsKeyDown(Keys.Right))
                state.X = 1;

            if (keyboardState.IsKeyDown(Keys.Up))
                state.Y = -1;
            else if (keyboardState.IsKeyDown(Keys.Down))
                state.Y = 1;

            if (keyboardState.IsKeyDown(Keys.Space))
                state.Jump = true;

            if (keyboardState.IsKeyDown(Keys.X))
                state.Fire = true;

            return state;
        }
    }

    public struct InputState
    {
        public float X;
        public float Y;
        public bool Jump;
        public bool Fire;
    }
}

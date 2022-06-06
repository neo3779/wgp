using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Assignment
{
    class Input
    {
        //Previous control states
        GamePadState previousGamePadState;
        #if (!XBOX360)
            KeyboardState previousKeyboardState;
            MouseState previousMouseState;
        #endif

        //Current control states
        GamePadState currentGamePadState;
        #if (!XBOX360)
            MouseState currentMouseState;
            KeyboardState currentKeyboardState;
        #endif
            
        public Input()
        {
            previousGamePadState = GamePad.GetState(PlayerIndex.One);
            #if (!XBOX360)
                previousKeyboardState = Keyboard.GetState();
                previousMouseState = Mouse.GetState();
            #endif
        }

        public void updateCurrent()
        {
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            #if (!XBOX360)
                currentMouseState = Mouse.GetState();
                currentKeyboardState = Keyboard.GetState();
            #endif
        }

        public void updatePrevious()
        {
            previousGamePadState = GamePad.GetState(PlayerIndex.One);
            #if (!XBOX360)
                previousKeyboardState = Keyboard.GetState();
                previousMouseState = Mouse.GetState();
            #endif
        }

        public Boolean fireButton()
        {
            this.updateCurrent();
            #if (!XBOX360)
            if ((currentGamePadState.Buttons.A == ButtonState.Pressed &&
                 previousGamePadState.Buttons.A == ButtonState.Released)||
                 (currentMouseState.LeftButton == ButtonState.Pressed &&
                 previousMouseState.LeftButton == ButtonState.Released) ||
                 currentKeyboardState.IsKeyDown(Keys.Space) &&
                 previousKeyboardState.IsKeyUp(Keys.Space))
                    
            {
                GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.0f);
                return true;
            }
            else
            {
                return false;
            }
            #endif

            if ((currentGamePadState.Buttons.A == ButtonState.Pressed &&
                previousGamePadState.Buttons.A == ButtonState.Released))
            {
                GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.0f);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void restVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
        }

        public Boolean isLeftKeyDown()
        {   
            #if (!XBOX360)
            return currentKeyboardState.IsKeyDown(Keys.Left);
            #endif
            return false;

        }

        public Boolean isRightKeyDown()
        {
            #if (!XBOX360)
            return currentKeyboardState.IsKeyDown(Keys.Right);
            #endif
            return false;
        }
        
        public Boolean isLeftUpDown()
        {
            #if (!XBOX360)
            return currentKeyboardState.IsKeyDown(Keys.Up);
            #endif
            return false;
        }
        
        public Boolean isLeftDownDown()
        {
            #if (!XBOX360)
            return currentKeyboardState.IsKeyDown(Keys.Down);
            #endif
            return false;
        }

        public float gamePadLftX()
        {
            return currentGamePadState.ThumbSticks.Left.X;
        }

        public float gamePadLftY()
        {
            return currentGamePadState.ThumbSticks.Left.Y;
        }

        public Boolean isMouseXEqualPrevious()
        {
            #if (!XBOX360)
            if (currentMouseState.X != previousMouseState.X)
            {
                return true;
            }
            else
            {
                return false;
            }
            #endif

            return false;
        }

        public Boolean isMouseYEqualPrevious()
        {
            #if (!XBOX360)
            if (currentMouseState.Y != previousMouseState.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
            #endif
            return false;
        }

        public float mouseX()
        {
            #if (!XBOX360)
            return currentMouseState.X;
            #endif
            return 0.0f;

        }
        
        public float mouseY()
        {
            #if (!XBOX360)
            return currentMouseState.Y;
            #endif
            return 0.0f;
        }

        public Boolean isNewKeyPress()
        {
            #if (!XBOX360)
            if ((currentGamePadState.Buttons.A != previousGamePadState.Buttons.A)||
                (currentKeyboardState.GetPressedKeys().Length != previousKeyboardState.GetPressedKeys().Length)||
                (currentMouseState.LeftButton != previousMouseState.LeftButton))
                
            {
                return true;
            }
            else
            {
                return false;
            }
            #endif

            if ((currentGamePadState.Buttons.A != previousGamePadState.Buttons.A))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean isKeyPress()
        {   
            #if (!XBOX360)
            if ((currentGamePadState.Buttons.A == ButtonState.Pressed)
                ||(currentKeyboardState.GetPressedKeys().Length > 0)  || 
                (currentMouseState.LeftButton == ButtonState.Pressed))
                
            {
                return true;
            }
            else
            {
                return false;
            }
            #endif

            if ((currentGamePadState.Buttons.A == ButtonState.Pressed))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

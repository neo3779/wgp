using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Assignment
{
    abstract class UserControlledSprite: Sprite

    {
        Input controls;

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
            controls = new Input();
        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            controls = new Input();
        }

        public override Vector2 direction
        {
            get
            {

                controls.updateCurrent();

                Vector2 inputDirection = Vector2.Zero;
                #if (!XBOX360)
                if (controls.isLeftKeyDown())
                    inputDirection.X -= 1;
                if (controls.isRightKeyDown())
                    inputDirection.X += 1;
                if (controls.isLeftUpDown())
                    inputDirection.Y -= 1;
                if (controls.isLeftDownDown())
                    inputDirection.Y += 1;
                #endif

                if (controls.gamePadLftX() != 0)
                    inputDirection.X += controls.gamePadLftX();
                if (controls.gamePadLftY() != 0)
                    inputDirection.Y -= controls.gamePadLftY();
                
                #if (!XBOX360)
                if (controls.isMouseXEqualPrevious() ||
                    controls.isMouseYEqualPrevious())
                {
                    inputDirection.X = controls.mouseX();
                    inputDirection.Y = controls.mouseY();
                }
                #endif

                return inputDirection * speed;
            }
            
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite according to the direction property
            position += direction;
            // If the mouse moved, set the position of the sprite to the mouse position
            
            #if (!XBOX360)
            if (controls.isMouseXEqualPrevious() ||
            controls.isMouseYEqualPrevious())
            {
                position = new Vector2(controls.mouseX(), controls.mouseY());
            }
            #endif

            controls.updatePrevious();
            // If the sprite is off the screen, put it back in play
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;
            base.Update(gameTime, clientBounds);
        }

    }
}

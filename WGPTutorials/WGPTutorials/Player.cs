using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment
{        

    class Player:UserControlledSprite
    {
        public Boolean spray { get; set; }

        public Player(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }

        public Player(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (spray == true && currentFrame.X == 1 && currentFrame.Y == 3)
            {
                spray = false;
                this.currentFrame = new Point(0, 0);
            }

            if (spray)
            {
                
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        ++currentFrame.Y;
                        if (currentFrame.Y >= sheetSize.Y)
                            currentFrame.Y = 0;
                    }
                }
            }
            else
            {
                currentFrame.X = 0;
                currentFrame.Y = 0;
            }
            base.Update(gameTime, clientBounds);
        }
    }
}

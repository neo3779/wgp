using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment
{
    abstract class Sprite
    {

        protected Texture2D textureImage;
        public Vector2 position;
        protected Point frameSize;
        protected int collisionOffset;
        public Point currentFrame;
        protected Point sheetSize;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected Vector2 speed;
        const int defaultMillisecondsPerFrame = 16;

        public Color colour
        {
            get;
            set;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
        int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }


        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.colour = Color.White;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,position,new Rectangle(currentFrame.X * frameSize.X,currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),colour, 0, Vector2.Zero,1f, SpriteEffects.None, 0);
        }

        public virtual Vector2 direction
        {
            get;
            set;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Assignment
{

    class Midge : AutomatedSprite
    {

        private int changeRed;
        private int changeBlue;

        public bool dead
        {
            get;
            set;
        }



        public Midge(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int changered)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
            this.changeRed = changered;
            this.changeBlue = 3000;
            this.colour = Color.White;
            this.dead = false;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            
                // If the sprite is off the screen, put it back in play
            if (position.X + frameSize.X < 0 && direction.X < 0)
            {
                if (colour != Color.Blue)
                {
                    position.X = clientBounds.Width - frameSize.X;
                }
                else
                {
                    dead = true;
                }
            }
            if (position.X > clientBounds.Width && direction.X > 0){
                if (colour != Color.Blue)
                    {
                    position.X = 0;
                }
                    else
                    {
                        dead = true;
                    }
            }
            if (position.Y + frameSize.Y < 0 && direction.Y < 0)
            {
                if (colour != Color.Blue)
                {
                    position.Y = clientBounds.Height - frameSize.Y;
                }
                else
                {
                    dead = true;
                }
            }
            if (position.Y > clientBounds.Height && direction.Y > 0)
            {
                if (colour != Color.Blue)
                {
                    position.Y = 0;
                }
                else
                {
                    dead = true;
                }
            }

            //color changing
            if (colour == Color.White)
            {

                if (changeRed > 0)
                {
                    changeRed -= gameTime.ElapsedGameTime.Milliseconds; ;
                }
                else
                {
                    colour = Color.Red;
                } 
            }else
                if (colour == Color.Red)
                {
                    if (changeBlue > 0)
                    {
                        changeBlue -= gameTime.ElapsedGameTime.Milliseconds; ;
                    }
                    else
                    {
                        colour = Color.Blue;
                    }

                }
            

            base.Update(gameTime, clientBounds);

        }

    }
}

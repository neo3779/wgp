using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Assignment
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Player player;
        Alien boss;
        List<Sprite> spriteList = new List<Sprite>();
        GameAudio sound;
        Input controls;
        Boolean fired = false;
        int vibTime = 250;
        int range = 2;
        
        //Score values
        int midStart = 5, midRem = 25, midEsc = 0, puffs = 40, midCount = 0 , midKilled = 0;
        public int MidRem { get { return midRem; } }
        public int MidEsc { get { return midEsc; } }
        public int Puffs { get { return puffs; } }
        public int MidKilled { get { return midKilled; } }

        //Spawn max/min
        int midSpawnMinMilSecs = 1000;
        int midSpawnMaxMilSecs = 2000;

        int midMinColChangeSecs = 5000;
        int midMaxColChangeSecs = 15000;
        
        //Midge's speed max/min
        int midMinSpeed = 1;
        int midMaxSpeed = 3;

        //Timr until next Midge spawns
        int nextSpawnTime = 0;

        // Game started or not.
        Boolean gameStarted;

        //Restspawntime for next spawn of the migde.
        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
            midSpawnMinMilSecs,
            midSpawnMaxMilSecs);
        }

        
        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            sound = new GameAudio("Content/Audio/IGPCA.xgs", "Content/Audio/IGPCAwavBank.xwb", "Content/Audio/IGPCAsouBank.xsb",
                                  "fly", "Volume", 0.0f);
            controls = new Input();

            ResetSpawnTime();
            
            for (int i = 0; i < midStart; i++)
            {
                SpawnEnemy();
            }

            gameStarted = false;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0 && midCount < 20)
            {
                SpawnEnemy();
                // Reset spawn timer
                ResetSpawnTime();
                midCount++;
            }

            if (((Game1)Game).currentGameState == Game1.GameState.Level2)
            {
                if (boss.collisionRect.Intersects(player.collisionRect))
                {
                    ((Game1)Game).currentGameState = Game1.GameState.GameOver;

                }

                bossVsPlayer();

                boss.Update(gameTime, Game.Window.ClientBounds);
            }

            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            if (fired)
            {
                vibTime -=gameTime.ElapsedGameTime.Milliseconds;
            }

            if (vibTime < 0)
            {
                controls.restVibration();
                fired = false;
                vibTime = 250;
            }

            // Check for collisions 
            if (gameStarted)
            {
                if (controls.fireButton())
                {
                    if (puffs > 0)
                    {

                        fired = true;
                        puffs--;
                        sound.playSound("spray");
                        player.spray = true;
                        player.currentFrame = new Point(1, 2);

                        List<Sprite> spriteList2 = new List<Sprite>();

                        foreach (Sprite s in spriteList)
                        {
                            if (s.collisionRect.Intersects(player.collisionRect) && (s as Midge).colour == Color.Red)
                            {
                                sound.playSound("MidgeKilled");
                                spriteList2.Add(s);
                                midRem--;
                                midKilled++;
                            }

                        }
                        foreach (Sprite s in spriteList2)
                        {
                            spriteList.Remove(s);
                        }

                        spriteList2.Clear();

                    }
                }
            }

            controls.updatePrevious();

            List<Sprite> spriteList3 = new List<Sprite>();

            // Update all sprites
            foreach (Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
                if ((s as Midge).dead)
                {
                    sound.playSound("tada");
                    midRem--;
                    midEsc++;
                    spriteList3.Add(s);
                }
            }

            foreach (Sprite s in spriteList3)
            {
                spriteList.Remove(s);
            }
            
            spriteList3.Clear();

            sound.backGroundVol(spriteList.Count);
            
            gameStarted = true;

            base.Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime)
        {

            if (((Game1)Game).currentGameState == Game1.GameState.Level1 ||
                ((Game1)Game).currentGameState == Game1.GameState.Level2)
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.BackToFront, SaveStateMode.None);
                
                // Draw the player
                player.Draw(gameTime, spriteBatch);
                if (((Game1)Game).currentGameState == Game1.GameState.Level2)
                {
                    boss.Draw(gameTime, spriteBatch);
                }
                // Draw all sprites
                foreach (Sprite s in spriteList)
                    s.Draw(gameTime, spriteBatch);

                spriteBatch.End();

                base.Draw(gameTime);
            }
        }

        protected override void LoadContent( )
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            player = new Player(
            Game.Content.Load<Texture2D>(@"Textures\\midgespray"),
            Vector2.Zero, new Point(43, 38),10, new Point(0, 0),
            new Point(4,6), new Vector2(10, 10));

            boss = new Alien(
            Game.Content.Load <Texture2D> (@"Textures\\Alien"),
            Vector2.Zero, new Point(72, 91), 10, new Point(0, 0),
             new Point(0, 0), new Vector2(2, 2));
            
            base.LoadContent();
        }
        
        private void SpawnEnemy( )
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            int change = 0;

            // Default frame size
            Point frameSize = new Point(75, 75);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            if (((Game1)Game).currentGameState == Game1.GameState.Level2)
            {
                range = 4;
            }
            else if (((Game1)Game).currentGameState == Game1.GameState.Level1)
            { range = 2; }

            switch (((Game1)Game).rnd.Next(range))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                        -frameSize.X, ((Game1)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(((Game1)Game).rnd.Next(midMinSpeed, midMaxSpeed), 0);

                    change = ((Game1)Game).rnd.Next(midMinColChangeSecs,midMaxColChangeSecs);
                break;
                
                case 1: // RIGHT to LEFT
                    position = new Vector2(Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                        ((Game1)Game).rnd.Next(0,Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(-((Game1)Game).rnd.Next(midMinSpeed, midMaxSpeed), 0);

                    change = ((Game1)Game).rnd.Next(midMinColChangeSecs, midMaxColChangeSecs);
                break;
                case 2: // BOTTOM to TOP
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                                            Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                                            - frameSize.X),
                                            Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
                    
                    speed = new Vector2(0, -((Game1)Game).rnd.Next(midMinSpeed, midMaxSpeed));

                    change = ((Game1)Game).rnd.Next(midMinColChangeSecs, midMaxColChangeSecs);
                break;
                case 3: // TOP to BOTTOM
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                                            Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                                            - frameSize.X), -frameSize.Y);

                    speed = new Vector2(0, -((Game1)Game).rnd.Next(midMinSpeed, midMaxSpeed));

                    change = ((Game1)Game).rnd.Next(midMinColChangeSecs, midMaxColChangeSecs);
                break;
                
            }

        // Create the sprite
        spriteList.Add(
            new Midge(Game.Content.Load<Texture2D>(@"Textures\\midge"),
            position, new Point(64, 64), 10, new Point(0, 0),
            new Point(1, 1), speed,change));
        }

        public void restGame()
        {
            range = 2;
            controls.restVibration();
            sound.backGroundVol(0);
            midStart = 5; 
            midRem = 25; 
            midEsc = 0; 
            puffs = 40; 
            midCount = 0;
            spriteList.Clear();
            gameStarted = false;
            for (int i = 0; i < midStart; i++)
            {
                SpawnEnemy();
            }
        }

        private void bossVsPlayer()
        {
            float x = boss.position.X;

            if (player.position.X != boss.position.X)
            {
                if (player.position.X > boss.position.X)
                {
                    x = boss.position.X + 1;
                }
                if (player.position.X < boss.position.X)
                {
                    x = boss.position.X - 1;
                }
            }

            float y = boss.position.Y;

            if (player.position.Y != boss.position.Y)
            {
                if (player.position.Y > boss.position.Y)
                {
                    y = boss.position.Y + 1;
                }
                if (player.position.Y < boss.position.Y)
                {
                    y = boss.position.Y - 1;
                }
            }
            
            boss.position = new Vector2(x,y);
        }

    }
}
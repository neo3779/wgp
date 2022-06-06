using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Assignment
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteManager spriteManager;

        ///Load background image 
        private Texture2D level1bgTex, firLevel2bgTex, secLevel2bgTex;
        private Rectangle viewportRect;

        ///Text
        private SpriteFont arialFont;
        private Vector2 scorepos;

        //Input
        private Input controls;

        //Text
        private string text;

        //Game state
        public enum GameState { Start, Level1, Level2, Level1Complete, GameOver, GameComplete};
        public GameState currentGameState { get; set; }

        //Random number
        public Random rnd { get; private set; }

        public Game1()
        {
            controls = new Input();
            currentGameState = GameState.Start;
            rnd = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            #if (!XBOX360)
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            #endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ///Backgound
            level1bgTex = Content.Load<Texture2D>("Textures\\background");
            firLevel2bgTex = Content.Load<Texture2D>("Textures\\B1_nebula02");
            secLevel2bgTex = Content.Load<Texture2D>("Textures\\B1_stars");

            viewportRect = new Rectangle(0, 0,GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height);

            ///Text
            arialFont = Content.Load<SpriteFont>("Fonts\\Arial");

            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Update(GameTime gameTime)
        {

            switch (currentGameState)
            {
                case GameState.Start:

                    controls.updateCurrent();

                    if (controls.isNewKeyPress())
                    {
                        if (controls.isKeyPress())
                        {
                            currentGameState = GameState.Level1;
                            spriteManager.Enabled = true;
                            spriteManager.Visible = true;
                        }
                    }

                    controls.updatePrevious();

                    break;

                case GameState.Level1:

                    controls.updateCurrent();

                    if (spriteManager.MidEsc >= 3)
                    {
                        currentGameState = GameState.GameOver;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    if (spriteManager.MidEsc < 3 && spriteManager.MidRem <= 0)
                    {
                        currentGameState = GameState.Level1Complete;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    controls.updatePrevious();

                    break;
                case GameState.Level2:

                    controls.updateCurrent();

                    if (spriteManager.MidEsc >= 3)
                    {
                        currentGameState = GameState.GameOver;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    if (spriteManager.MidEsc < 3 && spriteManager.MidRem <= 0)
                    {
                        currentGameState = GameState.GameComplete;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    controls.updatePrevious();
                    break;
                case GameState.GameComplete:

                    controls.updateCurrent();

                    spriteManager.restGame();

                                        if (controls.isNewKeyPress())
                    {
                    if (controls.isKeyPress())
                    {
                        spriteManager.Visible = true;
                        currentGameState = GameState.Start;
                    }
                                            }

                    controls.updatePrevious();

                    break;
                case GameState.Level1Complete:

                    controls.updateCurrent();

                    spriteManager.restGame();
                    if (controls.isNewKeyPress())
                    {
                        if (controls.isKeyPress())
                        {
                            currentGameState = GameState.Level2;
                            spriteManager.Enabled = true;
                            spriteManager.Visible = true;
                            
                        }
                    }

                    controls.updatePrevious();
                    break;
                case GameState.GameOver:

                    controls.updateCurrent();
                    
                    spriteManager.restGame();
                    if (controls.isKeyPress())
                    {
                        spriteManager.Visible = true;
                        currentGameState = GameState.Start;
                    }

                    controls.updatePrevious();

                    break;

            }
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.Start:

                    GraphicsDevice.Clear(Color.AliceBlue);

                    //Draw test for into splash screen
                    spriteBatch.Begin();
                    
                    text = "(Press Any Key To Begin)";
                    
                    spriteBatch.Draw(level1bgTex, viewportRect, Color.White);

                    
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2)),
                        Color.White);

                    text = "Kill all the midges when they turn red, if three escape then the planet is doomed.";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2) + 30),
                        Color.White);

                    spriteBatch.End();

                    controls.updatePrevious();

                    break;
                case GameState.Level1:

                    GraphicsDevice.Clear(Color.White);
                    base.Draw(gameTime);

                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    ///Backgound
                    graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    spriteBatch.Draw(level1bgTex, viewportRect, Color.White);

                    ///Text
                    spriteBatch.DrawString(arialFont, "Midges Remaining: " + spriteManager.MidRem + " Midges Escaped: " + spriteManager.MidEsc + " Remaining Puffs: " + spriteManager.Puffs + "", Vector2.Zero, Color.White);

                    spriteBatch.End();

                    // TODO: Add your drawing code here
                    break;
                    
                case GameState.Level2:

                    GraphicsDevice.Clear(Color.White);
                    base.Draw(gameTime);

                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    ///Backgound
                    graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    spriteBatch.Draw(firLevel2bgTex, viewportRect, Color.White);
                    spriteBatch.Draw(secLevel2bgTex, viewportRect, Color.White);

                    ///Text
                    spriteBatch.DrawString(arialFont, "Midges Remaining: " + spriteManager.MidRem + " Midges Escaped: " + spriteManager.MidEsc + " Remaining Puffs: " + spriteManager.Puffs + "", Vector2.Zero, Color.White);

                    spriteBatch.End();

                    break;
                case GameState.Level1Complete:
                    GraphicsDevice.Clear(Color.AliceBlue);

                    //Draw test for into splash screen
                    spriteBatch.Begin();

                    spriteBatch.Draw(level1bgTex, viewportRect, Color.White);

                    text = "(Press Any Key To Begin)";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2)),
                        Color.White);

                    text = "Well Done. One level down, one to go.";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2) + 30),
                        Color.White);

                    spriteBatch.End();
                    break;
                case GameState.GameComplete:

                    GraphicsDevice.Clear(Color.AliceBlue);

                    //Draw test for into splash screen
                    spriteBatch.Begin();

                    spriteBatch.Draw(level1bgTex, viewportRect, Color.White);

                    text = "(Press Any Key To Begin)";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2)),
                        Color.White);

                    text = "Well Done";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2) + 30),
                        Color.White);

                    spriteBatch.End();

                    break;
                case GameState.GameOver:

                    GraphicsDevice.Clear(Color.AliceBlue);

                    //Draw test for into splash screen
                    spriteBatch.Begin();

                    spriteBatch.Draw(level1bgTex, viewportRect, Color.White);

                    text = "(Press Any Key To Begin)";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2)),
                        Color.White);

                    text = "You Failed";
                    spriteBatch.DrawString(arialFont, text, new Vector2(
                        (Window.ClientBounds.Width / 2) - (arialFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (arialFont.MeasureString(text).Y / 2) + 30),
                        Color.White);

                    spriteBatch.End();

                    break;

            }



            base.Draw(gameTime);
        }
    }
}

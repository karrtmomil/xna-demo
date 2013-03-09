using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Represents the player
        Player player;

        // Keyboard states used to determine
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        // Gamepad tates used to determin button presses
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        // A movement speed for the player
        float playerMoveSpeed;

        // Image used to display the static background
        Texture2D mainBackground;

        // Parallaxing Layers
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize the player class
            player = new Player();

            // Set a constant player move speed
            playerMoveSpeed = 8.0f;

            // enabel the FreeDrag gesture.
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            // Init paralaxxing Background
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

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

            // Load the player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTecture = Content.Load<Texture2D>("shipAnimation");
            playerAnimation.Initialize(playerTecture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialze(playerAnimation, playerPosition);

            // Load the parallaxing background
            bgLayer1.Initialize(Content, "bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "bgLayer2", GraphicsDevice.Viewport.Width, -2);

            mainBackground = Content.Load<Texture2D>("mainbackground");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Save the previous state of the keyboard and game pad so we can determinesinglekey/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // Upadate the player
            UpdatePlayer(gameTime);

            // Update the parallaxing background
            bgLayer1.Update();
            bgLayer2.Update();

            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            // Get Thumbstick Controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                    currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                    currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                    currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                    currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMoveSpeed;
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, player.Width / 2, GraphicsDevice.Viewport.Width - player.Width / 2);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, player.Height / 2, GraphicsDevice.Viewport.Height - player.Height / 2);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            spriteBatch.Begin();

            // Draw the background
            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            // Draw the Player
            player.Draw(spriteBatch);

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

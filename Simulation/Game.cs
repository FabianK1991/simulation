﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Simulation.Game;
using Simulation.Game.World;
using Simulation.Spritesheet;
using Comora;
using System;
using System.Collections.Generic;
using Simulation.Game.Spells;

namespace Simulation
{
    public struct Size
    {
        public Size(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public int Width;
        public int Height;
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SimulationGame : Microsoft.Xna.Framework.Game
    {
        public static Camera camera
        {
            get; private set;
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private World world;
        private Player player;

        private List<Fireball> fireballs = new List<Fireball>();

        public static Size resolution = new Size(1280, 768); 

        public static ContentManager contentManager
        {
            get; private set;
        }

        public static Rectangle visibleArea;

        public SimulationGame()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = resolution.Width;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = resolution.Height;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            contentManager = Content;

            player = new Player();
            world = new World();

            visibleArea = Rectangle.Empty;
        }

        private void updateVisibleArea()
        {
            visibleArea.X = (int)(SimulationGame.camera.Position.X - resolution.Width * 0.5f);
            visibleArea.Y = (int)(SimulationGame.camera.Position.Y - resolution.Height * 0.5f);

            visibleArea.Width = resolution.Width;
            visibleArea.Height = resolution.Height;
        }

        private Vector2 mouseToWorld()
        {
            var mousePosition = Mouse.GetState().Position;

            return new Vector2(visibleArea.X + mousePosition.X, visibleArea.Y + mousePosition.Y);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(graphics.GraphicsDevice);
            //camera.Debug.IsVisible = true;
            camera.Position = new Vector2(64, 64);

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

            player.LoadContent(Content);

            //camera.LoadContent();
            //camera.Debug.Grid.AddLines(32, Color.White, 2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            player.Update(gameTime);
            camera.Update(gameTime);

            if(Keyboard.GetState().IsKeyDown(Keys.D1) && fireballs.Count == 0)
            {
                var mousePosition = mouseToWorld();
                var direction = new Vector2(mousePosition.X - camera.Position.X, mousePosition.Y - camera.Position.Y);

                direction.Normalize();

                fireballs.Add(new Fireball(camera.Position, direction));
            }

            foreach(Fireball fireball in fireballs)
            {
                fireball.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            updateVisibleArea();
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(camera);

            world.Draw(spriteBatch);
            player.Draw(spriteBatch);

            for(int i=0;i<fireballs.Count;i++)
            {
                Fireball fireball = fireballs[i];

                if (SimulationGame.visibleArea.Contains(fireball.Position))
                {
                    fireball.Draw(spriteBatch);
                }
                else
                {
                    fireballs.Remove(fireball);
                    i--;
                }
            }

            spriteBatch.End();

            // spriteBatch.Draw(camera.Debug);

            base.Draw(gameTime);
        }
    }
}

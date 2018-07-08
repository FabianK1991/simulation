﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Simulation.Game.Base.Entity;
using Simulation.Game.Hud;
using Simulation.Game.Renderer.Entities;
using Simulation.Game.Skills;
using Simulation.Util;

namespace Simulation.Game
{
    public class Player: DurableEntity
    {
        private FireballSkill fireballSkill;
        private bool leftMouseClick = false;

        public Player(): base(LivingEntityType.PLAYER, new Vector2(0, 0), new Rectangle(-8, -20, 16, 20), 3)
        {
            fireballSkill = new FireballSkill(this, new Vector2(0, -20));
        }

        public override void updatePosition(Vector2 newPosition)
        {
            base.updatePosition(newPosition);

            SimulationGame.camera.Position = new Vector2(position.X, (int)position.Y);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            Vector2 newDirection = Vector2.Zero;

            if (state.IsKeyDown(Keys.D))
            {
                newDirection.X += 1.0f;
            }

            if (state.IsKeyDown(Keys.A))
            {
                newDirection.X -= 1.0f;
            }

            if (state.IsKeyDown(Keys.W))
            {
                newDirection.Y -= 1.0f;
            }

            if (state.IsKeyDown(Keys.S))
            {
                newDirection.Y += 1.0f;
            }

            if (state.IsKeyDown(Keys.D1))
            {
                // fireballSkill.use(SimulationGame.mousePosition);
            }

            direction = newDirection;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!leftMouseClick)
                {
                    if (SimulationGame.isDebug)
                    {
                        Point currentBlock = GeometryUtils.getChunkPosition((int)SimulationGame.mousePosition.X, (int)SimulationGame.mousePosition.Y, World.World.BlockSize.X, World.World.BlockSize.Y);

                        GameConsole.WriteLine("Block", "Block: " + currentBlock.X + ", " + currentBlock.Y);
                    }

                    leftMouseClick = true;
                }
            }
            else
            {
                leftMouseClick = false;
            }

            base.Update(gameTime);
        }
    }
}

﻿namespace Simulation.Spritesheet
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Linq;

    public class Spritesheet
    {
        public Spritesheet(Texture2D texture) : this(texture, null, null, null) { }

        private Spritesheet(Texture2D texture, Point? cellSize = null, Point? cellOffset = null, Point? cellOrigin = null, double frameDuration = 200.0f, SpriteEffects frameEffects = SpriteEffects.None)
        {
            this.Texture = texture;
            this.CellSize = cellSize ?? new Point(0, 0);
            this.CellOffset = cellOffset ?? new Point(0, 0);
            this.CellOrigin = cellOrigin ?? new Point(0, 0);
            this.FrameDefaultDuration = frameDuration;
            this.FrameDefaultEffects = frameEffects;
        }

        #region Properties

        public Texture2D Texture { get; }

        public Point CellSize { get; }

        public Point CellOffset { get; }

        public Point CellOrigin { get; }

        public double FrameDefaultDuration { get; }

        public SpriteEffects FrameDefaultEffects { get; }

        #endregion

        #region Grid

        public Spritesheet WithGrid((int w, int h) cell, (int x, int y) offset, (int x, int y) cellOrigin)
        {
            return new Spritesheet(this.Texture, new Point(cell.w, cell.h), new Point(offset.x, offset.y), new Point(cellOrigin.x, cellOrigin.y), this.FrameDefaultDuration, this.FrameDefaultEffects);
        }

        public Spritesheet WithGrid((int w, int h) cell, (int x, int y) offset)
        {
            return this.WithGrid(cell, offset, (0, 0));
        }

        public Spritesheet WithGrid((int w, int h) cell)
        {
            return this.WithGrid(cell, (0, 0), (0, 0));
        }

        public Spritesheet WithGrid(Point cell)
        {
            return new Spritesheet(this.Texture, cell, Point.Zero, Point.Zero, this.FrameDefaultDuration, this.FrameDefaultEffects);
        }

        #endregion

        #region Frame default settings

        public Spritesheet WithCellOrigin(Point cellOrigin)
        {
            return new Spritesheet(this.Texture, this.CellSize, this.CellOffset, cellOrigin, this.FrameDefaultDuration, this.FrameDefaultEffects);
        }

        public Spritesheet WithFrameDuration(double durationInMs)
        {
            return new Spritesheet(this.Texture, this.CellSize, this.CellOffset, this.CellOrigin, durationInMs, this.FrameDefaultEffects);
        }

        public Spritesheet WithFrameEffect(SpriteEffects effects)
        {
            return new Spritesheet(this.Texture, this.CellSize, this.CellOffset, this.CellOrigin, this.FrameDefaultDuration, effects);
        }

        #endregion

        public Frame CreateFrame(int x, int y, double duration = 0.0, SpriteEffects effects = SpriteEffects.None)
        {
            x = x * this.CellSize.X + this.CellOffset.X;
            y = y * this.CellSize.Y + this.CellOffset.Y;
            var area = new Rectangle(x, y, this.CellSize.X, this.CellSize.Y);
            return new Frame(this.Texture, area, this.CellOrigin, duration, effects);
        }

        public Animation CreateAnimation(params (int x, int y, double duration, SpriteEffects effects)[] frames)
        {
            return new Animation(frames.Select(f => this.CreateFrame(f.x, f.y, f.duration, f.effects)).ToArray());
        }

        public Animation CreateAnimation(params (int x, int y, double duration)[] frames)
        {
            return this.CreateAnimation(frames.Select(x => (x.x, x.y, x.duration, this.FrameDefaultEffects)).ToArray());
        }

        public Animation CreateAnimation(params (int x, int y)[] frames)
        {
            return this.CreateAnimation(frames.Select(x => (x.x, x.y, this.FrameDefaultDuration, this.FrameDefaultEffects)).ToArray());
        }
    }
}
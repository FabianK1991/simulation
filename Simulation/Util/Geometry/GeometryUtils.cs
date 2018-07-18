﻿using Microsoft.Xna.Framework;
using Simulation.Game.World;
using System;
using System.Runtime.CompilerServices;

namespace Simulation.Util.Geometry
{
    public enum ReservedDepthLayers
    {
        Block = 0,
        BlockDecoration = 1
    }

    public class GeometryUtils
    {
        private static readonly int ReservedDepthLayers = 100;
        private static readonly int HalfResolutionWidth = SimulationGame.Resolution.Width / 2;
        private static readonly int HalfResolutionHeight = SimulationGame.Resolution.Height / 2;

        public static readonly float SmallFloat = 0.1f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetVectorDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VectorsWithinDistance(float x1, float y1, float x2, float y2, float d)
        {
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) < d * d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VectorsWithinDistance(int x1, int y1, int x2, int y2, int d)
        {
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) < d * d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLayerDepthFromReservedLayer(ReservedDepthLayers layer)
        {
            return Normalize((int)layer,
                            0,
                            SimulationGame.VisibleArea.Width * SimulationGame.VisibleArea.Height + ReservedDepthLayers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLayerDepthFromReservedLayer(int zIndex)
        {
            return Normalize(zIndex,
                            0,
                            SimulationGame.VisibleArea.Width * SimulationGame.VisibleArea.Height + ReservedDepthLayers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLayerDepthFromPosition(float X, float Y)
        {
            float value = (Y - SimulationGame.VisibleArea.Top) * SimulationGame.VisibleArea.Width + (X - SimulationGame.VisibleArea.Left) + ReservedDepthLayers;

            return Normalize(value,
                            0,
                            SimulationGame.VisibleArea.Width * SimulationGame.VisibleArea.Height + ReservedDepthLayers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float min, float max) => Math.Max(0.0f, Math.Min(1.0f, (value - min) / (max - min)));

        public static Vector2 Rotate(float angle, Vector2 pivot, ref Vector2 point)
        {
            Vector2 rotatedPoint = point;

            rotatedPoint.X -= pivot.X;
            rotatedPoint.Y -= pivot.Y;

            // Rotate about the origin.
            Matrix mat = Matrix.CreateRotationZ(angle);
            rotatedPoint = Vector2.Transform(rotatedPoint, mat);

            rotatedPoint.X += pivot.X;
            rotatedPoint.Y += pivot.Y;

            return rotatedPoint;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAngleFromDirection(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetBlockFromReal(int realX, int realY)
        {
            return new Point(realX < 0 ? (realX / WorldGrid.BlockSize.X) - (realX % WorldGrid.BlockSize.X != 0 ? 1 : 0) : realX / WorldGrid.BlockSize.X, realY < 0 ? (realY / WorldGrid.BlockSize.Y) - (realY % WorldGrid.BlockSize.Y != 0 ? 1 : 0) : realY / WorldGrid.BlockSize.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndexFromPoint(int realX, int realY, int chunkWidth, int chunkHeight)
        {
            return (realX < 0 ? -realX - 1 : realX) % chunkWidth + chunkWidth * ((realY < 0 ? -realY - 1 : realY) % chunkHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetChunkPosition(int realX, int realY, int chunkWidth, int chunkHeight)
        {
            return new Point(realX < 0 ? (realX / chunkWidth) - (realX % chunkWidth != 0 ? 1 : 0) : realX / chunkWidth, realY < 0 ? (realY / chunkHeight) - (realY % chunkHeight != 0 ? 1 : 0) : realY / chunkHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ConvertPointToLong(int x, int y) => ((ulong)x << 32) | ((ulong)y & 0xFFFFFFFF);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetPointFromLong(ulong point) => new Point((int)(point >> 32), (int)point);

        /* 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Point GetChunkPositionNew(int realX, int realY, int chunkWidth, int chunkHeight)
            {
                float x = (float)realX / (float)chunkWidth;
                float y = (float)realY / (float)chunkHeight;

                return new Point((int)x - ((x < (int)x) ? 1 : 0), (int)y - ((y < (int)y) ? 1 : 0));
            }
        */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point GetPositionWithinChunk(int realX, int realY, int chunkWidth, int chunkHeight)
        {
            return new Point(realX - (realX < 0 ? (realX / chunkWidth) - (realX % chunkWidth != 0 ? 1 : 0) : realX / chunkWidth) * chunkWidth, realY - (realY < 0 ? (realY / chunkHeight) - (realY % chunkHeight != 0 ? 1 : 0) : realY / chunkHeight) * chunkHeight);
        }
    }
}

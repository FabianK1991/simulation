﻿using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

namespace Simulation.Util.Geometry
{
    public struct Circle
    {
        public int CenterX;
        public int CenterY;

        public int Radius;

        public Circle(int centerX, int centerY, int radius)
        {
            CenterX = centerX;
            CenterY = centerY;

            Radius = radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ToVector()
        {
            return new Vector2(CenterX, CenterY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point point) => (point.X - CenterX) * (point.X - CenterX) + (point.Y - CenterY) * (point.Y - CenterY) <= Radius * Radius;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int x, int y) => (x - CenterX) * (x - CenterX) + (y - CenterY) * (y - CenterY) <= Radius * Radius;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Circle other)
        {
            return (CenterX - other.CenterX) * (CenterX - other.CenterX) + (CenterY - other.CenterY) * (CenterY - other.CenterY) <= (Radius + other.Radius) * (Radius + other.Radius);
        }

        public bool Intersects(Rect rect)
        {
            float halfWidth = rect.Width / 2.0f;
            float halfHeight = rect.Height / 2.0f;

            float cx = Math.Abs(CenterX - rect.X - halfWidth);
            float xDist = halfWidth + Radius;

            if (cx > xDist)
                return false;

            float cy = Math.Abs(CenterY - rect.Y - halfHeight);
            float yDist = halfHeight + Radius;

            if (cy > yDist)
                return false;

            if (cx <= halfWidth || cy <= halfHeight)
                return true;

            float xCornerDist = cx - halfWidth;
            float yCornerDist = cy - halfHeight;
            float xCornerDistSq = xCornerDist * xCornerDist;
            float yCornerDistSq = yCornerDist * yCornerDist;
            float maxCornerDistSq = Radius * Radius;

            return xCornerDistSq + yCornerDistSq <= maxCornerDistSq;
        }
    }
}

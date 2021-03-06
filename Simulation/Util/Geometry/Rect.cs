﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Simulation.Game.World;
using System.Runtime.CompilerServices;

namespace Simulation.Util.Geometry
{
    public struct Rect
    {
        public static readonly Rect Empty = new Rect(0, 0, 0, 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect Union(Rect rect1, Rect rect2)
        {
            // Maybe use ShapeCollision.ConvertPolyToRect idea to implement this?

            // Doesn't have to be efficient so we just use the XNA one
            return new Rect(Rectangle.Union(rect1.ToXnaRectangle(), rect2.ToXnaRectangle()));
        }

        public int X;
        public int Y;

        public int Width;
        public int Height;

        [JsonIgnore]
        public int Left
        {
            get
            {
                return X;
            }
        }

        [JsonIgnore]
        public int Top
        {
            get
            {
                return Y;
            }
        }

        [JsonIgnore]
        public int Right
        {
            get
            {
                return X + Width - 1;
            }
        }

        [JsonIgnore]
        public int Bottom
        {
            get
            {
                return Y + Height - 1;
            }
        }

        public Rect(Rectangle rectangle)
        {
            X = rectangle.X;
            Y = rectangle.Y;

            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        public Rect(Point position, Point size)
        {
            X = position.X;
            Y = position.Y;

            Width = size.X;
            Height = size.Y;
        }

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetPosition()
        {
            return new Point(X, Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 GetPositionVector()
        {
            return new Vector2(X, Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point GetSize()
        {
            return new Point(Width, Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 GetSizeVector()
        {
            return new Vector2(Width, Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Rect rect)
        {
            return (X <= rect.Right) && (Right >= rect.X) && (Y <= rect.Bottom) && (Bottom >= rect.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector2 point)
        {
            return (point.X >= X) && (point.X <= Right) && (point.Y >= Y) && (point.Y <= Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(WorldPosition point)
        {
            return (point.X >= X) && (point.X <= Right) && (point.Y >= Y) && (point.Y <= Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Point point)
        {
            return (point.X >= X) && (point.X <= Right) && (point.Y >= Y) && (point.Y <= Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int x, int y)
        {
            return (x >= X) && (x <= Right) && (y >= Y) && (y <= Bottom);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rectangle ToXnaRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}

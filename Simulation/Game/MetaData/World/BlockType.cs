﻿using Microsoft.Xna.Framework;
using Simulation.Game.Fractions;
using Simulation.Game.World;
using System.Collections.Generic;

namespace Simulation.Game.MetaData.World
{
    public class BlockType: MetaDataType
    {
        public static readonly int Invalid = -1;
        public static readonly int None = 0;
        public static Dictionary<int, BlockType> lookup = new Dictionary<int, BlockType>()
        {
            {0, new BlockType()
            {
                ID=0,
                Name="None",
                IsBlocking=true,
                IsHitable=true
            }},
            {1, new BlockType()
            {
                ID=1,
                Name="Grass",
                SpritePath=@"Tiles\Exterior\Exterior01",
                SpritePosition=new Point(0, 0)
            }},
        };

        public bool IsBlocking = false;
        public bool IsHitable = false;

        // Render
        public string SpritePath = null;
        public Point SpritePosition = Point.Zero;
        public Point SpriteBounds = new Point(WorldGrid.BlockSize.X, WorldGrid.BlockSize.Y);
    }
}

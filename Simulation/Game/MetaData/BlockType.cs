﻿using Microsoft.Xna.Framework;
using Simulation.Game.Enums;
using Simulation.Game.World;
using System.Collections.Generic;

namespace Simulation.Game.MetaData
{
    public class BlockType
    {
        public static readonly int None = 0;
        public static readonly Dictionary<int, BlockType> lookup = new Dictionary<int, BlockType>()
        {
            {0, new BlockType()
            {
                ID=0,
                Name="None",
                BlockingType=BlockingType.BLOCKING,
                HitBoxType=HitBoxType.HITABLE_BLOCK
            }},
            {1, new BlockType()
            {
                ID=1,
                Name="Grass_01",
                SpritePath=@"terrain_atlas",
                SpritePostion=new Point(672, 160)
            }},
            {2, new BlockType()
            {
                ID=2,
                Name="Grass_Waterhole",
                SpritePath=@"terrain_atlas",
                BlockingType=BlockingType.BLOCKING,
                HitBoxType=HitBoxType.HITABLE_BLOCK,
                SpritePostion=new Point(192, 288)
            }}
        };

        public int ID;
        public string Name;

        public BlockingType BlockingType = BlockingType.NOT_BLOCKING;
        public HitBoxType HitBoxType = HitBoxType.NO_HITBOX;

        // Render
        public string SpritePath;
        public Point SpritePostion;
        public Point SpriteBounds = new Point(WorldGrid.BlockSize.X, WorldGrid.BlockSize.Y);
    }
}
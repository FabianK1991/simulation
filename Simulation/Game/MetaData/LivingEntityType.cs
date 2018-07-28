﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Simulation.Game.Enums;
using Simulation.Game.MetaData.AI;
using Simulation.Game.MetaData.Skills;
using Simulation.Game.Objects.Entities;
using Simulation.Game.Renderer.Entities;
using Simulation.Game.Skills;
using Simulation.Game.World;
using Simulation.Spritesheet;
using Simulation.Util.Geometry;
using System.Collections.Generic;

namespace Simulation.Game.MetaData
{
    public class LivingEntityType
    {
        public static readonly int Player = 0;
        public static readonly Dictionary<int, LivingEntityType> lookup = new Dictionary<int, LivingEntityType>()
        {
            {0, new LivingEntityType(){
                ID=0,
                Name="Player",
                MaximumLife=100,
                CurrentLife=100,
                Fraction=FractionType.PLAYER,
                PreloadedSurroundingWorldGridChunkRadius=3,
                Velocity=0.2f,
                Skills=new SkillMetaData[]
                {
                    new SlashSkillMetaData(),
                    new FireballSkillMetaData()
                },
                SpritePath=@"Characters\Player",
                DownAnimation=new Point[]
                {
                    new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0)
                },
                UpAnimation=new Point[]
                {
                    new Point(0, 3), new Point(1, 3), new Point(2, 3), new Point(3, 3)
                },
                LeftAnimation=new Point[]
                {
                    new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1)
                },
                RightAnimation=new Point[]
                {
                    new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2)
                }
            }},
            {1, new LivingEntityType(){
                ID=1,
                Name="Geralt",
                MaximumLife=100,
                CurrentLife=100,
                Fraction=FractionType.NPC,
                IsDurableEntity=true,
                Skills=new SkillMetaData[]
                {
                    new SlashSkillMetaData(),
                    new FireballSkillMetaData()
                },
                AIMetaData=new WanderAIMetaData() {
                    BlockRadius=10
                },
                SpritePath=@"Characters\Geralt",
                DownAnimation=new Point[]
                {
                    new Point(1, 0), new Point(0, 0), new Point(1, 0), new Point(2, 0)
                },
                UpAnimation=new Point[]
                {
                    new Point(1, 3), new Point(0, 3), new Point(1, 3), new Point(2, 3)
                },
                LeftAnimation=new Point[]
                {
                    new Point(1, 1), new Point(0, 1), new Point(1, 1), new Point(2, 1)
                },
                RightAnimation=new Point[]
                {
                    new Point(1, 2), new Point(0, 2), new Point(1, 2), new Point(2, 2)
                }
            }}
        };

        public int ID;
        public string Name;

        public int MaximumLife;
        public int CurrentLife;
        public float LifeRegeneration = 0;
        public FractionType Fraction;
        public int AttentionBlockRadius = 10;
        public float Velocity = 0.08f;

        public SkillMetaData[] Skills = null;
        public AIMetaData AIMetaData = null;
        
        public Rect RelativeHitBoxBounds = new Rect(-14, -38, 28, 48);
        public Rect RelativeBlockingBounds = new Rect(-8, -10, 16, 20);
        public BlockingType BlockingType = BlockingType.NOT_BLOCKING;

        public bool IsDurableEntity = false;
        public int PreloadedSurroundingWorldGridChunkRadius = 1;

        // Rendering
        public string SpritePath;
        public Point SpriteBounds=new Point(32, 48);
        public Point SpriteOrigin=new Point(16, 38);
        
        public Point[] DownAnimation;
        public Point[] UpAnimation;
        public Point[] LeftAnimation;
        public Point[] RightAnimation;

        public bool WithGrid = true;
        public int FrameDuration = 160;

        public static LivingEntity Create(WorldPosition worldPosition, LivingEntityType livingEntityType)
        {
            MovingEntity livingEntity = null;

            if(livingEntityType.IsDurableEntity)
            {
                livingEntity = new DurableEntity(worldPosition)
                {
                    PreloadedSurroundingWorldGridChunkRadius=livingEntityType.PreloadedSurroundingWorldGridChunkRadius
                };
            }
            else if(livingEntityType.ID == Player)
            {
                livingEntity = new Player(worldPosition)
                {
                    PreloadedSurroundingWorldGridChunkRadius = livingEntityType.PreloadedSurroundingWorldGridChunkRadius
                };
            }
            else
            {
                livingEntity = new MovingEntity(worldPosition);
            }

            livingEntity.LivingEntityType = livingEntityType.ID;

            livingEntity.MaximumLife = livingEntityType.MaximumLife;
            livingEntity.CurrentLife = livingEntityType.CurrentLife;
            livingEntity.LifeRegeneration = livingEntityType.LifeRegeneration;
            livingEntity.Fraction = livingEntityType.Fraction;
            livingEntity.AttentionBlockRadius = livingEntityType.AttentionBlockRadius;
            livingEntity.Velocity = livingEntityType.Velocity;

            if(livingEntityType.Skills.Length > 0)
            {
                livingEntity.Skills = new Skill[livingEntityType.Skills.Length];

                for (int i = 0; i < livingEntityType.Skills.Length; i++)
                {
                    var skill = livingEntityType.Skills[i];

                    livingEntity.Skills[i] = SkillMetaData.Create(livingEntity, skill);
                }
            }

            if(livingEntityType.AIMetaData != null)
            {
                livingEntity.BaseAI = AIMetaData.Create(livingEntity, livingEntityType.AIMetaData);
            }

            livingEntity.RelativeBlockingBounds = livingEntityType.RelativeBlockingBounds;
            livingEntity.RelativeHitBoxBounds = livingEntityType.RelativeHitBoxBounds;
            livingEntity.BlockingType = livingEntityType.BlockingType;

            livingEntity.Init();

            return livingEntity;
        }

        public static LivingEntityRendererInformation CreateRendererInformation(LivingEntity livingEntity)
        {
            var livingEntityType = lookup[livingEntity.LivingEntityType];

            var texture = SimulationGame.ContentManager.Load<Texture2D>(livingEntityType.SpritePath);
            var sheet = new Spritesheet.Spritesheet(texture);
            
            if(livingEntityType.WithGrid)
            {
                sheet = sheet.WithGrid(livingEntityType.SpriteBounds);
            }

            sheet = sheet.WithCellOrigin(livingEntityType.SpriteOrigin).WithFrameDuration(livingEntityType.FrameDuration);

            Frame[] downFrames = new Frame[livingEntityType.DownAnimation.Length];
            Frame[] upFrames = new Frame[livingEntityType.UpAnimation.Length];
            Frame[] leftFrames = new Frame[livingEntityType.LeftAnimation.Length];
            Frame[] rightFrames = new Frame[livingEntityType.RightAnimation.Length];

            for (var i=0;i<livingEntityType.DownAnimation.Length;i++)
                downFrames[i] = sheet.CreateFrame(livingEntityType.DownAnimation[i].X, livingEntityType.DownAnimation[i].Y, sheet.FrameDefaultDuration, sheet.FrameDefaultEffects);
            for (var i = 0; i < livingEntityType.UpAnimation.Length; i++)
                upFrames[i] = sheet.CreateFrame(livingEntityType.UpAnimation[i].X, livingEntityType.UpAnimation[i].Y, sheet.FrameDefaultDuration, sheet.FrameDefaultEffects);
            for (var i = 0; i < livingEntityType.LeftAnimation.Length; i++)
                leftFrames[i] = sheet.CreateFrame(livingEntityType.LeftAnimation[i].X, livingEntityType.LeftAnimation[i].Y, sheet.FrameDefaultDuration, sheet.FrameDefaultEffects);
            for (var i = 0; i < livingEntityType.RightAnimation.Length; i++)
                rightFrames[i] = sheet.CreateFrame(livingEntityType.RightAnimation[i].X, livingEntityType.RightAnimation[i].Y, sheet.FrameDefaultDuration, sheet.FrameDefaultEffects);

            var rendererInformation = new LivingEntityRendererInformation(
                new Animation(downFrames),
                new Animation(upFrames), 
                new Animation(leftFrames), 
                new Animation(rightFrames)
            );

            return rendererInformation;
        }
    }
}
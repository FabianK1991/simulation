﻿using Microsoft.Xna.Framework;
using Simulation.Game.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Game.AI
{
    public class FightingAI: BaseAI
    {
        public FightingAI(MovingEntity movingEntity): base(movingEntity) { }

        protected override void addAITasks()
        {
            throw new NotImplementedException();
        }
    }
}

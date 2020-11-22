using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine;

namespace RainingFire.Container
{
    class RainingFireObject
    {
        public Agent attackerAgent;
        public MissionTimer burningtime;
        public GameEntity fireEntity;
        public List<ParticleSystem> Flame = new List<ParticleSystem>();
        public int damagecount;
        public bool isBurning = false;
    }
}

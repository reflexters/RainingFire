using System;
using TaleWorlds.Core;

namespace RainingFire.Setting
{
    class RainingFireBattle
    {
        private RainingFireBattle() { }
        public static RainingFireBattle getInstance()
        {
            if (instance == null)
                instance = new RainingFireBattle();
            return instance;
        }

        public bool battle = false;
        public bool duel = false;
        public bool stealth = false;
        public bool tournament = false;

        private static RainingFireBattle instance;
    }
}

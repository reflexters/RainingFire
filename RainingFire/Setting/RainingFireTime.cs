using System;
using TaleWorlds.Engine;

namespace RainingFire
{
    class RainingFireTime
    {
        private RainingFireTime() { }
        
        public static RainingFireTime getInstance()
        {
            if (instance == null)
            {
                instance = new RainingFireTime();
            }
            return instance;
        }

        public bool rainingfireCheckTime(float time)
        {
            bool rval = false;
            int currenttime = (int)time % 24;
            int timediff = rainingfirestarttime - rainingfireendtime;
            if (timediff == 0)
                rval = true;
            else
            {
                if(timediff < 0)
                {
                    if (currenttime >= rainingfirestarttime && currenttime <= rainingfireendtime)
                        rval = true;
                }
                else if(timediff > 0)
                {
                    if (!(currenttime < rainingfirestarttime && currenttime > rainingfireendtime))
                        rval = true;
                }
            }
            return rval;
        }

        public static int rainingfirestarttime;
        public static int rainingfireendtime;
        public static int rainingfireburningtime;

        private static RainingFireTime instance;
    }
}

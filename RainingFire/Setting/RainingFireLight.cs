using System;
using TaleWorlds.Engine;

namespace RainingFire.Setting
{
    class RainingFireLight
    {
        private RainingFireLight() 
        {
            Arrow = Light.CreatePointLight(0f);
            Bolt = Light.CreatePointLight(0f);
            ThrowingAxe = Light.CreatePointLight(0f);
            ThrowingKnife = Light.CreatePointLight(0f);
            Javelin = Light.CreatePointLight(0f);
        }

        public static RainingFireLight getInstance()
        {
            if (instance == null)
            {
                instance = new RainingFireLight();
            }
            return instance;
        }

        public static Light Arrow { get; set; }
        public static Light Bolt { get; set; }
        public static Light ThrowingAxe { get; set; }
        public static Light ThrowingKnife { get; set; }
        public static Light Javelin { get; set; }
        private static RainingFireLight instance;
    }
}

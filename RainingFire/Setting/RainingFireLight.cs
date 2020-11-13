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

        public Light Arrow { get; set; }
        public Light Bolt { get; set; }
        public Light ThrowingAxe { get; set; }
        public Light ThrowingKnife { get; set; }
        public Light Javelin { get; set; }
        private static RainingFireLight instance;
    }
}

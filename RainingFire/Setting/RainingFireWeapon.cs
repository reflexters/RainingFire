using System;
using TaleWorlds.Core;

namespace RainingFire.Setting
{
    class RainingFireWeapon
    {
        private RainingFireWeapon() { }

        public static RainingFireWeapon getInstance() 
        {
            if (instance == null)
            {
                instance = new RainingFireWeapon();
            }
            
            return instance;
        }

        public bool rainingfireCheckWeapon(WeaponClass weapon)
        {
            bool rval = false;
            switch (weapon)
            {
                case WeaponClass.Bow:
                    if (this.Arrow)
                        rval = true;
                    break;
                case WeaponClass.Crossbow:
                    if (this.Bolt)
                        rval = true;
                    break;
                case WeaponClass.ThrowingAxe:
                    if (this.ThrowingAxe)
                        rval = true;
                    break;
                case WeaponClass.ThrowingKnife:
                    if (this.ThrowingKnife)
                        rval = true;
                    break;
                case WeaponClass.Javelin:
                    if (this.Javelin)
                        rval = true;
                    break;
                default:
                    rval = false;
                    break;
            }
            return rval;
        }

        public bool Arrow { get; set; }
        public bool Bolt { get; set; }
        public bool ThrowingAxe { get; set; }
        public bool ThrowingKnife { get; set; }
        public bool Javelin { get; set; }

        private static RainingFireWeapon instance;
    }
}

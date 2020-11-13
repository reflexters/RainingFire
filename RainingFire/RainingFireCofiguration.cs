using System;
using System.IO;
using Newtonsoft.Json.Linq;
using RainingFire.Setting;
using TaleWorlds.Library;

namespace RainingFire
{
    class RainingFireCofiguration
    {
        public void parsingJSON()
        {
            string setting = File.ReadAllText(BasePath.Name + "Modules/RainingFire/setting/RainingFireSetting.json");
            JObject jsondata = JObject.Parse(setting);

            RainingFireCofiguration.rainingFireSwitch = (bool)jsondata["rainingfireswitch"];
            RainingFireTime.rainingfirestarttime = (int)jsondata["rainingfirestarttime"] - 1;
            RainingFireTime.rainingfireendtime = (int)jsondata["rainingfireendtime"] - 1;
            RainingFireTime.rainingfireburningtime = (int)jsondata["rainingfireburningtime"];

            foreach (JObject weaponsetlist in jsondata["rainingfireweaponsetting"])
            {
                inputConfig(weaponsetlist);
            }
        }

        public void inputConfig(JObject data)
        {
            RainingFireWeapon rainingFireWeapon = RainingFireWeapon.getInstance();
            RainingFireLight rainingFireLight = RainingFireLight.getInstance();
            switch ((string)data["name"])
            {
                case "arrow":
                    rainingFireWeapon.Arrow = (bool)data["flag"];
                    RainingFireLight.Arrow.Intensity = (float)data["intensity"];
                    RainingFireLight.Arrow.Radius = (float)data["radius"];
                    break;
                case "bolt":
                    rainingFireWeapon.Bolt = (bool)data["flag"];
                    RainingFireLight.Bolt.Intensity = (float)data["intensity"];
                    RainingFireLight.Bolt.Radius = (float)data["radius"];
                    break;
                case "throwingaxe":
                    rainingFireWeapon.ThrowingAxe = (bool)data["flag"];
                    RainingFireLight.ThrowingAxe.Intensity = (float)data["intensity"];
                    RainingFireLight.ThrowingAxe.Radius = (float)data["radius"];
                    break;
                case "throwingknife":
                    rainingFireWeapon.ThrowingKnife = (bool)data["flag"];
                    RainingFireLight.ThrowingKnife.Intensity = (float)data["intensity"];
                    RainingFireLight.ThrowingKnife.Radius = (float)data["radius"];
                    break;
                case "javelin":
                    rainingFireWeapon.Javelin = (bool)data["flag"];
                    RainingFireLight.Javelin.Intensity = (float)data["intensity"];
                    RainingFireLight.Javelin.Radius = (float)data["radius"];
                    break;
            }
            
            
        }

        public static bool rainingFireSwitch { get; set; }
    }
}

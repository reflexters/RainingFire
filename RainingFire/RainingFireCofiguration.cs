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

            RainingFireTime fireTime = RainingFireTime.getInstance();
            RainingFireBattle battle = RainingFireBattle.getInstance();

            RainingFireCofiguration.rainingFireSwitch = (bool)jsondata["rainingfireswitch"];
            fireTime.rainingfirestarttime = (int)jsondata["rainingfirestarttime"] - 1;
            fireTime.rainingfireendtime = (int)jsondata["rainingfireendtime"] - 1;
            fireTime.rainingfireburningtime = (int)jsondata["rainingfireburningtime"];
            fireTime.rainingfiredamagepersec = (int)jsondata["rainingfiredamagepersec"];

            JArray battletype = (JArray)jsondata["rainingfirebattletype"];
            foreach(JObject array in battletype.Children())
            {
                switch((string)array["type"])
                {
                    case "battle":
                        battle.battle = (bool)array["flag"];
                        break;
                    case "duel":
                        battle.duel = (bool)array["flag"];
                        break;
                    case "stealth":
                        battle.stealth = (bool)array["flag"];
                        break;
                    case "tournament":
                        battle.tournament = (bool)array["flag"];
                        break;
                }
            }

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
                    rainingFireLight.Arrow.Intensity = (float)data["intensity"];
                    rainingFireLight.Arrow.Radius = (float)data["radius"];
                    break;
                case "bolt":
                    rainingFireWeapon.Bolt = (bool)data["flag"];
                    rainingFireLight.Bolt.Intensity = (float)data["intensity"];
                    rainingFireLight.Bolt.Radius = (float)data["radius"];
                    break;
                case "throwingaxe":
                    rainingFireWeapon.ThrowingAxe = (bool)data["flag"];
                    rainingFireLight.ThrowingAxe.Intensity = (float)data["intensity"];
                    rainingFireLight.ThrowingAxe.Radius = (float)data["radius"];
                    break;
                case "throwingknife":
                    rainingFireWeapon.ThrowingKnife = (bool)data["flag"];
                    rainingFireLight.ThrowingKnife.Intensity = (float)data["intensity"];
                    rainingFireLight.ThrowingKnife.Radius = (float)data["radius"];
                    break;
                case "javelin":
                    rainingFireWeapon.Javelin = (bool)data["flag"];
                    rainingFireLight.Javelin.Intensity = (float)data["intensity"];
                    rainingFireLight.Javelin.Radius = (float)data["radius"];
                    break;
            }
            
            
        }

        public static bool rainingFireSwitch { get; set; }
    }
}

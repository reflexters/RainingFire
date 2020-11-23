using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using RainingFire.Setting;

namespace RainingFire
{
    public class RainingFireInitialize : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            ModInitialize();
            InformationManager.DisplayMessage(new InformationMessage("Succeed to load RainingFire."));
        }

        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);
            mission.AddMissionBehaviour(new RainingFireAttachFire());
        }

        public void ModInitialize()
        {
            RainingFireWeapon.getInstance();
            RainingFireLight.getInstance();
            RainingFireTime.getInstance();
            RainingFireBattle.getInstance();
            RainingFireConfiguration configuration = new RainingFireConfiguration();
            configuration.parsingJSON();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            InformationManager.DisplayMessage(new InformationMessage("RainingFire v1.0.4 is successfully loaded."));
        }
    }
}

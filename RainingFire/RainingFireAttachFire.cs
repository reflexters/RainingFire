using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using TaleWorlds.Engine;
using TaleWorlds.CampaignSystem;
using RainingFire.Setting;
using RainingFire.Container;


namespace RainingFire
{
    class RainingFireAttachFire : MissionLogic
    {
  //      public override void OnMissionTick(float dt)
  //      {
  //          base.OnMissionTick(dt);
		//	SpinLock sl = new SpinLock();
		//	bool splock = false;
		//	RainingFireTime rainingFireTime = RainingFireTime.getInstance();

		//	if (RainingFireCofiguration.rainingFireSwitch)
		//	{
		//		sl.Enter(ref splock);
		//		for (int i = 0 ; i < projectiles.Count ; i++)
		//		{
		//			RainingFireProjectile rainingFireProjectile = projectiles[i];
		//			if (rainingFireProjectile.timer.Check(true))
		//			{
		//				rainingFireProjectile.missile.Entity.RemoveAllParticleSystems();
		//				Light light = rainingFireProjectile.missile.Entity.GetLight();
		//				if (light != null)
		//					rainingFireProjectile.missile.Entity.RemoveComponent(light);
						
		//				projectiles.Remove(rainingFireProjectile);
		//			}
		//		}
		//		sl.Exit();
		//	}
		//}
		public bool GetCurrentCombatType()
        {
			MissionMode currentMode = Mission.Current.Mode;
			RainingFireBattle battle = RainingFireBattle.getInstance();
			bool rval = false;

			switch(currentMode)
            {
				case MissionMode.Battle:
					if(battle.battle)
						rval = true;
					break;
				case MissionMode.Duel:
					if(battle.duel)
						rval = true;
					break;
				case MissionMode.Stealth:
					if(battle.stealth)
						rval = true;
					break;
				case MissionMode.Tournament:
					if(battle.tournament)
						rval = true;
					break;
				default:
					rval = false;
					break;
			}
			return rval;
        }

		public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        {
			RainingFireWeapon rainingFireWeapon = RainingFireWeapon.getInstance();
			RainingFireTime rainingFireTime = RainingFireTime.getInstance();
			RainingFireLight rainingFireLight = RainingFireLight.getInstance();

			
			//InformationManager.DisplayMessage(new InformationMessage("Current time is " + Campaign.CurrentTime%24f));
			if (RainingFireCofiguration.rainingFireSwitch && rainingFireTime.rainingfireCheckTime(Campaign.CurrentTime) && GetCurrentCombatType())
			{
				WeaponClass weapon = shooterAgent.Equipment[weaponIndex].CurrentUsageItem.WeaponClass;
				if (rainingFireWeapon.rainingfireCheckWeapon(weapon))
                {
					foreach (Mission.Missile missile in Mission.Current.Missiles)
					{
						MatrixFrame matrixFrame = new MatrixFrame(Mat3.Identity, new Vec3(0f, 0f, 0f, -1f));
						ParticleSystem.CreateParticleSystemAttachedToEntity("psys_game_missile_flame", missile.Entity, ref matrixFrame);
						Light light = Light.CreatePointLight(1f);
						switch(weapon)
						{
							case WeaponClass.Bow:
								light.Intensity = rainingFireLight.Arrow.Intensity;
								light.Radius	= rainingFireLight.Arrow.Radius;
								break;
							case WeaponClass.Crossbow:
								light.Intensity = rainingFireLight.Bolt.Intensity;
								light.Radius	= rainingFireLight.Bolt.Radius;
								break;
							case WeaponClass.ThrowingAxe:
								light.Intensity = rainingFireLight.ThrowingAxe.Intensity;
								light.Radius	= rainingFireLight.ThrowingAxe.Radius;
								break;
							case WeaponClass.ThrowingKnife:
								light.Intensity = rainingFireLight.ThrowingKnife.Intensity;
								light.Radius	= rainingFireLight.ThrowingKnife.Radius;
								break;
							case WeaponClass.Javelin:
								light.Intensity = rainingFireLight.Javelin.Intensity;
								light.Radius	= rainingFireLight.Javelin.Radius;
								break;
							default:
								light.Intensity = 10f;
								light.Radius	= 100f;
								break;

						}
						light.LightColor = new Vec3(204f, 102f, 0f, -1f); //RGB color
						missile.Entity.AddLight(light);
						break;
					}
				}
			}
		}

        public override void OnMissileCollisionReaction(Mission.MissileCollisionReaction collisionReaction, Agent attackerAgent, Agent attachedAgent, sbyte attachedBoneIndex)
        {
			SpinLock sl = new SpinLock();
			bool splock = false;
			RainingFireTime rainingFireTime = RainingFireTime.getInstance();

			if (RainingFireCofiguration.rainingFireSwitch && rainingFireTime.rainingfireCheckTime(Campaign.CurrentTime) && GetCurrentCombatType())
			{
				base.OnMissileCollisionReaction(collisionReaction, attackerAgent, attachedAgent, attachedBoneIndex);

				foreach (Mission.Missile missile in Mission.Current.Missiles)
				{
					Light light = missile.Entity.GetLight();
					if (attachedAgent != null)
					{
						missile.Entity.RemoveAllParticleSystems();
						if (light != null)
							missile.Entity.RemoveComponent(light);
					}
					else
					{
						missile.Entity.RemoveAllParticleSystems();
						MatrixFrame matrixFrame = new MatrixFrame(Mat3.Identity, new Vec3(0f, 0f, 0f, -1f));
						ParticleSystem.CreateParticleSystemAttachedToEntity("psys_campfire", missile.Entity, ref matrixFrame);
						if (light != null)
						{
							light.Intensity = 5f;
							light.Radius = 5f;
						}

						//RainingFireProjectile rainingFireProjectile = new RainingFireProjectile();
						//rainingFireProjectile.timer = new MissionTimer((float)RainingFireTime.rainingfireburningtime);
						//rainingFireProjectile.missile = missile;
						//splock = false;
						//sl.Enter(ref splock);
						//projectiles.Add(rainingFireProjectile);
						//sl.Exit();
					}
				}
			}
		}

		//private List<RainingFireProjectile> projectiles = new List<RainingFireProjectile>();
    }
}

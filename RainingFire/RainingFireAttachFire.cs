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

		public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        {
			RainingFireWeapon rainingFireWeapon = RainingFireWeapon.getInstance();
			RainingFireLight rainingFireLight = RainingFireLight.getInstance();
			RainingFireTime rainingFireTime = RainingFireTime.getInstance();

			
			//InformationManager.DisplayMessage(new InformationMessage("Current time is " + Campaign.CurrentTime%24f));
			if (RainingFireCofiguration.rainingFireSwitch && rainingFireTime.rainingfireCheckTime(Campaign.CurrentTime))
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
								light.Intensity = RainingFireLight.Arrow.Intensity;
								light.Radius	= RainingFireLight.Arrow.Radius;
								break;
							case WeaponClass.Crossbow:
								light.Intensity = RainingFireLight.Bolt.Intensity;
								light.Radius	= RainingFireLight.Bolt.Radius;
								break;
							case WeaponClass.ThrowingAxe:
								light.Intensity = RainingFireLight.ThrowingAxe.Intensity;
								light.Radius	= RainingFireLight.ThrowingAxe.Radius;
								break;
							case WeaponClass.ThrowingKnife:
								light.Intensity = RainingFireLight.ThrowingKnife.Intensity;
								light.Radius	= RainingFireLight.ThrowingKnife.Radius;
								break;
							case WeaponClass.Javelin:
								light.Intensity = RainingFireLight.Javelin.Intensity;
								light.Radius	= RainingFireLight.Javelin.Radius;
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

			if (RainingFireCofiguration.rainingFireSwitch && rainingFireTime.rainingfireCheckTime(Campaign.CurrentTime))
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

		private List<RainingFireProjectile> projectiles = new List<RainingFireProjectile>();
    }
}

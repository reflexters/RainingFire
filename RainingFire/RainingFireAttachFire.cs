using System;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using RainingFire.Setting;
using RainingFire.Container;



namespace RainingFire
{
    class RainingFireAttachFire : MissionLogic
    {
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
			List<Agent> deadagents = new List<Agent>();

			foreach (KeyValuePair<Agent, RainingFireObject> keyValuePair in burningObjects)
            {
				Agent key = keyValuePair.Key;
				RainingFireObject value = keyValuePair.Value;
				
				if (key.IsActive())
				{
					if (value.damagecount > 0)
					{
						if (value.burningtime.Check(true))
						{
							Blow blow = CreateBurningBlow(value.attackerAgent, key);
							key.RegisterBlow(blow);
							value.damagecount--;
							if (value.attackerAgent == Agent.Main)
								InformationManager.DisplayMessage(new InformationMessage("getting burning damage " + blow.InflictedDamage));

							if (key == Agent.Main)
								InformationManager.DisplayMessage(new InformationMessage("receiving burning damage " + blow.InflictedDamage));

							if (value.Flame.Count == 0)
							{
								Skeleton attachedAgent = key.AgentVisuals.GetSkeleton();
								MatrixFrame matrixFrame = new MatrixFrame(Mat3.Identity, new Vec3(0f, 0f, 0f, -1f));
								for (int i = 0; i <= (int)HumanBone.HandR; i++)
									value.Flame.Add(ParticleSystem.CreateParticleSystemAttachedToBone("psys_campfire", attachedAgent, (sbyte)i, ref matrixFrame));

								int rval = attachedAgent.GetComponentCount(GameEntity.ComponentType.ParticleSystemInstanced);
							}
						}
					}
     //               else
     //               {
					//	Skeleton attachedAgent = key.AgentVisuals.GetSkeleton();
					//	int rval = attachedAgent.GetComponentCount(GameEntity.ComponentType.ParticleSystemInstanced);
					//}
					
				}
				else
					deadagents.Add(key);
            }

			if(deadagents.Count > 0)
			{ 
				for (int i = deadagents.Count-1; i >= 0; i--)
				{
					burningObjects.Remove(deadagents[i]);
					deadagents.Remove(deadagents[i]);
				}
			}
			//      SpinLock sl = new SpinLock();
			//      bool splock = false;
			//      RainingFireTime rainingFireTime = RainingFireTime.getInstance();

			//      if (RainingFireCofiguration.rainingFireSwitch)
			//      {
			//          sl.Enter(ref splock);
			//          for (int i = 0; i < projectiles.Count; i++)
			//          {
			//              RainingFireProjectile rainingFireProjectile = projectiles[i];
			//              if (rainingFireProjectile.timer.Check(false))
			//              {
			//                  rainingFireProjectile.missile.Entity.RemoveAllParticleSystems();
			//                  Light light = rainingFireProjectile.missile.Entity.GetLight();
			//                  if (light != null)
			//                      rainingFireProjectile.missile.Entity.RemoveComponent(light);

			//                  projectiles.Remove(rainingFireProjectile);
			//              }
			//          }
			//          sl.Exit();
			//      }
		}

        public bool GetRainingFireAvailable()
        {
			RainingFireTime rainingFireTime = RainingFireTime.getInstance();
			float currentTime = Mission.Current.Scene.TimeOfDay;

			return RainingFireCofiguration.rainingFireSwitch && rainingFireTime.rainingfireCheckTime(currentTime) && GetCurrentCombatType();
		}
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
			RainingFireLight rainingFireLight = RainingFireLight.getInstance();

			
			//InformationManager.DisplayMessage(new InformationMessage("Current time is " + Campaign.CurrentTime%24f));
			if (GetRainingFireAvailable())
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
			RainingFireTime time = RainingFireTime.getInstance();
			
			if (GetRainingFireAvailable())
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

						if(attachedAgent.IsHuman && attachedBoneIndex != (int)HumanBone.Forearm1L)
                        {
							RainingFireObject rainingFireObject;
							if(burningObjects.ContainsKey(attachedAgent))
                            {
								rainingFireObject = burningObjects[attachedAgent];
								rainingFireObject.attackerAgent = attackerAgent;
								if (!rainingFireObject.isBurning)
                                {
									rainingFireObject.isBurning = true;
									rainingFireObject.damagecount = time.rainingfireburningtime;
								}
								else
									rainingFireObject.damagecount = time.rainingfireburningtime;
							}
							else
							{
								rainingFireObject = new RainingFireObject();
								rainingFireObject.attackerAgent = attackerAgent;
								rainingFireObject.burningtime = new MissionTimer(time.rainingfireburningtime);
								rainingFireObject.isBurning = true;
								rainingFireObject.damagecount = time.rainingfireburningtime;
								burningObjects.Add(attachedAgent, rainingFireObject);
								
							}
						}
					}
					else
					{
						missile.Entity.RemoveAllParticleSystems();
						MatrixFrame matrixFrame = new MatrixFrame(Mat3.Identity, new Vec3(0f, 0f, 0f, -1f));
						ParticleSystem.CreateParticleSystemAttachedToEntity("psys_campfire", missile.Entity, ref matrixFrame);
						if (light != null)
						{
							light.Intensity = 1f;
							light.Radius = 1f;
						}

						//RainingFireTime rainingFireTime = RainingFireTime.getInstance();
						//RainingFireProjectile rainingFireProjectile = new RainingFireProjectile();
						//rainingFireProjectile.timer = new MissionTimer((float)rainingFireTime.rainingfireburningtime);
						//rainingFireProjectile.missile = missile;
						//splock = false;
						//sl.Enter(ref splock);
						//projectiles.Add(rainingFireProjectile);
						//sl.Exit();
					}
				}
			}
		}

		private Blow CreateBurningBlow(Agent attackerAgent, Agent attachedAgent)
		{
			RainingFireTime time = RainingFireTime.getInstance();
			Blow blow = new Blow(attackerAgent.Index);
			blow.DamageType = DamageTypes.Blunt;
			blow.BlowFlag = BlowFlags.NoSound;
			//blow.BlowFlag |= BlowFlags.ShrugOff;
			blow.BoneIndex = attachedAgent.Monster.HeadLookDirectionBoneIndex;
			blow.Position = attachedAgent.Position;
			blow.Position.z = blow.Position.z + attachedAgent.GetEyeGlobalHeight();
			blow.BaseMagnitude = 0f;
			blow.InflictedDamage = time.rainingfiredamagepersec;
			blow.SwingDirection = attachedAgent.LookDirection;
			blow.SwingDirection.Normalize();
			blow.Direction = blow.SwingDirection;
			blow.DamageCalculated = true;
			return blow;
		}
		//private List<RainingFireProjectile> projectiles = new List<RainingFireProjectile>();
		private Dictionary<Agent, RainingFireObject> burningObjects = new Dictionary<Agent, RainingFireObject>();
	}
}

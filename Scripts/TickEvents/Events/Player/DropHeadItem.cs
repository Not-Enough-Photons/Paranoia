using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class DropHeadItem : ParanoiaEvent
    {
        public override void Start()
        {
            string objName = "[RigManager (Default Brett)]/[SkeletonRig (GameWorld Brett)]/Body/skull/Head/HeadSlotContainer/WeaponReciever";
            GameObject slotObj = GameObject.Find(objName);
            StressLevelZero.Props.Weapons.HandWeaponSlotReciever slot = slotObj.GetComponent<StressLevelZero.Props.Weapons.HandWeaponSlotReciever>();

            if (slot.m_WeaponHost != null)
            {
                slot.GetHost().DisableColliders();
                slot.DropWeapon();
            }
        }
    }
}

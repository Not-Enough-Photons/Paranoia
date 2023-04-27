using UnityEngine;

using SLZ.Rig;

using NEP.Paranoia.Entities;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class Paralysis : ParanoiaEvent
    {
        private PhysicsRig physicsRig;

        public override void Start()
        {
            physicsRig = Object.FindObjectOfType<PhysicsRig>();
            MelonLoader.MelonCoroutines.Start(CoParalysisRoutine());
        }

        private void FreezePlayer(PhysicsRig rig, bool freeze)
        {
            Rigidbody pelvis = rig.torso.rbPelvis;
            Rigidbody feet = rig.rbFeet;
            Rigidbody leftHand = rig.leftHand.rb;
            Rigidbody rightHand = rig.rightHand.rb;

            feet.isKinematic = freeze;
            pelvis.isKinematic = freeze;
            leftHand.isKinematic = freeze;
            rightHand.isKinematic = freeze;
        }

        private System.Collections.IEnumerator CoParalysisRoutine()
        {
            FreezePlayer(physicsRig, true);

            yield return new WaitForSeconds(Random.Range(1f, 6f));

            new SpawnParalyzer().Start();

            BaseMirage paralyzer = Utilities.GetMirage("Paralyzer");

            while (paralyzer.isActiveAndEnabled)
            {
                yield return null;
            }

            FreezePlayer(physicsRig, false);

            yield return null;
        }
    }
}

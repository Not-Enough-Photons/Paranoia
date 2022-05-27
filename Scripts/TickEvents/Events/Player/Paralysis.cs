using UnityEngine;

using StressLevelZero.Rig;

using NEP.Paranoia.Managers;
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
            StressLevelZero.VRMK.PhysBody physBody = rig.physBody;

            physBody.rbFeet.isKinematic = freeze;
            physBody.rbPelvis.isKinematic = freeze;
            rig.leftHand.rb.isKinematic = freeze;
            rig.rightHand.rb.isKinematic = freeze;
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

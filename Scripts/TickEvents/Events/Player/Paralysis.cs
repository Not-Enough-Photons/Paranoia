using UnityEngine;

using StressLevelZero.Rig;

using NotEnoughPhotons.Paranoia.TickEvents.Mirages;
using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class Paralysis : ParanoiaEvent
    {
        private PhysicsRig physicsRig;

        public override void Start()
        {
            physicsRig = Object.FindObjectOfType<PhysicsRig>();

            new AmbientParalyzerSpawn().Start();
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
            ParanoiaGameManager.instance.paralysisMode = true;
            FreezePlayer(physicsRig, true);

            while (ParanoiaGameManager.instance.hParalyzer.isActiveAndEnabled)
            {
                yield return null;
            }

            ParanoiaGameManager.instance.paralysisMode = false;
            FreezePlayer(physicsRig, false);

            yield return null;
        }
    }
}

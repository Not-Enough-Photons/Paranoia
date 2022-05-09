using UnityEngine;

using StressLevelZero.Rig;

using NEP.Paranoia.TickEvents.Mirages;
using NEP.Paranoia.Managers;

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
            /*Paranoia.instance.gameManager.paralysisMode = true;
            FreezePlayer(physicsRig, true);

            yield return new WaitForSeconds(Random.Range(1f, 6f));

            new AmbientParalyzerSpawn().Start();

            while (GameManager.hParalyzer.isActiveAndEnabled)
            {
                yield return null;
            }

            //Paranoia.instance.gameManager.paralysisMode = false;
            FreezePlayer(physicsRig, false);*/

            yield return null;
        }
    }
}

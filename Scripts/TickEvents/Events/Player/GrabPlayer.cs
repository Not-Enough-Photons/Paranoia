using NEP.Paranoia.Utilities;
using StressLevelZero.Rig;
using StressLevelZero.VRMK;

using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class GrabPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            // Get physics rig
            PhysicsRig rig = ParanoiaUtilities.GetRig<PhysicsRig>();
            PhysBody physBody = rig.physBody;

            Rigidbody[] rbs = new Rigidbody[]
            {
                rig.leftHand.rb,
                rig.rightHand.rb,
                physBody.rbHead,
                physBody.rbPelvis,
                physBody.rbKnee
            };

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rbs[Random.Range(0, rbs.Length)]));
        }

        private System.Collections.IEnumerator CoGrabRoutine(Rigidbody part)
        {
            string originalText = ParanoiaMapUtilities.clipboardText.text;

            ParanoiaMapUtilities.SetClipboardText("DO NOT LET IT GRAB YOU            " +
                " RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN " +
                "RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN ");

            yield return new WaitForSeconds(Random.Range(15f, 30f));

            float timer = 0f;

            // Insert grab sound effect here.
            // AudioSource.PlayClipAtPoint(Paranoia.main.grabSounds[Random.Range(0, Paranoia.main.grabSounds.Length)], part.position);

            yield return new WaitForSeconds(2f);

            while(timer < 7f)
            {
                part.AddForce(Vector3.up + (Random.onUnitSphere * 10f) * Random.Range(250f, 1250f), ForceMode.Force);
                yield return null;
            }

            ParanoiaMapUtilities.SetClipboardText(originalText);
        }
    }
}

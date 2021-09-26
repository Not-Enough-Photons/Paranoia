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
            PhysicsRig rig = ParanoiaUtilities.GetPhysicsRig();
            PhysBody physBody = rig.physBody;

            Rigidbody[] rbs = new Rigidbody[]
            {
                rig.leftHand.rb,
                rig.rightHand.rb
            };

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rbs[Random.Range(0, rbs.Length)]));
        }

        private System.Collections.IEnumerator CoGrabRoutine(Rigidbody part)
        {
            string originalText = ParanoiaMapUtilities.clipboardText.text;

            ParanoiaMapUtilities.SetClipboardText("DO NOT LET IT GRAB YOU            " +
                " RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN " +
                "RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN RUN ");

            yield return new WaitForSeconds(15f);

            float timer = 0f;

            // Insert grab sound effect here.
            AudioSource.PlayClipAtPoint(Paranoia.instance.grabSounds[Random.Range(0, Paranoia.instance.grabSounds.Count)], part.position);

            yield return new WaitForSeconds(2f);

            Vector3 dir = Vector3.up + (Random.onUnitSphere * 10f);
            float force = Random.Range(50f, 100f);

            while (timer < 7f)
            {
                timer += Time.deltaTime;

                part.AddForce(dir * force, ForceMode.Acceleration);
                yield return null;
            }

            ParanoiaMapUtilities.SetClipboardText(originalText);
        }
    }
}

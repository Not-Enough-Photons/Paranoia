using System.Collections;
using NEP.Paranoia.ParanoiaUtilities;
using SLZ.Rig;
using SLZ.VRMK;

using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class GrabPlayer : ParanoiaEvent
    {
        public override void Start()
        {
            // Get physics rig
            PhysicsRig rig = Utilities.GetPhysicsRig();

            Rigidbody[] rbs = new Rigidbody[]
            {
                rig.leftHand.rb,
                rig.rightHand.rb
            };

            MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rig, rbs[Random.Range(0, rbs.Length)]));
        }

        private IEnumerator CoGrabRoutine(PhysicsRig physicsRig, Rigidbody part)
        {
            float beforeTimer = 0f;

            while(beforeTimer <= 15f)
            {
                beforeTimer += Time.deltaTime;
            }

            float timer = 0f;

            // Insert grab sound effect here.

            float grabDelay = 0f;

            while(grabDelay <= 2f)
            {
                grabDelay += Time.deltaTime;
            }

            Vector3 dir = Vector3.up + (Random.onUnitSphere * 10f);
            float force = Random.Range(50f, 100f);

            while (timer < 7f)
            {
                timer += Time.deltaTime;

                part.AddForce(dir * force, ForceMode.Acceleration);
                yield return null;
            }
        }
    }
}

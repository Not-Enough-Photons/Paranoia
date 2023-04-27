using SLZ.Rig;
using SLZ.Interaction;

using UnityEngine;

using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class HandTremble : ParanoiaEvent
    {
        public override void Start()
        {
            PhysicsRig physicsRig = BoneLib.Player.physicsRig;

            Rigidbody leftHand = physicsRig?.leftHand.rb;
            Rigidbody rightHand = physicsRig?.rightHand.rb;

            if(leftHand == null || rightHand == null) { return; }

            MelonLoader.MelonCoroutines.Start(CoShakeHands(Random.Range(10, 20f), leftHand, rightHand));
        }

        private System.Collections.IEnumerator CoShakeHands(float duration, Rigidbody leftHand, Rigidbody rightHand)
        {
            float time = 0f;

            while(time < duration)
            {
                time += Time.deltaTime;

                float rand = Random.Range(0.05f, 0.15f);

                leftHand.AddForce(Random.rotation.eulerAngles * rand);
                rightHand.AddForce(Random.rotation.eulerAngles * rand);

                leftHand.AddTorque(Random.rotation.eulerAngles * rand);
                rightHand.AddTorque(Random.rotation.eulerAngles * rand);

                yield return null;
            }

            yield return null;
        }
    }
}

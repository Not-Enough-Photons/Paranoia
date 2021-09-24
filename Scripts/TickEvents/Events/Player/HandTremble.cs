using StressLevelZero.Rig;
using StressLevelZero.Interaction;

using UnityEngine;

using NEP.Paranoia.Utilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class HandTremble : ParanoiaEvent
    {
        public override void Start()
        {
            PhysicsRig physicsRig = ParanoiaUtilities.GetRig<PhysicsRig>();

            Rigidbody leftHand = physicsRig?.leftHand.rb;
            Rigidbody rightHand = physicsRig?.rightHand.rb;

            if(leftHand == null || rightHand == null) { return; }

            MelonLoader.MelonCoroutines.Start(CoShakeHands(Random.Range(3f, 8f), leftHand, rightHand));
        }

        private System.Collections.IEnumerator CoShakeHands(float duration, Rigidbody leftHand, Rigidbody rightHand)
        {
            float time = 0f;

            while(time < duration)
            {
                time += Time.deltaTime;

                leftHand.AddTorque(Random.rotation.eulerAngles * Random.Range(5f, 25f));
                rightHand.AddTorque(Random.rotation.eulerAngles * Random.Range(5f, 25f));

                yield return null;
            }

            yield return null;
        }
    }
}

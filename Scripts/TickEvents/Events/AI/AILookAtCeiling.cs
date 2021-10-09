using RealisticEyeMovements;

using NEP.Paranoia.ParanoiaUtilities;
using NEP.Paranoia.Managers;

using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class AILookAtCeiling : ParanoiaEvent
    {
        public override void Start()
        {
            LookTargetController[] lookTargets = ParanoiaUtilities.Utilities.GetLookAtControllers();

            if(lookTargets == null) { return; }

            foreach(LookTargetController lookTarget in lookTargets)
            {
                lookTarget.LookAtPoiDirectly(Vector3.up * 200f, Random.Range(60f, 120f), 0f);
            }
        }
    }
}

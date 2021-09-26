using NEP.Paranoia.Utilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ClonePlayer : ParanoiaEvent
    {
        public override void Start()
        {
            for(int i = 0; i < 10; i++)
            {
                GameObject rigObject = null;
                ParanoiaUtilities.ClonePlayerBody(out rigObject, rigObject.transform.forward * i, Quaternion.identity);
            }
        }
    }
}

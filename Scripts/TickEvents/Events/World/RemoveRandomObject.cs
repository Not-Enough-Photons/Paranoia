using NEP.Paranoia.Utilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class RemoveRandomObject : ParanoiaEvent
    {
        public override void Start()
        {
            Transform head = Utilities.Utilities.FindHead();
            Rigidbody[] rbs = Utilities.Utilities.FindObjectsBehindHead(head, "Interactable");

            if(rbs == null) { return; }

            rbs[Random.Range(0, rbs.Length)].gameObject.SetActive(false);
        }
    }
}

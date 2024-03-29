﻿using NEP.Paranoia.ParanoiaUtilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class RemoveRandomObject : ParanoiaEvent
    {
        public override void Start()
        {
            Transform head = Utilities.FindHead();
            Rigidbody[] rbs = Utilities.FindObjectsBehindHead(head, "Interactable");

            if(rbs == null) { return; }

            rbs[Random.Range(0, rbs.Length)].gameObject.SetActive(false);
        }
    }
}

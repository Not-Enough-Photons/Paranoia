using UnityEngine;
using NEP.Paranoia.Utilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FlingRandomObject : ParanoiaEvent
    {
        public override void Start()
        {
            Rigidbody[] rbs = Object.FindObjectsOfType<Rigidbody>();
            Transform player = Utilities.Utilities.FindPlayer();

            Rigidbody randomRB = rbs[Random.Range(0, rbs.Length)];

            randomRB.AddForce((player.position - randomRB.transform.position) * Random.Range(100f, 200f), ForceMode.Impulse);
        }
    }
}

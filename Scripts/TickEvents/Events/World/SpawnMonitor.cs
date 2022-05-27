using NEP.Paranoia.ParanoiaUtilities;
using NEP.Paranoia.Managers;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnMonitor : ParanoiaEvent
    {
        public override void Start()
        {
            /*GameObject monitorClone = Paranoia.instance.gameManager.monitorObject;

            monitorClone.transform.position = Paranoia.instance.gameManager.playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            monitorClone.transform.LookAt(ParanoiaUtilities.Utilities.FindPlayer());

            monitorClone.SetActive(true);

            Paranoia.instance.gameManager.insanity += 0.25f;*/
        }
    }
}

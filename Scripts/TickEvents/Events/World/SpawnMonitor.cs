using NEP.Paranoia.Utilities;
using NEP.Paranoia.Managers;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnMonitor : ParanoiaEvent
    {
        public override void Start()
        {
            GameObject monitorClone = ParanoiaGameManager.instance.monitorObject;

            monitorClone.transform.position = ParanoiaGameManager.instance.playerCircle.CalculatePlayerCircle(Random.Range(0, 360), 10f);

            monitorClone.transform.LookAt(ParanoiaUtilities.FindPlayer());

            monitorClone.SetActive(true);

            ParanoiaGameManager.instance.insanity++;
        }
    }
}

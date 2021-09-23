using UnityEngine;
using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class AmbientParalyzerSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.hParalyzer.gameObject.SetActive(true);
        }
    }
}

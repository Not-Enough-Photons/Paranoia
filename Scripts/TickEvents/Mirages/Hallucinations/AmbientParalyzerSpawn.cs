using UnityEngine;
using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class AmbientParalyzerSpawn : ParanoiaEvent
    {
        public AmbientParalyzerSpawn(string hallucinationName)
        {

        }

        public override void Start()
        {
            ParanoiaGameManager.instance.hParalyzer.gameObject.SetActive(true);
        }
    }
}

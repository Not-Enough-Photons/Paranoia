using System.Reflection;

using NEP.Paranoia.Managers;
using NEP.Paranoia.Entities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnMirage : ParanoiaEvent
    {
        private BaseHallucination spawnEnt;

        public SpawnMirage(BaseHallucination hallucination)
        {
            spawnEnt = hallucination;
        }

        public override void Start()
        {
            spawnEnt.gameObject.SetActive(true);
        }
    }
}

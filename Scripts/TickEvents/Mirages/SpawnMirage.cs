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
            MelonLoader.MelonLogger.Msg($"Trying to spawn the hallucination");
            spawnEnt = hallucination;

            MelonLoader.MelonLogger.Msg($"Attempted to spawn {hallucination.name}");
        }

        public override void Start()
        {
            spawnEnt.gameObject.SetActive(true);
        }
    }
}

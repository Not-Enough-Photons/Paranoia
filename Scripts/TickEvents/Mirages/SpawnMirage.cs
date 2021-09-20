using System.Reflection;

using NEP.Paranoia.Managers;
using NEP.Paranoia.Entities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnMirage : ParanoiaEvent
    {
        public SpawnMirage(BaseHallucination hallucination)
        {
            hallucination.gameObject.SetActive(true);
        }
    }
}

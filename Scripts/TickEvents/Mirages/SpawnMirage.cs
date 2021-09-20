using System.Reflection;

using NEP.Paranoia.Managers;
using NEP.Paranoia.Entities;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class SpawnMirage : ParanoiaEvent
    {
        public SpawnMirage(string name)
        {
            ParanoiaGameManager manager = ParanoiaGameManager.instance;

            FieldInfo info = manager.GetType().GetField(name);
            BaseHallucination hallucination = info.GetValue(null) as BaseHallucination;

            hallucination.gameObject.SetActive(true);
        }
    }
}

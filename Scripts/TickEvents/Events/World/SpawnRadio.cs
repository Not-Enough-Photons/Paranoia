using MelonLoader;
using NEP.Paranoia.ParanoiaUtilities;
using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnRadio : ParanoiaEvent
    {
        public override void Start()
        {
            Utilities.GetMirage("Radio").gameObject.SetActive(true);
            new MoveAIToRadio().Start();
        }
    }
}

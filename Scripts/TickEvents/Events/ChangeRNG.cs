using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class ChangeRNG : ParanoiaEvent
    {
        public override void Start()
        {
            //ParanoiaGameManager.instance.SetRNG(UnityEngine.Random.Range(25, 300));
        }
    }
}

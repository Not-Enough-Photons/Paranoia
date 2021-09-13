using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Mirages
{
    public class AmbientDarkVoiceSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            ParanoiaGameManager.instance.hDarkVoice.gameObject.SetActive(true);
        }
    }
}

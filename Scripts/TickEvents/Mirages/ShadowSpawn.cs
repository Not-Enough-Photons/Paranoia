using NotEnoughPhotons.Paranoia.Managers;

namespace NotEnoughPhotons.Paranoia.TickEvents.Mirages
{
    public class ShadowSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            int rng = ParanoiaGameManager.instance.rng;

            if (rng >= 25 || rng <= 30)
            {
                ParanoiaGameManager.instance.hShadowPersonChaser.gameObject.SetActive(true);
            }
            else
            {
                ParanoiaGameManager.instance.hShadowPerson.gameObject.SetActive(true);
            }
        }
    }
}

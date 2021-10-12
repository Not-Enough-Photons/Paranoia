using NEP.Paranoia.Managers;

namespace NEP.Paranoia.TickEvents.Mirages
{
    public class ShadowSpawn : ParanoiaEvent
    {
        public override void Start()
        {
            int rng = Paranoia.instance.gameManager.rng;

            if (rng >= 25 || rng <= 30)
            {
                ParanoiaGameManager.hShadowPersonChaser.gameObject.SetActive(true);
            }
            else
            {
                ParanoiaGameManager.hShadowPerson.gameObject.SetActive(true);
            }
        }
    }
}

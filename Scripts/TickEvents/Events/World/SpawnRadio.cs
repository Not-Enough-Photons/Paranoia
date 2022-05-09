using MelonLoader;
using ModThatIsNotMod.RandomShit;
using NEP.Paranoia.ParanoiaUtilities;
using NEP.Paranoia.Managers;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnRadio : ParanoiaEvent
    {
        public override void Start()
        {
            new MoveAIToRadio().Start();
            //GameManager.hRadio.gameObject.SetActive(true);
        }
    }
}

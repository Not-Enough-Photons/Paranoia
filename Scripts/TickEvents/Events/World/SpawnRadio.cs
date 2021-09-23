using MelonLoader;
using ModThatIsNotMod.RandomShit;
using NEP.Paranoia.Utilities;
using NEP.Paranoia.Managers;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class SpawnRadio : ParanoiaEvent
    {
        public override void Start()
        {
            new MoveAIToRadio().Start();
            ParanoiaGameManager.hRadio.gameObject.SetActive(true);
        }
    }
}

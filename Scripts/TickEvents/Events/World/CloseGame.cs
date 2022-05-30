using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class CloseGame : ParanoiaEvent
    {
        public override void Start()
        {
            Application.Quit();
        }
    }
}

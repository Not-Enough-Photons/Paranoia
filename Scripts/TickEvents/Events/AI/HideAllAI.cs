using NEP.Paranoia.Utilities;
using StressLevelZero.AI;

namespace NEP.Paranoia.TickEvents.Events
{
    public class HideAllAI : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = Utilities.Utilities.FindAIBrains();

            if(brains == null) { return; }

            foreach(AIBrain brain in brains)
            {
                brain.gameObject.SetActive(false);
            }
        }
    }
}

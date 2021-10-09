using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.AI;

namespace NEP.Paranoia.TickEvents.Events
{
    public class HideAllAI : ParanoiaEvent
    {
        public override void Start()
        {
            AIBrain[] brains = ParanoiaUtilities.Utilities.FindAIBrains();

            if(brains == null) { return; }

            foreach(AIBrain brain in brains)
            {
                brain.gameObject.SetActive(false);
            }
        }
    }
}

using Paranoia.Helpers;
using PuppetMasta;
using Random = UnityEngine.Random;

namespace Paranoia.Events
{
    public static class LaughAtPlayer
    {
#if MELONLOADER
        public static void Activate()
        {
            var brains = Utilities.FindAIBrains();

            foreach(var brain in brains)
            {
                if(brain == null)
                {
                    continue;
                }

                var powerLegs = brain.gameObject.GetComponentInChildren<BehaviourPowerLegs>();

                if(!powerLegs)
                {
                    return;
                }

                powerLegs.faceAnim.Attack1(Random.Range(1, 3));
            }
        }
#else
        public static void Activate()
        {

        }
#endif
    }
}
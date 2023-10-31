using Paranoia.Helpers;
using PuppetMasta;
using Random = UnityEngine.Random;

namespace Paranoia.Events
{
    /// <summary>
    /// Makes all NPCs laugh at the player.
    /// </summary>
    public static class LaughAtPlayer
    {
        public static void Activate()
        {
            var brains = Utilities.FindAIBrains();

            foreach(var brain in brains)
            {
                if (brain == null)
                {
                    ModConsole.Error("Brain is null!");
                    return;
                }

                var powerLegs = brain.behaviour.gameObject.GetComponent<BehaviourPowerLegs>();

                if (powerLegs == null)
                {
                    ModConsole.Error("Power legs is null!");
                    return;
                }
                // TODO: This doesn't work. Maybe it changed in BL?
                powerLegs.faceAnim.Attack1(Random.Range(1, 3));
            }
        }
    }
}
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
        // TODO: Make this work, bonelab must of changed SOMETHING to break this.
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

                var powerLegs = brain.gameObject.GetComponentInChildren<BehaviourPowerLegs>();

                if (powerLegs == null)
                {
                    ModConsole.Error("Power legs is null!");
                    return;
                }

                powerLegs.faceAnim.Attack1(Random.Range(1, 3));
            }
        }
    }
}
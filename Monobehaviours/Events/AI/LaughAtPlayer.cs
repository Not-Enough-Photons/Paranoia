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
    }
}
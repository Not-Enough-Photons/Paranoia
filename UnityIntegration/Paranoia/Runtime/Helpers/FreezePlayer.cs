using UnityEngine;

namespace Paranoia.Helpers
{
    [AddComponentMenu("Paranoia/Helpers/Freeze Player")]
    public class FreezePlayer : ParanoiaEvent
    {
        public override string Comment => "Freezes the player, useful for freezing the player during the end scene.";
        public void Freeze()
        {
            Utilities.FreezePlayer(true);
        }
        
        public void Unfreeze()
        {
            Utilities.FreezePlayer(false);
        }
    }
}
using NEP.Paranoia.ParanoiaUtilities;
using StressLevelZero.Props.Weapons;

namespace NEP.Paranoia.TickEvents.Events
{
    public class FireGun : ParanoiaEvent
    {
        public override void Start()
        {
            // Find a random gun in the scene to fire
            Gun[] guns = UnityEngine.Object.FindObjectsOfType<Gun>();

            if(guns == null) { return; }

            guns[UnityEngine.Random.Range(0, guns.Length)]?.Fire();
        }
    }
}

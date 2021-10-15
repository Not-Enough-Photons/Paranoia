using UnityEngine;
using NEP.Paranoia.ParanoiaUtilities;

namespace NEP.Paranoia.TickEvents.Events
{
    public class ChangeHoloSign : ParanoiaEvent
    {
        public override void Start()
        {
            if(MapUtilities.collectAllSign == null) { return; }
            if(!MapUtilities.collectAllSign.active) { return; }

            MapUtilities.ChangeHoloSign(MapUtilities.collectAllSign, Paranoia.instance.GetTextureInList("level_museumbasement_holosign_01"));
            MelonLoader.MelonCoroutines.Start(CoHideRoutine());
        }

        private System.Collections.IEnumerator CoHideRoutine()
        {
            yield return new WaitForSeconds(10f);
            MapUtilities.collectAllSign.SetActive(false);
        }
    }
}

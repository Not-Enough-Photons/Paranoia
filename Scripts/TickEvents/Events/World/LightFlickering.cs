using StressLevelZero.Rig;
using NEP.Paranoia.Managers;
using NEP.Paranoia.Utilities;
using UnityEngine;

namespace NEP.Paranoia.TickEvents.Events
{
    public class LightFlickering : ParanoiaEvent
    {
        public override void Start()
        {
            new DisableNimbus().Start();
            new DisableWasp().Start();

            //MelonLoader.MelonCoroutines.Start(CoLightFlicker(Random.Range(1, 10)));
        }

        public override void Stop()
        {
            base.Stop();
        }

        private System.Collections.IEnumerator CoLightFlicker(int iterations)
        {
            GameObject staticCeiling = ParanoiaMapUtilities.staticCeiling;
            GameObject mainLight = ParanoiaMapUtilities.mainLight;

            for(int i = 0; i < iterations; i++)
            {
                yield return new WaitForSeconds(0.05f);

                FlickerLightmaps();
                FlickerStaticGrids(ParanoiaMapUtilities.staticPlaneMaterials);
                FlickerCeiling(staticCeiling);
                FlickerFlashlights();

                DynamicGI.UpdateEnvironment();
            }

            yield return null;
        }

        private PropFlashlight[] ScanFlashlights()
        {
            if(Object.FindObjectsOfType<PropFlashlight>() != null)
            {
                return Object.FindObjectsOfType<PropFlashlight>();
            }

            return null;
        }

        private void FlickerStaticGrids(Material[] gridMats)
        {
            for(int i = 0; i < gridMats.Length; i++)
            {
                Material mat = gridMats[i];

                bool isZero = i % 2 == 0;

                mat.color = isZero ? Color.black : ParanoiaMapUtilities.staticPlaneMaterials[i].color;
                mat.SetFloat("g_flCubeMapScalar", 0.0f);
            }
        }

        private void FlickerLightmaps()
        {
            LightmapSettings.lightmaps = new LightmapData[0];
            LightmapSettings.lightProbes.bakedProbes = null;
        }

        private void FlickerCeiling(GameObject ceiling)
        {
            ceiling.active = !ceiling.active;
        }
        
        private void FlickerFlashlights()
        {
            if(ScanFlashlights() == null) { return; }

            PropFlashlight[] flashlights = ScanFlashlights();

            foreach(PropFlashlight flashlight in flashlights)
            {
                flashlight.SwitchLight();
            }
        }
    }
}

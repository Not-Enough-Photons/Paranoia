using StressLevelZero.Rig;
using NotEnoughPhotons.Paranoia.Managers;
using UnityEngine;

namespace NotEnoughPhotons.Paranoia.TickEvents.Events
{
    public class LightFlickering : ParanoiaEvent
    {
        public override void Start()
        {
            new DisableNimbus().Start();
            new DisableWasp().Start();
            MelonLoader.MelonCoroutines.Start(CoLightFlicker());
        }

        public override void Stop()
        {
            base.Stop();
        }

        private System.Collections.IEnumerator CoLightFlicker()
        {
            int iterations = Random.Range(2, 5);
            int rng = ParanoiaGameManager.instance.rng;
            int i = 0;
            bool isOn = false;
            float random = 0f;

            if (GameObject.FindObjectsOfType<VLB.VolumetricLightBeam>() != null)
            {
                Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>();
                VLB.VolumetricLightBeam[] lightbeams = GameObject.FindObjectsOfType<VLB.VolumetricLightBeam>();

                Light blankBoxLight = GameObject.FindObjectOfType<CustomLightMachine>().light;

                foreach (VLB.VolumetricLightBeam lightbeam in lightbeams)
                {
                    for (i = 0; i < iterations; i++)
                    {
                        yield return new WaitForSeconds(0.05f);

                        random = Random.Range(1, iterations);

                        if (blankBoxLight != null || ParanoiaGameManager.instance.lightBeam != null)
                        {
                            if ((i * random / 2) * rng % 2 == 0)
                            {
                                isOn = false;
                                blankBoxLight.gameObject.SetActive(false);
                                LightmapSettings.lightmaps = new LightmapData[0];
                                LightmapSettings.lightProbes.bakedProbes = null;
                            }
                            else
                            {
                                isOn = true;
                                blankBoxLight.gameObject.SetActive(true);
                                LightmapSettings.lightmaps = ParanoiaGameManager.instance.lightmaps;
                                LightmapSettings.lightProbes.bakedProbes = new UnhollowerBaseLib.Il2CppStructArray<UnityEngine.Rendering.SphericalHarmonicsL2>(ParanoiaGameManager.instance.bakedProbes.Count);
                                LightmapSettings.lightProbes.bakedProbes = ParanoiaGameManager.instance.bakedProbes;
                            }

                            DynamicGI.UpdateEnvironment();
                        }
                    }
                }
            }

            if (!isOn)
            {
                ParanoiaGameManager.instance.SetIsDark(true);
            }
            
            yield return null;
        }
    }
}

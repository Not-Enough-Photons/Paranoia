namespace NEP.Paranoia.Events;

/// <summary>
/// Flicker all flashlights in the scene.
/// </summary>
public class FlickerFlashlights : Event
{
    public override void Invoke()
    {
        PropFlashlight[] flashlights = Object.FindObjectsOfType<PropFlashlight>();

        foreach (var t in flashlights)
        {
            ModStats.IncrementEntry("LightsFlickered");
            MelonLoader.MelonCoroutines.Start(CoLightFlicker(t, Random.Range(15, 25)));
        }
    }

    private static IEnumerator CoLightFlicker(PropFlashlight light, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.10f));
            light.SwitchLight();
        }
    }
}
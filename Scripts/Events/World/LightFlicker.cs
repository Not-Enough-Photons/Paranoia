namespace NEP.Paranoia.Scripts.Events;

/// <summary>
/// Flickers the given lights by toggling the gameobject over and over.
/// <br/>Parent your lights to the gameobject given to this script.
/// </summary>
public class LightFlicker : Event
{
    public override void Invoke()
    {
        var lights = ParanoiaManager.Instance.eventSettings.lights;
        if(lights == null) { return; }
        ModStats.IncrementEntry("LightsFlickered");
        MelonCoroutines.Start(CoLightFlicker(lights, Random.Range(30, 45)));
    }
    
    public override bool CanInvoke()
    {
        return ParanoiaManager.Instance.managerType == ManagerType.Paranoia;
    }

    private static IEnumerator CoLightFlicker(Light[] lights, int iterations)
    {
        for(var i = 0; i < iterations; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            foreach (var light in lights)
            {
                light.gameObject.SetActive(i % 2 == 0);
            }
        }
    }
}
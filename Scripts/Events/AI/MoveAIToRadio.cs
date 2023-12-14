using NEP.Paranoia.Helpers;

namespace NEP.Paranoia.Events;

/// <summary>
/// Spawns a radio and sends all AI to the radio.
/// </summary>
public class MoveAIToRadio : Event
{
    private static GameObject _radioObj;
        
    public override void Invoke()
    {
        var radiopos = ParanoiaManager.Instance.groundSpawns[Random.Range(0, ParanoiaManager.Instance.groundSpawns.Length)].position;
        var radio = new SpawnableCrateReference(Pallet.Entities.Radio);
        HelperMethods.SpawnCrate(radio, radiopos, Quaternion.identity, Vector3.one, false, go =>
        {
            _radioObj = go;
        });
            
        Utilities.FindAIBrains(out var navs);

        if (_radioObj == null) return;

        foreach (var nav in navs)
        {
            Utilities.MoveAIToPoint(nav, _radioObj.transform.position);
        }

        _radioObj = null;
    }
}
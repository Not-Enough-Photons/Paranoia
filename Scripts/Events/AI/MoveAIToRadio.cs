﻿namespace Paranoia.Events;

/// <summary>
/// Spawns a radio and sends all AI to the radio.
/// </summary>
public static class MoveAIToRadio
{
    private static GameObject _radioObj;
        
    public static void Activate(Transform location)
    {
        var radio = new SpawnableCrateReference(Pallet.Entities.Radio);
        HelperMethods.SpawnCrate(radio, location.position, Quaternion.identity, Vector3.one, false, go =>
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
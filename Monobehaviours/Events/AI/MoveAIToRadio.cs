using Paranoia.Helpers;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace Paranoia.Events
{
    public static class MoveAIToRadio
    {
        #if MELONLOADER
        private static GameObject _radioObj;
        
        public static void Activate(string barcode, Transform location)
        {
            var radio = new SpawnableCrateReference(barcode);
            Warehouse.Spawn(radio, location.position, Quaternion.identity, false, go =>
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
#else
        private static GameObject _radioObj;

        public static void Activate(string barcode, Transform location)
        {

        }
#endif
    }
}
#if MELONLOADER
using System;
#endif
using UnityEngine;

namespace Paranoia.Helpers
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Helpers/Freeze Player")]
#endif
    public class FreezePlayer : MonoBehaviour
    {
        public void Freeze()
        {
            Utilities.FreezePlayer(true);
        }
#if MELONLOADER
        public FreezePlayer(IntPtr ptr) : base(ptr) { }
#endif
    }
}
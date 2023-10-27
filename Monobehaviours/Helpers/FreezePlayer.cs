using System;
using UnityEngine;

namespace Paranoia.Helpers
{
    public class FreezePlayer : MonoBehaviour
    {
        public void Freeze()
        {
            Utilities.FreezePlayer(true);
        }

        public void Unfreeze()
        {
            Utilities.FreezePlayer(false);
        }

        public FreezePlayer(IntPtr ptr) : base(ptr) { }
    }
}
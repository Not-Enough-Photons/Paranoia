using System;
using UnityEngine;

namespace Paranoia.Managers
{
#if UNITY_EDITOR
    [AddComponentMenu("Paranoia/Managers/Clip Holder")]
#endif
    public class ClipHolder : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip("The list of audio clips that might play.")]
#endif
        public AudioClip[] clips;
#if MELONLOADER
        public ClipHolder(IntPtr ptr) : base(ptr) { }
#endif
    }
}
using System;
using UnityEngine;

namespace Paranoia
{
    public class ParanoiaEvent : MonoBehaviour
    {
        public ParanoiaEvent(IntPtr ptr) : base(ptr) { }
        private Transform _transform;
        private bool _hasTransform = false;

        public Transform Transform
        {
            get
            {
                if (!_hasTransform)
                {
                    _transform = transform;
                    _hasTransform = true;
                }
                return _transform;
            }
        }
    }
}
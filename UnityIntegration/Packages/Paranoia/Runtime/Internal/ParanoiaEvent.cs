using UnityEngine;

namespace NEP.Paranoia
{
    public abstract class ParanoiaEvent : MonoBehaviour
    {
        public virtual string Comment => null;
        public virtual string Warning => null;
    }
}
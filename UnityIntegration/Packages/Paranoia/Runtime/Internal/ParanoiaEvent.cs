using UnityEngine;

namespace NEP.Paranoia.Scripts.InternalBehaviours
{
    public abstract class ParanoiaEvent : MonoBehaviour
    {
        public virtual string Comment => null;
        public virtual string Warning => null;
    }
}
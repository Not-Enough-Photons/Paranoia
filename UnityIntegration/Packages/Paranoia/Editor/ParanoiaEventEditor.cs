using NEP.Paranoia.Scripts.InternalBehaviours;
using UnityEngine;
using UnityEditor;

namespace NEP.Paranoia.Editor
{
    [CustomEditor(typeof(ParanoiaEvent), editorForChildClasses: true)]
    public class ParanoiaEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var behaviour = target as ParanoiaEvent;
            if (behaviour.Comment != null)
            {
                EditorGUILayout.HelpBox(behaviour.Comment, MessageType.Info);
            }
            if (behaviour.Warning != null)
            {
                EditorGUILayout.HelpBox(behaviour.Warning, MessageType.Warning);
            }
            base.OnInspectorGUI();
        }
    }
}
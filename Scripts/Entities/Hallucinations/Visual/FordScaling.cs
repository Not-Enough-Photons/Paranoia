using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NEP.Paranoia.Entities
{
    public class FordScaling : BaseHallucination
    {
        public FordScaling(System.IntPtr ptr) : base(ptr) { }

        public enum FaceState
        {
            Default,
            Happy,
            Angry,
            Sad,
            Distorted,
            WideEyed
        }

        private FaceState _faceState;
        public FaceState faceState { get => _faceState; }

        private SkinnedMeshRenderer meshRenderer;

        private Dictionary<int, float> wideEyedFlexes = new Dictionary<int, float>()
        {
            { 5, -300f },
            { 6, -300f },
            { 9, 300f },
            { 10, 300f },
        };

        protected override void Awake()
        {
            ReadValuesFromJSON(System.IO.File.ReadAllText(baseJsonPath + "FordScaling.json"));
            meshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();

            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            switch (_faceState)
            {
                case FaceState.Default:
                    break;
                case FaceState.Happy:
                    break;
                case FaceState.Angry:
                    break;
                case FaceState.Sad:
                    break;
                case FaceState.Distorted:
                    break;
                case FaceState.WideEyed:
                    UpdateFaceFlexes(wideEyedFlexes.Keys.ToArray(), wideEyedFlexes.Values.ToArray(), meshRenderer);
                    break;
            }
        }

        private void UpdateFaceFlexes(int[] indexes, float[] values, SkinnedMeshRenderer meshRenderer)
        {
            if(meshRenderer == null) { return; }

            for(int i = 0; i < indexes.Length; i++)
            {
                for(int j = 0; j < values.Length; j++)
                {
                    meshRenderer.SetBlendShapeWeight(indexes[i], values[j]);
                }
            }
        }
    }
}

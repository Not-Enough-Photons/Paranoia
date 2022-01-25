using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace NEP.Paranoia.Entities
{
    public class Paralyzer : AudioHallucination
    {
        public Paralyzer(System.IntPtr ptr) : base(ptr) { }

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
            { 0, 75f },
            { 4, 100f },
            { 5, -600f },
            { 6, -600f },
            { 50, 950f }
        };

        protected override void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            meshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

            base.Awake();

            clips = Paranoia.instance.paralyzerAmbience.ToArray();

            ReadValuesFromJSON(System.IO.File.ReadAllText(audioJsonPath + "Paralyzer.json"));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            source.spatialBlend = 0.75f;

            gameObject.AddComponent<AudioReverbZone>();
        } 

        private void UpdateFaceFlexes(int[] indexes, float[] values, SkinnedMeshRenderer meshRenderer)
        {
            if (meshRenderer == null) { return; }

            for (int i = 0; i < indexes.Length; i++)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    meshRenderer.SetBlendShapeWeight(indexes[i], values[j]);
                }
            }
        }
    }
}

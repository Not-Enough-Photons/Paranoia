using UnityEngine;

namespace Paranoia.Helpers
{
    public static class CameraHelper
    {
        private static void LayerCullingShow(Camera cam, int layerMask)
        {
            cam.cullingMask |= layerMask;
        }

        public static void LayerCullingShow(Camera cam, string layer)
        {
            LayerCullingShow(cam, 1 << LayerMask.NameToLayer(layer));
        }

        private static void LayerCullingHide(Camera cam, int layerMask)
        {
            cam.cullingMask &= ~layerMask;
        }

        public static void LayerCullingHide(Camera cam, string layer)
        {
            LayerCullingHide(cam, 1 << LayerMask.NameToLayer(layer));
        }
        
        private static bool LayerCullingIncludes(Camera cam, int layerMask)
        {
            return (cam.cullingMask & layerMask) > 0;
        }

        public static bool LayerCullingIncludes(Camera cam, string layer)
        {
            return LayerCullingIncludes(cam, 1 << LayerMask.NameToLayer(layer));
        }
    }
}
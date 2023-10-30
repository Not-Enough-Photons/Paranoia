using UnityEngine;

namespace Paranoia.Events
{
    /// <summary>
    /// Handles the events for the museum.
    /// </summary>
    public static class MuseumEvents
    {
        /// <summary>
        /// Changes the sign's texture.
        /// </summary>
        /// <param name="sign">The mesh renderer of the sign</param>
        /// <param name="texture">The texture to change to</param>
        public static void ChangeSign(MeshRenderer sign, Texture2D texture)
        {
            sign.material.mainTexture = texture;
        }
        /// <summary>
        /// Deletes the sign.
        /// </summary>
        /// <param name="sign">The sign to delete</param>
        public static void DeleteSign(MeshRenderer sign)
        {
            Object.Destroy(sign.gameObject);
        }
        /// <summary>
        /// Activates the fog.
        /// </summary>
        /// <param name="globalVolume">The global volume with the fog</param>
        public static void TheFogIsHere(GameObject globalVolume)
        {
            globalVolume.SetActive(true);
        }
        /// <summary>
        /// Deactivates the fog.
        /// </summary>
        /// <param name="globalVolume">The global volume with the fog</param>
        public static void TheFogIsGone(GameObject globalVolume)
        {
            globalVolume.SetActive(false);
        }
    }
}
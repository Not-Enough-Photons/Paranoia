﻿using System;
using MelonLoader;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Paranoia.Entities
{
    /// <summary>
    /// Very similar to AudioEvent, but is seperate for one reason:
    /// <br/>It's used to determine where the radio is in MoveAIToRadio.<see cref="Events.MoveAIToRadio.Activate"/>
    /// </summary>
    public class Radio : MonoBehaviour
    {
        public AudioClip[] audioClips;
        public AudioSource audioSource;

        private void Start()
        {
            ModConsole.Msg("Radio spawned", LoggingMode.DEBUG);
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.Play();
            MelonCoroutines.Start(DespawnSelf(audioSource.clip.length));
        }

        private IEnumerator DespawnSelf(float delay)
        {
            yield return new WaitForSeconds(delay);
            ModConsole.Msg("Radio despawned", LoggingMode.DEBUG);
            Destroy(gameObject);
        }
        
        public Radio(IntPtr ptr) : base(ptr) { }
    }
}
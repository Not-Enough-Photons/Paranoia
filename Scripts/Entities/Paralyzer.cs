﻿using System;
using BoneLib;
using MelonLoader;
using System.Collections;
using Paranoia.Helpers;
using UnityEngine;

namespace Paranoia.Entities
{
    /// <summary>
    /// Freezes the player in place and moves towards them.
    /// </summary>
    public class Paralyzer : MonoBehaviour
    {
        public AudioSource paralysisSound;
        private Transform _player;
        private Transform This => transform;

        private void Start()
        {
            ModConsole.Msg("Paralyzer spawned", LoggingMode.DEBUG);
            _player = Player.physicsRig.m_chest;
            This.position = _player.position + _player.forward * 25f;
            Utilities.FreezePlayer(true);
            paralysisSound.Play();
            This.LookAt(_player);
            MelonCoroutines.Start(MoveCloser());
        }

        private IEnumerator MoveCloser()
        {
            for (var i = 0; i < 3; i++)
            {
                This.position = Vector3.MoveTowards(This.position, _player.position, 5f);
                paralysisSound.Play();
                yield return new WaitForSeconds(5f);
                if (i == 2)
                {
                    This.position = Vector3.MoveTowards(This.position, _player.position, 5f);
                    paralysisSound.Play();
                    MelonCoroutines.Start(DespawnSelf());
                }
            }
        }
        
        private IEnumerator DespawnSelf()
        {
            yield return new WaitForSeconds(5f);
            ModConsole.Msg("Paralyzer despawned", LoggingMode.DEBUG);
            Utilities.FreezePlayer(false);
            Destroy(gameObject);
        }

        public Paralyzer(IntPtr ptr) : base(ptr) { }
    }
}
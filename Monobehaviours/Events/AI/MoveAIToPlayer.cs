﻿using BoneLib;
using Paranoia.Helpers;

namespace Paranoia.Events
{
    public static class MoveAIToPlayer
    {
        public static void Activate()
        {
            Utilities.FindAIBrains(out var navs);
            var player = Player.playerHead;

            if(player == null) { return; }
            if(navs == null) { return; }

            foreach (var nav in navs)
            {
                Utilities.MoveAIToPoint(nav, player.position);
            }
        }
    }
}
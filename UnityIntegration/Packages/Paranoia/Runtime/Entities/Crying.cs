using System.Collections.Generic;
using SLZ.Rig;
using UnityEngine;
using NEP.Paranoia.Scripts.InternalBehaviours;

namespace NEP.Paranoia.Scripts.Entities
{
    [AddComponentMenu("Paranoia/Crying")]
[HelpURL("https://github.com/Not-Enough-Photons/Paranoia/wiki/Entities#crying")]
    public class Crying : ParanoiaEvent
    {
        public override string Comment => "Marks this entity as a cryer. Prevents duplicates from spawning.";
        public override string Warning => "You will have to set up the trigger/audio! Use trigger lasers or something!";
    }
}
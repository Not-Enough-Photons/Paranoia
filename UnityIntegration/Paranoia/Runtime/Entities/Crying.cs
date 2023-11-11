using System.Collections.Generic;
using SLZ.Rig;
using UnityEngine;

namespace Paranoia.Entities
{
    [AddComponentMenu("Paranoia/Crying")]
    public class Crying : ParanoiaEvent
    {
        public override string Comment => "Marks this entity as a cryer. Prevents duplicates from spawning.";
        public override string Warning => "You will have to set up the trigger/audio! Use trigger lasers or something!";
    }
}
using NEP.Paranoia.Helpers;

namespace NEP.Paranoia.Events;

/// <summary>
/// Drags a random NPC to the ceiling.
/// </summary>
public class DragNpcToCeiling : Event
{
    public override void Invoke()
    {
        var clips = ParanoiaManager.Instance.grabSounds;
        var brains = Utilities.FindAIBrains();
        if (brains == null || brains.Length == 0)
        {
            ModConsole.Error("No AI brains found!");
            return;
        }
        var rand = brains[Random.Range(0, brains.Length)];
        if (rand == null)
        {
            ModConsole.Error("Random AI brain is null!");
            return; 
        }
        var physRoot = rand.transform.Find("Physics/Root_M");
        if (physRoot == null)
        {
            ModConsole.Error("Physics root is null!");
            return;
        }
        var targetRB = physRoot.Find("Spine_M/Chest_M/Head_M").GetComponent<Rigidbody>();
        if (targetRB == null)
        {
            ModConsole.Error("Target rigidbody is null!");
            return;
        }
        rand.puppetMaster.muscleWeight = 0f;
        MelonCoroutines.Start(CoGrabRoutine(rand, targetRB, clips));
        ModStats.IncrementEntry("FordsGrabbed");
    }

    private static IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part, AudioClip[] grabClips)
    {
        var timer = 0f;
            
        AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

        yield return new WaitForSeconds(2f);

        var dir = Vector3.up;
        var force = Random.Range(250f, 300f);

        while (timer < 7f)
        {
            timer += Time.deltaTime;

            if(part == null) { break; }

            part.AddForce(dir * force, ForceMode.Acceleration);
            yield return null;
        }

        brain.gameObject.SetActive(false);
    }
}
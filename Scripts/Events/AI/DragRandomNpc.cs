namespace Paranoia.Events;

/// <summary>
/// Drags a random NPC to a random place.
/// </summary>
public static class DragRandomNpc
{
    public static void Activate(AudioClip[] grabClips)
    {
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

        var rbs = new Rigidbody[]
        {
            physRoot.Find("Hip_L/Knee_L").GetComponent<Rigidbody>(),
            physRoot.Find("Hip_R/Knee_R").GetComponent<Rigidbody>(),
            physRoot.Find("Spine_M/Chest_M/Shoulder_L/Elbow_L/Wrist_L").GetComponent<Rigidbody>(),
            physRoot.Find("Spine_M/Chest_M/Shoulder_R/Elbow_R/Wrist_R").GetComponent<Rigidbody>()
        };

        var targetRb = rbs[Random.Range(0, rbs.Length)];

        if (targetRb == null)
        {
            ModConsole.Error("Target rigidbody is null!");
            return;
        }

        MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rand, targetRb, grabClips));
        ModStats.IncrementEntry("FordsGrabbed");
    }

    private static IEnumerator CoGrabRoutine(AIBrain brain, Rigidbody part, AudioClip[] grabClips)
    {
        var timer = 0f;
            
        AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

        yield return new WaitForSeconds(2f);

        var dir = Vector3.up * 0.25f + (-Vector3.right * Random.Range(5f, 50f) + (Vector3.forward * Random.Range(5f, 10f)));
        const float force = 175f;

        while (timer < 7f)
        {
            timer += Time.deltaTime;

            part.AddForce(dir * force, ForceMode.Acceleration);
            yield return null;
        }

        brain.gameObject.SetActive(false);

        yield return null;
    }
}
namespace Paranoia.Events;

/// <summary>
/// Grabs the player and moves them around for a bit.
/// </summary>
public static class GrabPlayer
{
    public static void Activate(AudioClip[] grabClips)
    {
            
        var rig = Player.physicsRig;

        Rigidbody[] rbs = {
            rig.leftHand.rb,
            rig.rightHand.rb
        };

        MelonLoader.MelonCoroutines.Start(CoGrabRoutine(rbs[Random.Range(0, rbs.Length)], grabClips));
    }

    private static IEnumerator CoGrabRoutine(Rigidbody part, AudioClip[] grabClips)
    {
        yield return new WaitForSeconds(15f);

        var timer = 0f;
            
        AudioSource.PlayClipAtPoint(grabClips[Random.Range(0, grabClips.Length)], part.position);

        yield return new WaitForSeconds(2f);

        var dir = Vector3.up + (Random.onUnitSphere * 10f);
        var force = Random.Range(50f, 100f);

        while (timer < 7f)
        {
            timer += Time.deltaTime;

            part.AddForce(dir * force, ForceMode.Acceleration);
            yield return null;
        }
    }
}
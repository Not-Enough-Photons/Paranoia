namespace NEP.Paranoia.Events;

/// <summary>
/// Grabs the player and moves them around for a bit.
/// </summary>
public class GrabPlayer : Event
{
    public override void Invoke()
    {
        var grabClips = ParanoiaManager.Instance.grabSounds;
        var rig = Player.physicsRig;

        Rigidbody[] rbs = {
            rig.leftHand.rb,
            rig.rightHand.rb
        };

        MelonCoroutines.Start(CoGrabRoutine(rbs[Random.Range(0, rbs.Length)], grabClips));
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
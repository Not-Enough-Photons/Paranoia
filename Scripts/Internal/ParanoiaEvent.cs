namespace Paranoia.Internal;

/// <summary>
/// Used for Unity Editor, kept in built code just in case.
/// <br/>You can ignore this.
/// </summary>
public class ParanoiaEvent : MonoBehaviour
{
    public ParanoiaEvent(IntPtr ptr) : base(ptr) { }
    private Transform _transform;
    private bool _hasTransform = false;

    public Transform Transform
    {
        get
        {
            if (!_hasTransform)
            {
                _transform = transform;
                _hasTransform = true;
            }
            return _transform;
        }
    }
}
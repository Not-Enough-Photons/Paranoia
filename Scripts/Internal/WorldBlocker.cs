namespace NEP.Paranoia.Internal;

public class WorldBlocker : MonoBehaviour
{
    public MovementType movementType;
    private Transform _target;
    private Transform This => transform;
    
    private void Start()
    {
        _target = Player.playerHead;
    }

    private void Update()
    {
        switch (movementType)
        {
            case MovementType.XAxis:
                This.position = new Vector3(_target.position.x, transform.position.y, This.position.z);
                break;
            case MovementType.ZAxis:
                This.position = new Vector3(transform.position.x, This.position.y, _target.position.z);
                break;
            default:
                ModConsole.Error("Invalid movement type!");
                break;
        }
    }
    
    public WorldBlocker(IntPtr ptr) : base(ptr) { }
}

public enum MovementType
{
    XAxis,
    ZAxis
}
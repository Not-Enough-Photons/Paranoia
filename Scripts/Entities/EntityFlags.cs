[System.Flags]
public enum EntityFlags
{
    None,
    HideWhenSeen = (1 << 0),
    HideWhenClose = (1 << 1),
    LookAtTarget = (1 << 2),
    Moving = (1 << 3),
    Damaging = (1 << 4),
    DamageThenHide = (1 << 5),
    SpinAroundPlayer = (1 << 6),
    Teleporting = (1 << 7),
    MoveWhenNotSeen = (1 << 8),
    Wait =  (1 << 9)
}
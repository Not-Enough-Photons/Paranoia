[System.Flags]
public enum SpawnFlags
{
    None = 0,
    SpawnAroundPlayer = (1 << 0),
    SpawnAtPoints = (1 << 1),
    LookAtTarget = (1 << 2)
}

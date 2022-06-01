namespace NEP.Paranoia.Entities
{
    [System.Flags]
    public enum SpawnFlags
    {
        None = 0,
        SpawnAroundTarget = (1 << 0),
        SpawnAtPoints = (1 << 1),
        LookAtTarget = (1 << 2)
    }
}
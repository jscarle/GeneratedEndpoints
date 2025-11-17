namespace GeneratedEndpoints.Common;

internal readonly struct CollisionInfo(int firstIndex, bool firstHandlerRenamed)
{
    public int FirstIndex { get; } = firstIndex;

    public bool FirstHandlerRenamed { get; } = firstHandlerRenamed;

    public CollisionInfo WithFirstHandlerRenamed()
    {
        return new CollisionInfo(FirstIndex, true);
    }
}

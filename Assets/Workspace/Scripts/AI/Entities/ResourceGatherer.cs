using System;

public class ResourceGatherer<T> : FactionMember<T> where T : ResourceGatherer<T>
{
    public int AxePower = 10;
}
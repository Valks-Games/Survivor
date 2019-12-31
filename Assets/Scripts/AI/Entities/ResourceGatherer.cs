using UnityEngine;
using System.Linq;

public class ResourceGatherer<T> : FactionMember<T> where T: ResourceGatherer<T>
{
    public int AxePower = 10;
}

public class ResourceGatherer : ResourceGatherer<ResourceGatherer> { }
using System;
using System.Collections.Generic;
using UnityEngine;

public class FactionMember<T> : GameEntity<T> where T : FactionMember<T>
{
    public Faction Faction = null;
    public Base Base => Faction.Base;

    public void InitFaction(Faction faction)
    {
        Faction = faction;
    }

    [Obsolete("May be removed in the future. Do not use if possible.")]
    private Transform FindClosestTeammate(List<Transform> targets)
    {
        Transform closestTransform = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (Transform t in targets)
        {
            if (t.gameObject.GetComponent<FactionMember<T>>().Faction == Faction)
            {
                float dist = Vector2.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    closestTransform = t;
                    minDist = dist;
                }
            }
        }

        return closestTransform;
    }
}
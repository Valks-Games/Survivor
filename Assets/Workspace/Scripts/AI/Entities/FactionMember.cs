using System;
using System.Collections.Generic;
using UnityEngine;

public class FactionMember<T> : GameEntity<T> where T : FactionMember<T>
{
    public Faction Faction = null;
    public Base Base => Faction.Base;

    public void InitFaction(Faction faction) =>
        Faction = faction;
}
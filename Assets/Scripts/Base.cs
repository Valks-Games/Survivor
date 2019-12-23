using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int Wood = 0;

    public void AddWood(int amount) {
        Wood += amount;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : Structure
{
    private Text _textWood;
    private Text _textStone;

    public int costMultiplier = 10;

    private int tier = 1;

    public int Wood { get; private set; } = 0;
    public int Stone { get; private set; } = 0;

    public void Start()
    {
        _textWood = GameObject.Find("Wood").GetComponent<Text>();
        _textStone = GameObject.Find("Stone").GetComponent<Text>();
    }

    public void DepositResource(string type, int amount) {
        switch (type) {
            case "Wood":
                Wood += amount;
                _textWood.text = "Wood: " + Wood;
                break;
            case "Stone":
                Stone += amount;
                _textStone.text = "Stone: " + Stone;
                break;
        }
    }

    public void Upgrade() {
        if (CanUpgrade()) {
            Wood -= tier * costMultiplier;
            Stone -= tier * costMultiplier;

            tier++;
            Debug.Log("Upgraded base to tier " + tier);
        }
    }

    public bool CanUpgrade() {
        List<Resource> reqResources = ResourcesRequired();

        return reqResources.Count <= 0;
    }

    public List<Resource> ResourcesRequired() {
        List<Resource> reqResources = new List<Resource>();

        int reqWood = tier * costMultiplier;
        int reqStone = tier * costMultiplier;

        int woodNeeded = Wood - reqWood;
        if (woodNeeded < 0) {
            for (int i = 0; i < Mathf.Abs(woodNeeded); i++) {
                reqResources.Add(new Wood());
            }
        }

        int stoneNeeded = Stone - reqStone;
        if (stoneNeeded < 0) {
            for (int i = 0; i < Mathf.Abs(stoneNeeded); i++) {
                reqResources.Add(new Stone());
            }
        }

        return reqResources;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : Structure
{
    public static int id = 0;
    private Text _textWood;
    private Text _textStone;

    public int costMultiplier = 10;

    private int tier = 1;

    public int Wood { get; private set; } = 0;
    public int Stone { get; private set; } = 0;

    public override void Awake()
    {
        base.Awake();
        id++;
        SetName("Base " + id);
        SetParent("Bases");
    }

    public override void Start()
    {
        base.Start();
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
        Dictionary<string, int> reqResources = ResourcesRequired();

        return reqResources["Wood"] == 0 && reqResources["Stone"] == 0;
    }

    public Dictionary<string, int> ResourcesRequired() {
        Dictionary<string, int> reqResources = new Dictionary<string, int>();

        int reqWood = tier * costMultiplier;
        int reqStone = tier * costMultiplier;

        reqResources.Add("Wood", 0);
        reqResources.Add("Stone", 0);

        int woodNeeded = Wood - reqWood;
        if (woodNeeded < 0) {
            reqResources["Wood"] = Mathf.Abs(woodNeeded);
        }

        int stoneNeeded = Stone - reqStone;
        if (stoneNeeded < 0) {
            reqResources["Stone"] = Mathf.Abs(stoneNeeded);
        }

        return reqResources;
    }
}

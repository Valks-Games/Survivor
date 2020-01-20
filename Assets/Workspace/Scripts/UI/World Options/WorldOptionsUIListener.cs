using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldOptionsUIListener : MonoBehaviour
{
    public Transform SectionMenu;

    private UIInputField inputFieldSeed;
    private UIToggle toggleRandomizeSeed;
    private UIButton buttonStart;

    public void Start()
    {
        inputFieldSeed = new UIInputField("Input Seed", SectionMenu);
        toggleRandomizeSeed = new UIToggle("Randomize Seed", SectionMenu);
        buttonStart = new UIButton("Create World", SectionMenu);

        inputFieldSeed.UpdatePlaceholder("Enter world seed...");
        inputFieldSeed.AddListener(() =>
        {
            World.StringSeed = inputFieldSeed.Instance.text;
        });

        toggleRandomizeSeed.AddListener(() =>
        {
            World.RandomizeSeed = !World.RandomizeSeed;
        });

        buttonStart.AddListener(() =>
        {
            StartCoroutine(Utils.LoadAsynchronously("Loading"));
        });
    }
}

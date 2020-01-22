using UnityEngine;

public class WorldOptionsUIListener : MonoBehaviour
{
    public GameObject InputFieldSeed;
    public GameObject ToggleRandomizeSeed;
    public GameObject ButtonCreateWorld;

    private UIInputField inputFieldSeed;
    private UIToggle toggleRandomizeSeed;
    private UIButton buttonStart;

    public void Start()
    {
        inputFieldSeed = new UIInputField(InputFieldSeed);
        toggleRandomizeSeed = new UIToggle(ToggleRandomizeSeed);
        buttonStart = new UIButton(ButtonCreateWorld);

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
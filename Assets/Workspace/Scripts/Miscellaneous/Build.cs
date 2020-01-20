using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Build : MonoBehaviour
{
    [Header("Version")]
    public string version;

    [Header("Patch Notes")]
    [TextArea(5, 20)]
    public string notesNewFeatures;

    [TextArea(5, 20)]
    public string notesChanges;

    [TextArea(5, 20)]
    public string notesBugFixes;

    [Header("Links")]
    public GameObject TVersion;

    public GameObject TTitleNewFeatures;
    public GameObject TContentNewFeatures;
    public GameObject TTitleChanges;
    public GameObject TContentChanges;
    public GameObject TTitleBugFixes;
    public GameObject TContentBugFixes;

    private TextMeshProUGUI textVersion;
    private TextMeshProUGUI textNewFeatures;
    private TextMeshProUGUI textChanges;
    private TextMeshProUGUI textBugFixes;

    private Transform TPatchNotes;

    // This could go into a scene called "Release Notes"
    // All these text elements could be created from scratch instead.
    // This is very incomplete (we could add buttons in the inspector to increment the version (MAJOR.MINOR.PATCH))
    public void Start()
    {
        TPatchNotes = GameObject.Find("Patch Notes").transform;
        StartCoroutine(UpdateVerticalLayoutSpacing());

        textVersion = TVersion.GetComponent<TextMeshProUGUI>();
        textVersion.text = version;

        textNewFeatures = TContentNewFeatures.GetComponent<TextMeshProUGUI>();
        textNewFeatures.text = notesNewFeatures;
        if (textNewFeatures.text == "")
        {
            TTitleNewFeatures.SetActive(false);
            TContentNewFeatures.SetActive(false);
        }

        textChanges = TContentChanges.GetComponent<TextMeshProUGUI>();
        textChanges.text = notesChanges;
        if (textChanges.text == "")
        {
            TTitleChanges.SetActive(false);
            TContentChanges.SetActive(false);
        }

        textBugFixes = TContentBugFixes.GetComponent<TextMeshProUGUI>();
        textBugFixes.text = notesBugFixes;
        if (textBugFixes.text == "")
        {
            TTitleBugFixes.SetActive(false);
            TContentBugFixes.SetActive(false);
        }
    }

    // Obviously a temporary measure..
    private IEnumerator UpdateVerticalLayoutSpacing()
    {
        yield return new WaitForSeconds(0.001f);
        TPatchNotes.GetComponent<VerticalLayoutGroup>().spacing = 11;
    }
}
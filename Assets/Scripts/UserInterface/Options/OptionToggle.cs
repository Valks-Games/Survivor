using UnityEngine;
using UnityEngine.UI;

public class OptionToggle : Option
{
    public Toggle Instance => (Toggle)selectable;

    public OptionToggle(Selectable selectable, Color32 color) : base(selectable, color)
    {

    }
}
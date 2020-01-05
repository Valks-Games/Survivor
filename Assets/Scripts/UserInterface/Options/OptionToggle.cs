using UnityEngine;
using UnityEngine.UI;

public class OptionToggle : Option
{
    public Toggle Instance => (Toggle)selectable;

    public OptionToggle(string name) : base(name)
    {

    }
}
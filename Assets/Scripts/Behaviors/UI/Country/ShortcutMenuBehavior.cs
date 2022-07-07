using UnityEngine;

public class ShortcutMenuBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public void OpenDiamondMenu()
    {
        DiamondMenuBehavior.Instance.Show();
    }
}

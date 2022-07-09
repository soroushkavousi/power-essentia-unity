using UnityEngine;

public class ShortcutMenuBehavior : MonoBehaviour
{
    public void OpenDiamondMenu()
    {
        DiamondMenuBehavior.Instance.Show();
    }
}

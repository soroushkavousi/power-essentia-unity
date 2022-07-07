using UnityEngine;

public class PreparationMenuBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public void StartMission()
    {
        SceneManagerBehavior.Instance.LoadMission();
    }

    public void LoadLevel()
    {
        MusicPlayerBehavior.Instance.PlayClickSound();
        SceneManagerBehavior.Instance.LoadMission();
    }

    public void LoadDiamondMenu()
    {
        DiamondMenuBehavior.Instance.Show();
    }
}

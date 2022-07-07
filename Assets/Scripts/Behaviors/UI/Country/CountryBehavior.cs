using UnityEngine;

public class CountryBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public void Fight()
    {
        SceneManagerBehavior.Instance.LoadMission();
    }
}

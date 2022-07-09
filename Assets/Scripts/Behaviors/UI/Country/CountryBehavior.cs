using UnityEngine;

public class CountryBehavior : MonoBehaviour
{
    public void Fight()
    {
        SceneManagerBehavior.Instance.LoadMission();
    }
}

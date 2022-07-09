using UnityEngine;

public class CountryMenuBehavior : MonoBehaviour
{
    public void BackToStartScene()
    {
        SceneManagerBehavior.Instance.LoadStart();
    }
}

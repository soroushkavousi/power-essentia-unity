using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneBehavior : MonoBehaviour
{
    public void GoToWorld()
    {
        MusicPlayerBehavior.Instance.PlayClickSound();
        SceneManagerBehavior.Instance.LoadCountry();
    }

    public void OpenOptionsDialog()
    {
        MusicPlayerBehavior.Instance.PlayClickSound();
    }
}

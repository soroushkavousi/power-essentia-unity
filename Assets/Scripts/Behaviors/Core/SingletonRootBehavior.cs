using UnityEngine;

public class SingletonRootBehavior : MonoBehaviour
{
    public static SingletonRootBehavior Instance { get; private set; }

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        //var singletonObjects = FindObjectsOfType(GetType());
        //var duplicates = singletonObjects.Where(
        //    sigletonObject => sigletonObject.name == name).ToList();
        //if (duplicates.Count > 1)
        //    Destroy(gameObject);
        //else
        //{
        //    _instance =
        //    DontDestroyOnLoad(gameObject);
        //}
    }
}


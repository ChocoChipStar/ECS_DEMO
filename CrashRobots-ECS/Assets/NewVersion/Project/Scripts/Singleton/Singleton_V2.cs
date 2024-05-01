using UnityEngine;

public class Singleton_V2<T> : MonoBehaviour where T : MonoBehaviour
{
    private static volatile T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>(true);

            return instance;
        }
    }
}
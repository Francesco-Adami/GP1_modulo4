using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singleton Pattern")]
    public bool DontDestroyOnLoadEnabled = true;

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    throw new System.Exception($"Singleton of type {typeof(T)} is not initialized.");
                }

                if (_instance.GetComponent<Singleton<T>>().DontDestroyOnLoadEnabled)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }

            return _instance;
        }
    }
}

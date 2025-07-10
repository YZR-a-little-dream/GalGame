using UnityEngine;

public class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    { get => instance; private set => instance = value; }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
        }
    }
}

public class SingletonDontDestory<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    private  void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);      
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}




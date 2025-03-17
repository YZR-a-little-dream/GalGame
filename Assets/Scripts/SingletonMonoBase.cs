using UnityEngine;

public class SingletonMonoBase<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    public static T Instance
        {get => instance;private set => instance = value;}
    
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



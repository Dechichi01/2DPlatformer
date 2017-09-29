using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Singleton pattern for Monobehaviour. As for now haven't found a suitable way to make it thread safe.
/// </summary>
/// <typeparam name="T">Subclass type. Ex: SubClass : SingletonMono<SubClass></typeparam>
public abstract class SingletonMonoNoPersist<T> : MonoBehaviour where T : SingletonMonoNoPersist<T>, new()
{
    protected static T _instance;
    
    public static T instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();
            if (_instance == null)
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = (T)this;
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}

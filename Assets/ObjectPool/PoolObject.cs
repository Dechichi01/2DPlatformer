using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour
{
    public int instancesOnFirstPool = 5;
    public int instancesOnNewPool = 3;
    [System.NonSerialized]
    public Transform poolHolder;
    [System.NonSerialized]
    public int poolKey;
    [System.NonSerialized]
    public bool parentIsPooledObject = false;

    public virtual T Instantiate<T>(Vector3 position, Quaternion rotation) where T : PoolObject
    {
        return PoolManager.instance.ReuseObject<T>(this, position, rotation);
    }
    public virtual T Instantiate<T>() where T : PoolObject
    {
        return PoolManager.instance.ReuseObject<T>(this);
    }

    public virtual void Reuse(Vector3 position, Quaternion rotation)
    {
        transform.parent = null;
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Disable object and send it back to the pool. Recommend to always be called with a delay.
    /// </summary>
    public virtual void Destroy(float delay = 0f)
    {
        if (parentIsPooledObject) return;

        transform.parent = null;
        if (delay != 0 && gameObject.activeSelf)
            StartCoroutine(DestroyWithDelay(delay));
        else
        {
            gameObject.SetActive(false);
            PoolManager.instance.poolDictionary[poolKey].Enqueue(this);
            transform.parent = poolHolder;
        }
    }

    IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy();
    }
}

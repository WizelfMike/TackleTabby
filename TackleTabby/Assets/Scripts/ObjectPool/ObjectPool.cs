using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
    where T : Component
{
    private HashSet<T> _pool = new();

    public bool Store(T obj)
    {
        if (_pool.Contains(obj))
            return false;

        _pool.Add(obj);
        obj.transform.position = transform.position;
        obj.gameObject.SetActive(false);
        return true;
    }

    public T Retrieve()
    {
        if (_pool.Count == 0)
        {
            return GetFromBaseTemplate();
        }

        T first = _pool.First();
        _pool.Remove(first);
        
        first.gameObject.SetActive(true);
        return first;
    }

    protected virtual T GetFromBaseTemplate()
    {
        throw new NotImplementedException();
    }
}
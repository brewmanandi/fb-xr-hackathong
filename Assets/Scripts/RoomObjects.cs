using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// holds reference and handles logic for all scanned room objects.
/// </summary>
public class RoomObjects
{
    public List<GameObject> Objects { get; private set; } = new List<GameObject>();

    public GameObject GetObject(string name)
    {
        return Objects.FirstOrDefault(o => o.name.Equals(name));
    }

    public bool TryAdd(GameObject obj)
    {
        if (Objects.Contains(obj))
        {
            return false;
        }
        Objects.Add(obj);
        return true;
    }

    public void AddRange(List<GameObject> obj)
    {
        Objects.AddRange(obj);
    }

    public bool TryRemove(GameObject obj)
    {
        if (!Objects.Contains(obj))
        {
            return false;
        }
        Objects.Remove(obj);
        return true;
    }

    public void DestroyAll()
    {
        Objects.ForEach(o => GameObject.Destroy(o));
        Objects.Clear();
    }
}

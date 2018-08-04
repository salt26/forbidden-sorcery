using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    private static PrefabManager instance;
    public static PrefabManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PrefabManager();
            }
            return instance;
        }
    }

    private Dictionary<string, GameObject> cachedPrefab = new Dictionary<string, GameObject>();

    public GameObject GetPrefab(string name)
    {
        if (cachedPrefab.ContainsKey(name))
        {
            return cachedPrefab[name];
        }

        var loadedPrefab = Resources.Load<GameObject>(string.Format("Prefabs/{0}", name));

        if (loadedPrefab != null)
        {
            cachedPrefab.Add(name, loadedPrefab);
        }

        return loadedPrefab;
    }
}

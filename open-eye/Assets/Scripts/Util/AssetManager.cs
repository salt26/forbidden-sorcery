using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager
{
    private static AssetManager instance;
    public static AssetManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AssetManager();
            }
            return instance;
        }
    }

    private Dictionary<string, GameObject> cachedPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> cachedSprite = new Dictionary<string, Sprite>();
    private Dictionary<string, UnitData> cachedUnitData = new Dictionary<string, UnitData>();
    
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

    public Sprite GetSprite(string name)
    {
        if (cachedSprite.ContainsKey(name))
        {
            return cachedSprite[name];
        }

        var loadedSprite = Resources.Load<Sprite>(string.Format("Sprites/{0}", name));

        if (loadedSprite != null)
        {
            cachedSprite.Add(name, loadedSprite);
        }

        return loadedSprite;
    }

    public UnitData GetUnitData(string name)
    {
        if (cachedUnitData.ContainsKey(name))
        {
            return cachedUnitData[name];
        }

        var loadedUnitData = Resources.Load<UnitData>(string.Format("UnitDatas/{0}", name));

        if (loadedUnitData != null)
        {
            cachedUnitData.Add(name, loadedUnitData);
        }

        return loadedUnitData;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public const string EnemyPath = "Prefabs/Enemy";
    
    Dictionary<string, GameObject> EnemyFileCache = new Dictionary<string, GameObject>();

    public GameObject Load(string resoucePath)
    {
        GameObject go = null;

        
        // resoucePath값 체크
        if (EnemyFileCache.ContainsKey(resoucePath))
        {
            go = EnemyFileCache[resoucePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resoucePath);
            if (!go)
            {
                Debug.LogError("Load error! path = "+ resoucePath);
                return null;
            }
            EnemyFileCache.Add(resoucePath, go);
        }

        return Instantiate<GameObject>(go);
    }
}

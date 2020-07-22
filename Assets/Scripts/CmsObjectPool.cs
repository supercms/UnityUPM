using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmsObjectPool : CmsSingleton<CmsObjectPool>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject pfb;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> dicObjPool = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.pfb);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }

            dicObjPool.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 p, Quaternion r)
    {
        if (!dicObjPool.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject go = dicObjPool[tag].Dequeue();
        go.SetActive(true);
        go.transform.position = p;
        go.transform.rotation = r;

        //dicObjPool[tag].Enqueue(go);

        return go;
    }

    public void ReturnObjectToPool(string tag, GameObject go)
    {
        if (dicObjPool.ContainsKey(tag))
        {
            dicObjPool[tag].Enqueue(go);
            go.SetActive(false);
        }
        else
        {
            Debug.Log(tag + " object pool is not available");
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class ContentBasedTextureCache : MonoBehaviour
{
    private Dictionary<string, Item> _cache = new Dictionary<string, Item>();

    public void Start()
    {
        DisposeAllCache();
    }

    public void Put(string key, Texture2D content, float expiration)
    {
        //Debug.Log("PUT "+key);
        Item item;
        if (_cache.ContainsKey(key))
        {
            PurgeTextureFromCache(key);
        }
        item = new Item();
        item.key = key;
        item.expiration = expiration;
        item.texture = content;
        item.expired = Time.time + expiration;
        _cache[key] = item;
    }

    public Texture2D Get(string key)
    {
        if (!_cache.ContainsKey(key))
            return null;
        Item item = _cache[key];
        item.expired = Time.time + item.expiration;
        return item.texture;
    }

    public bool Contains(string key)
    {
        return _cache.ContainsKey(key);
    }

    public void Cleanup()
    {
        List<string> removeList = new List<string>();
        foreach (Item item in _cache.Values)
        {
            if (Time.time > item.expired)
                removeList.Add(item.key);
        }
        foreach (string key in removeList)
        {
            //Debug.Log("EXPIRED " + key);
            PurgeTextureFromCache(key);
        }
    }

    private void PurgeTextureFromCache(string key)
    {
        Texture2D texture = _cache[key].texture;
        _cache[key].texture = null;
        DestroyTexture(texture);
        _cache.Remove(key);
    }

    private void DestroyTexture(Texture2D texture)
    {
        if (Application.isEditor)
        {
            DestroyImmediate(texture);
        }
        else
        {
            Destroy(texture);
        }
    }

    private class Item
    {
        public string key;
        public float expiration;
        public Texture2D texture;
        public float expired;
    }

    public void OnDisable()
    {
        DisposeAllCache();
    }

    private void DisposeAllCache()
    {
        //Debug.Log("disposing");
        List<string> removeList = new List<string>();
        foreach (Item item in _cache.Values)
        {
            //Debug.Log("remove list:" + item.key);
            removeList.Add(item.key);
        }
        foreach (string key in removeList)
        {
            //Debug.Log("cleaning " + key);
            PurgeTextureFromCache(key);
        }
        Resources.UnloadUnusedAssets();
    }
}


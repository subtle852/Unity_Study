﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
            Debug.Log($"Failed to load prefab : {path}");

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;

        Object.Destroy(go, time);
    }
}

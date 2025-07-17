using System.Collections;
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
        // 1. original 이미 들고 있으면, 바로 사용
        GameObject original = Resources.Load<GameObject>($"Prefabs/{path}");
        if (original == null)
            Debug.Log($"Failed to load prefab : {path}");

        // 2. 풀링된 것이 있으면, 그걸 사용
        GameObject go = Object.Instantiate(original, parent);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);

        return go;
    }

    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;

        // 풀링이 필요하다면, Destroy하지 않고 풀링 매니저에게 보내기

        Object.Destroy(go, time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    //GameObject prefab;

    GameObject go;
    void Start()
    {
        //prefab = Resources.Load<GameObject>("Prefabs/Sword");
        //go = Instantiate(prefab);
        //Destroy(go, 3.0f);

        go = Managers.Resource.Instantiate("Sword");
        Destroy(go, 3.0f);
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionTest3 : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // LayerMask 활용
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            //int mask = (1 << 8) | (1 << 9);
            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Debug.Log($"Raycast Camera {hit.collider.gameObject.name}");
            }

        }

        // Tag 활용
        GameObject go = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject[] gos = GameObject.FindGameObjectsWithTag("MainCamera");

        //Debug.Log($"Raycast Camera {hit.collider.gameObject.tag}");
    }
}

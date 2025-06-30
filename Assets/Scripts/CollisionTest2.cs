using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest2 : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 worldDir = worldMousePos -Camera.main.transform.position;
            worldDir = worldDir.normalized;

            Debug.DrawRay(Camera.main.transform.position, worldDir * 100.0f, Color.red, 1.0f);

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, worldDir, out hit, 100.0f))
            {
                Debug.Log($"Raycast Camera {hit.collider.gameObject.name}");
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            // 간소화 버전
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log($"Raycast Camera {hit.collider.gameObject.name}");
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TestEvent()
    {
        Debug.Log($"Event is called");
    }

    public void TestEvent2(int a)
    {
        Debug.Log($"Event is called {a}");
    }
}

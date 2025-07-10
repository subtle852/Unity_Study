using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public event Action<PointerEventData> onBeginDragHandler = null;
    public event Action<PointerEventData> onDragHandler = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(onBeginDragHandler != null)
            onBeginDragHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDragHandler != null)
            onDragHandler.Invoke(eventData);
    }
}

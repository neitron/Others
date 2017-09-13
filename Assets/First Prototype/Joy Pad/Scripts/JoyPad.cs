using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



/// <summary>
/// Class for handle pointer drag events and present the JoyPad on a screen
/// </summary>
[RequireComponent(typeof(RectTransform), typeof(Canvas))]
public class JoyPad : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{


    enum JoysticState
    {
        Idle,
        EndDrag,
        BeginDrag,
        Drag
    }



    Canvas rootCanvas;
    RectTransform pad;
    RectTransform stick;

    JoysticState joysticState;



    // Use this for initialization
    void Start ()
    {
        rootCanvas = GetComponent<Canvas>().rootCanvas;
        pad = transform.Find("Pad").GetComponent<RectTransform>();
        stick = pad.Find("Stick").GetComponent<RectTransform>();
	}


    public bool GetJosticDrag()
    {
        return joysticState == JoysticState.Drag;
    }


    public Vector2 GetJosticPosition()
    {
        return stick.anchoredPosition;
    }
    

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        joysticState = JoysticState.BeginDrag;
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        joysticState = JoysticState.Drag;

        Vector2 newPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(pad, eventData.position, rootCanvas.worldCamera, out newPosition);

        newPosition -= pad.rect.size * 0.5f;
        if (newPosition.magnitude > pad.rect.width * 0.5f)
        {
            newPosition = newPosition.normalized * pad.rect.width * 0.5f;
        }

        stick.anchoredPosition = newPosition;
    }


    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        joysticState = JoysticState.EndDrag;
        stick.anchoredPosition = Vector2.zero;

        StartCoroutine(JoysticToIdleState());
    }


    IEnumerator JoysticToIdleState()
    {
        yield return new WaitForEndOfFrame();
        joysticState = JoysticState.EndDrag;
        yield return null;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyPad : MonoBehaviour
{


    RectTransform pad;
    RectTransform stick;
    JoyPadStates state;

    public Action<Vector2> OnJoysticDrag;
    public Action OnJoysticRelease;

    enum JoyPadStates
    {
        Idle, Drag, Reset
    }

	// Use this for initialization
	void Start ()
    {
        pad = GetComponent<RectTransform>();
        stick = transform.Find("Stick").GetComponent<RectTransform>();
	}
	

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(pad, Input.mousePosition))
        {
            state = JoyPadStates.Drag;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state = JoyPadStates.Reset;
        }

        switch (state)
        {
            case JoyPadStates.Idle:
                break;

            case JoyPadStates.Drag:
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(pad, Input.mousePosition, null, out pos);

                pos -= pad.rect.size * 0.5f;
                if(pos.magnitude > pad.rect.width * 0.5f)
                {
                    pos = pos.normalized * pad.rect.width * 0.5f;
                }

                stick.anchoredPosition = pos;

                if(OnJoysticDrag != null)
                {
                    OnJoysticDrag(pos.normalized);
                }

                break;

            case JoyPadStates.Reset:
                stick.anchoredPosition = Vector2.zero;
                if (OnJoysticRelease != null)
                {
                    OnJoysticRelease();
                }
                state = JoyPadStates.Idle;
                break;

            default:
                break;
        }
    }


}

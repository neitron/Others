using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;

	// Use this for initialization
	void Start ()
    {
		if(instance == null)
        {
            instance = this;
        }
	}

    internal RectTransform SpawnEnemyInfo(EnemyAI temp, RectTransform rectTransform)
    {
        var pos = Camera.main.WorldToScreenPoint(temp.gameObject.transform.position);

        return Instantiate<RectTransform>(rectTransform, pos, Quaternion.identity, this.transform);
    }

    internal void UpdateInfoPos(EnemyAI temp, RectTransform rectTransform)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(temp.gameObject.transform.position + Vector3.up * 0.3f);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * GetComponent<RectTransform>().sizeDelta.x) - (GetComponent<RectTransform>().sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * GetComponent<RectTransform>().sizeDelta.y) - (GetComponent<RectTransform>().sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }
}

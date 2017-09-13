using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;

	// Use this for initialization
	void Awake ()
    {
		if(instance == null)
        {
            instance = this;
        }
	}

    internal RectTransform SpawnEnemyInfo(EnemyAI enemy, RectTransform enemyInfoOriginal)
    {
        var pos = Camera.main.WorldToScreenPoint(enemy.gameObject.transform.position);

        var enemyInfo = Instantiate<RectTransform>(enemyInfoOriginal, pos, Quaternion.identity, this.transform);

        enemyInfo.Find("Name").GetComponent<Text>().text = enemy.alias;

        return enemyInfo;
    }

    internal void UpdateInfoPos(EnemyAI enemy, RectTransform rectTransform)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(enemy.gameObject.transform.position + Vector3.up * 0.7f);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * GetComponent<RectTransform>().sizeDelta.x) - (GetComponent<RectTransform>().sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * GetComponent<RectTransform>().sizeDelta.y) - (GetComponent<RectTransform>().sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

    internal void UpdateInfo(EnemyAI enemyAI, RectTransform enemyInfo)
    {
        enemyInfo.Find("Health Bar").Find("Health Level").GetComponent<Image>().fillAmount = enemyAI.currentHealth / enemyAI.health;
    }
}

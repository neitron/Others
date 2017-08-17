using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{


    public float radius;

    public EnemyCanvas canvas;

    public GameObject enemyOriginal;
    public RectTransform enemyInfo;
    public Transform enemyFocus;

    public float delay;
    public int amount;


    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(delay);
        for (int i = 0; i < amount; i++)
        {
            var pos = transform.position;

            pos.x += Random.Range(-radius, radius);
            pos.y += Random.Range(-radius, radius);
            pos.z += Random.Range(-radius, radius);

            EnemyAI temp = Instantiate<GameObject>(enemyOriginal, pos, Quaternion.identity).GetComponent<EnemyAI>();
            temp.SetFocus(enemyFocus);

            var rect = canvas.SpawnEnemyInfo(temp, enemyInfo);
            temp.Init(rect);

            yield return wait;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}

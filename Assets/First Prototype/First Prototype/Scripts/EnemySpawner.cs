using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{


    public float radius;

    public EnemyCanvas canvas;

    public List<Enemy> enemies;
    public RectTransform enemyInfoOriginal;
    public Transform enemyFocus;

    public float delay;
    public int amount;
    


    private void Start()
    {
        DoNewWave();
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

            var enemyType = Random.Range(0, 2);

            EnemyAI temp = Instantiate<GameObject>(enemies[enemyType].prefab, pos, Quaternion.identity).GetComponent<EnemyAI>();
            temp.gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", enemies[enemyType].colorIsRandom ? Random.ColorHSV() : enemies[enemyType].tintColor);
            temp.SetFocus(enemyFocus);
            temp.currentHealth = enemies[enemyType].health;
            temp.health = enemies[enemyType].health;

            var rect = canvas.SpawnEnemyInfo(temp, enemies[enemyType], enemyInfoOriginal);
            temp.Init(rect);

            yield return wait;
        }
    }


    public void DoNewWave()
    {
        StartCoroutine(Spawn());
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{

    public Vector3 focus;

    [Space(20), Header("Enemy info")]
    public string alias;
    public float health;
    [HideInInspector]
    public float currentHealth;

    [HideInInspector]
    public RectTransform enemyInfo;

    private NavMeshAgent agent;



	// Use this for initialization
	void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = health;
    }

    internal void Init(RectTransform enemyInfo)
    {
        this.enemyInfo = enemyInfo;
        EnemyCanvas.instance.UpdateInfo(this, enemyInfo);
    }

    internal void SetFocus(Transform enemyFocus)
    {
        agent.SetDestination(enemyFocus.position);
    }

    public void Update()
    {
        EnemyCanvas.instance.UpdateInfoPos(this, enemyInfo);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            currentHealth -= UnityEngine.Random.Range(50, 55);
            EnemyCanvas.instance.UpdateInfo(this, enemyInfo);
        }
    }
}

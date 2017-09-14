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
    //public string alias;
    //public float health;
    [HideInInspector]
    public float currentHealth;
    public float health;

    [HideInInspector]
    public RectTransform enemyInfo;

    private NavMeshAgent agent;



	// Use this for initialization
	void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
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

    public void HitEnemy(int damage)
    {
        currentHealth -= damage;
        EnemyCanvas.instance.UpdateInfo(this, enemyInfo);

        if(currentHealth <= 0)
        {
            Destroy(enemyInfo.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
    }
}

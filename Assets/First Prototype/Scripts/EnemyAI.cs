using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 focus;

    public float helth;

    [HideInInspector]
    public RectTransform enemyInfo;

	// Use this for initialization
	void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
	}

    internal void Init(RectTransform enemyInfo)
    {
        this.enemyInfo = enemyInfo;
    }

    internal void SetFocus(Transform enemyFocus)
    {
        agent.SetDestination(enemyFocus.position);
    }

    public void Update()
    {
        EnemyCanvas.instance.UpdateInfoPos(this, enemyInfo);
    }
}

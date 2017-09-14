using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewEnemyType", menuName = "Game Scriptable Object/Enemy", order = 0)]
public class Enemy : ScriptableObject
{

    [Header("Look")]
    public string objectName = "Enemy Name";

    public GameObject prefab;

    public bool colorIsRandom = true;
    public Color tintColor = Color.white;


    [Header("Stats")]
    public float health;


}

using System;
using System.Collections;
using UnityEngine;


public class TowerController : MonoBehaviour
{


    public GameObject coinOriginal;

    public Vector2 radius;
    public Vector2 coinGenerationDelay;



    private void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        while(true)
        {
            SpawnCoin();

            var wait = UnityEngine.Random.Range((int)coinGenerationDelay.x, (int)coinGenerationDelay.y);
            yield return new WaitForSeconds(wait);
        }
    }

    private void SpawnCoin()
    {
        var pos = Utils.Randomf(radius) * new Vector3(Utils.Randomf(-radius.y, radius.y), 0.0f, Utils.Randomf(-radius.y, radius.y)).normalized;
        pos.y = 2.0f;
        Instantiate<GameObject>(coinOriginal, pos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius.y);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, radius.x);
    }
}
